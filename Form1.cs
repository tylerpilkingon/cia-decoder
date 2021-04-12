using System;
using System.Drawing;
using System.Windows.Forms;

namespace CIa_decoder {
    public partial class Form1 : Form {
        private Decoder CIAimage = new Decoder();
        public Form1() {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile("C:\\Users\\Tyler\\Pictures\\cia pic.png");
        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e) {
            DialogResult drgtemp = ofdLoadImage.ShowDialog();

            //check if user selected "open"
            if (drgtemp == DialogResult.OK) {

                richTextBox1.Text = null;

                CIAimage = new Decoder();
                string fileName = ofdLoadImage.FileName;
                Bitmap newImg = CIAimage.GenerateBitmap(fileName);
                picImage.Image = newImg;
                button1.Enabled =true;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            if (picImage.Image == null) MessageBox.Show("You have not put in a picture", "Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else {
                Bitmap img = new Bitmap(picImage.Image);
                string userMessage = CIAimage.DecodeMessage(img, CIAimage.BuildDictionary());
                richTextBox1.Text = userMessage;
            }
        }
    }
}
