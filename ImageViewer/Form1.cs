using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace ImageViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "JPG/XML(*.jpg,*.xml)|*.jpg;*.xml|모든 파일(*.*)|*.*";
            saveFileDialog1.Filter = "XML(*.xml)|*.xml|모든 파일(*.*)|*.*";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                return;

            try
            {
                //엑셀파일
                if (openFileDialog1.FileName.EndsWith(".xml"))
                {
                    StreamReader sr = new StreamReader(openFileDialog1.FileName);
                    XmlSerializer xs = new XmlSerializer(typeof(ImageData));

                    ImageData semi = (ImageData)xs.Deserialize(sr);
                    Bitmap bmp = new Bitmap(semi.width, semi.height);

                    for (int i = 0; i < semi.height; i++)
                        for (int j = 0; j < semi.width; j++)
                        {
                            int temp = semi.pixel[i * semi.width + j];
                            bmp.SetPixel(j, i, System.Drawing.Color.FromArgb(temp));
                        }

                    pictureBox1.Width = bmp.Width;
                    pictureBox1.Height = bmp.Height;
                    pictureBox1.Image = bmp;

                    sr.Close();
                }
                else //그림파일
                {
                    Image img = Image.FromFile(openFileDialog1.FileName);

                    while (img.Width > 1000 || img.Height > 1000)
                    {
                        img = new Bitmap(img, new Size(img.Width / 2, img.Height / 2));
                    }

                    pictureBox1.Width = img.Width;
                    pictureBox1.Height = img.Height;
                    pictureBox1.Image = img;
                }
            }
            catch
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(!(saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                return;
            else if(pictureBox1.Image == null){
                return;
            }
            
            Bitmap bmp = new Bitmap(pictureBox1.Image); //그림 파일을 저장
            ImageData semi = new ImageData();
            semi.SetSize(bmp.Width,bmp.Height);

            for (int i = 0; i < semi.height; i++)
                for (int j = 0; j < semi.width; j++)
                    semi.pixel[i * semi.width + j] = bmp.GetPixel(j,i).ToArgb();

            XmlSerializer xs = new XmlSerializer(typeof(ImageData));
            StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false);

            xs.Serialize(sw, semi);

            sw.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }
    }
}
