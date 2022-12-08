using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DIP1_IS_Bacalso
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed, imageA, imageB, colorGreen;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            for(int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color copyPixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, copyPixel);
                }
            }
            ProcessedImage.Image = processed;
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color copyPixel = loaded.GetPixel(x, y);
                    int greyscale = (copyPixel.R + copyPixel.G + copyPixel.B) / 3;
                    processed.SetPixel(x, y, Color.FromArgb(greyscale, greyscale, greyscale));
                }
            }
            ProcessedImage.Image = processed;
        }

        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color copyPixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, Color.FromArgb((255 - copyPixel.R), (255 - copyPixel.G), (255 - copyPixel.B)));
                }
            }
            ProcessedImage.Image = processed;
        }


        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            InputImage.Image = loaded;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Histogram(ref loaded, ref processed);
            ProcessedImage.Image = processed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog2.FileName);
            ImageA.Image = imageA;
        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog3.FileName);
            ImageB.Image = imageB;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(imageB.Width, imageB.Height);

            Color myGreen = Color.FromArgb(0, 255, 0);
            int greyGreen = (myGreen.R + myGreen.G + myGreen.B) / 3;
            int threshold = 5;

            for(int x = 0; x < imageB.Width; x++)
            {
                for(int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageA.GetPixel(x, y);
                    Color backPixel = imageB.GetPixel(x, y);

                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractedValue = Math.Abs(grey - greyGreen);

                    if (subtractedValue < threshold)
                        processed.SetPixel(x, y, backPixel);
                    else
                        processed.SetPixel(x, y, pixel);
                }
            }

            subtractedImage.Image = processed;
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color copyPixel = loaded.GetPixel(x, y);
                    int tr = (int)(0.393 * copyPixel.R + 0.769 * copyPixel.G + 0.189 * copyPixel.B);
                    int tg = (int)(0.349 * copyPixel.R + 0.686 * copyPixel.G + 0.168 * copyPixel.B);
                    int tb = (int)(0.272 * copyPixel.R + 0.534 * copyPixel.G + 0.131 * copyPixel.B);

                    int r, g, b;

                    if (tr > 255)
                        r = 255;
                    else
                        r = tr;

                    if (tg > 255)
                        g = 255;
                    else
                        g = tg;

                    if (tb > 255)
                        b = 255;
                    else
                        b = tb;

                    processed.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            ProcessedImage.Image = processed;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            using (Graphics g = Graphics.FromImage(processed))
            {
                g.DrawImage(processed, new Rectangle(0, 0, processed.Width, processed.Height));
                processed.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
            }
        }

        private static void Histogram(ref Bitmap a, ref Bitmap b)
        {
            Color sample;
            Color gray;
            Byte graydata;
            //Grayscale Convertion;
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    sample = a.GetPixel(x, y);
                    graydata = (byte)((sample.R + sample.G + sample.B) / 3);
                    gray = Color.FromArgb(graydata, graydata, graydata);
                    a.SetPixel(x, y, gray);
                }
            }

            //histogram 1d data;
            int[] histdata = new int[256]; // array from 0 to 255
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    sample = a.GetPixel(x, y);
                    histdata[sample.R]++; // can be any color property r,g or b
                }
            }

            // Bitmap Graph Generation
            // Setting empty Bitmap with background color
            b = new Bitmap(256, 800);
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 800; y++)
                {
                    b.SetPixel(x, y, Color.White);
                }
            }
            // plotting points based from histdata
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < Math.Min(histdata[x] / 5, b.Height - 1); y++)
                {
                    b.SetPixel(x, (b.Height - 1) - y, Color.Black);
                }
            }
        }
    }
}
