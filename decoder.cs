using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CIa_decoder {
    class Decoder {
        private string _filetype;
        private List<string> _comments = new List<string>();
        private int _maxPixel;

        public string Filetype {
            get { return _filetype; }
        }

        public List<string> Comments {
            get { return _comments; }
        }

        public int MaxPixel {
            get { return _maxPixel; }
        }
        //This method generates a bitmap from the ppm file
        public Bitmap GenerateBitmap(string fileName) {
            //get all the info and comments at the top out of the way
            StreamReader inputFile = new StreamReader(fileName);
            _filetype = inputFile.ReadLine();
            bool commentsEnd = false;
            
            int count = 0;
            string LW = "";
            while (commentsEnd == false) {
                _comments.Add(inputFile.ReadLine());
                if (!_comments[count].Contains('#')) {
                    commentsEnd = true;
                    LW = _comments[count];
                    _comments.RemoveAt(count);
                }
                count++;
            }
            _maxPixel = int.Parse(inputFile.ReadLine());
            int length = 0;
            int width = 0;
            char[] lengthAndwidth = LW.ToCharArray();
            string num = "";
            
            //get the length and width from a single line and split them up
            for (int index = 0; index < lengthAndwidth.Length; index++) {
                if (lengthAndwidth[index] == ' ') {
                    width = int.Parse(num);
                    num = "";
                } else if (int.Parse(lengthAndwidth[index].ToString()) >= 0 && int.Parse(lengthAndwidth[index].ToString()) <= 9) {
                    num += lengthAndwidth[index];
                }
                if (int.TryParse(num, out _) && width != 0) length = int.Parse(num);

            }
            Bitmap newImg = new Bitmap(width, length);

            if (_filetype == "P3") {
                //if it is a P3 it will just set the colors to the new image
                for (int y = 0; y < length; y++) {
                    for (int x = 0; x < width; x++) {
                        int r = int.Parse(inputFile.ReadLine()); int g = int.Parse(inputFile.ReadLine()); int b = int.Parse(inputFile.ReadLine());
                        Color colCurrent = Color.FromArgb(r, g, b);
                        newImg.SetPixel(x, y, colCurrent);
                    }
                }
                inputFile.Close();
                return newImg;
            } else {
                //if its a P6 it closes Streamreader and open filestream
                inputFile.Close();
                FileStream p6ByteFile = new FileStream(fileName, FileMode.Open);
                char curByte;

                //this gets rid of everything at the top
                for (int index = 0; index < 3 + _comments.Count; index++) {
                    curByte = (char)p6ByteFile.ReadByte();

                    while (curByte != '\n') {
                        curByte = (char)p6ByteFile.ReadByte();
                    }

                }
                
                Bitmap img = new Bitmap(width, length);
                
                for (int y = 0; y < length; y++) {
                    for (int x = 0; x < width; x++) {
                        int r = p6ByteFile.ReadByte(); int g = p6ByteFile.ReadByte(); int b = p6ByteFile.ReadByte();
                        Color colCurrent = Color.FromArgb(r, g, b);
                        img.SetPixel(x, y, colCurrent);
                    }
                }
                p6ByteFile.Close();
                return img;
            }
        }
        //This method Builds The Dictionary
        public Dictionary<int, string> BuildDictionary() {
            Dictionary<int, string> codeDic = new Dictionary<int, string>();

            codeDic.Add(32, " ");
            codeDic.Add(48, "0");
            codeDic.Add(49, "1");
            codeDic.Add(50, "2");
            codeDic.Add(51, "3");
            codeDic.Add(52, "4");
            codeDic.Add(53, "5");
            codeDic.Add(54, "6");
            codeDic.Add(55, "7");
            codeDic.Add(56, "8");
            codeDic.Add(57, "9");
            codeDic.Add(97, "a");
            codeDic.Add(98, "b");
            codeDic.Add(99, "c");
            codeDic.Add(100, "d");
            codeDic.Add(101, "e");
            codeDic.Add(102, "f");
            codeDic.Add(103, "g");
            codeDic.Add(104, "h");
            codeDic.Add(105, "i");
            codeDic.Add(106, "j");
            codeDic.Add(107, "k");
            codeDic.Add(108, "l");
            codeDic.Add(109, "m");
            codeDic.Add(110, "n");
            codeDic.Add(111, "o");
            codeDic.Add(112, "p");
            codeDic.Add(113, "q");
            codeDic.Add(114, "r");
            codeDic.Add(115, "s");
            codeDic.Add(116, "t");
            codeDic.Add(117, "u");
            codeDic.Add(118, "v");
            codeDic.Add(119, "w");
            codeDic.Add(120, "x");
            codeDic.Add(121, "y");
            codeDic.Add(122, "z");

            return codeDic;
        }
        //This method Decodes the picture with the secret message
        public String DecodeMessage(Bitmap img, Dictionary<int, string> codeDic) {
            Color colCurrent = new Color();
            string usermessage = "";

            //this just loops through the image and checks for the numbers representing ascii values
            for (int y = 0; y < img.Height; y++) {
                for (int x = 0; x < img.Width; x++) {
                    colCurrent = img.GetPixel(x, y);

                    if (codeDic.ContainsKey(colCurrent.G)) usermessage += codeDic[colCurrent.G];
                }
            }

            return usermessage;
        }
    }
}
