using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DIP1_IS_Bacalso
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed;
        
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

        private void brightnessLevel_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Brightness(ref loaded, ref processed, Convert.ToInt32(brightnessLevel.Text));
                ProcessedImage.Image = processed;
            }
        } 

        private static void Brightness(ref Bitmap a, ref Bitmap b, int value)
        {
            b = new Bitmap(a.Width, a.Height);
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    Color temp = a.GetPixel(x, y);
                    Color changed;
                    if (value > 0)
                        changed = Color.FromArgb(Math.Min(temp.R + value, 255), Math.Min(temp.G + value, 255), Math.Min(temp.B + value, 255));
                    else
                        changed = Color.FromArgb(Math.Max(temp.R + value, 0), Math.Max(temp.G + value, 0), Math.Max(temp.B + value, 0));

                    b.SetPixel(x, y, changed);
                }
            }
        }
    }
}
