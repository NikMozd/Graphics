using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            colorDialog1.Color = Color.Black;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); 
            g = Graphics.FromImage(pictureBox1.Image);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox1.Text = openFileDialog1.FileName;
            texture = new Bitmap(openFileDialog1.FileName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.Cursor == Cursors.Default)
                this.Cursor = Cursors.Hand;
            else this.Cursor = Cursors.Default;
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            pictureBox1.Invalidate();

            //pictureBox1.Dispose();
            //pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //g = Graphics.FromImage(pictureBox1.Image);

           // pictureBox1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button4.BackColor = colorDialog1.Color;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isPressed = true;
            Curr = e.Location;
        }
        
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPressed)
            {
                Prev = Curr;
                Curr = e.Location;
                Pen p = new Pen(colorDialog1.Color);
                g.DrawLine(p, Prev, Curr);
                pictureBox1.Invalidate();
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isPressed = false;
        }
        
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.Cursor == Cursors.Hand)
            {
                bclr = GetColor(e.X, e.Y);
                Curr = e.Location;
                fill(e.X, e.Y);
            }
        }

        private Color GetColor(int X, int Y)
        {
            Color clr = (pictureBox1.Image as Bitmap).GetPixel(X, Y);
            return clr;
        }
                
        private void fill (int x, int y)
        {
            if (GetColor(x, y) == bclr)
            {
                int xl = x, xr = x;
                while (--xl >= 0 && GetColor(xl, y) == bclr)
                    /* empty*/
                    ;
                while (++xr < pictureBox1.Width && GetColor(xr, y) == bclr)
                    /* empty*/
                    ;

                int ty = (y - Curr.Y) % texture.Height;
                if (ty < 0) ty += texture.Height;

                for (int i = xl + 1; i < xr; ++i)
                {
                    int tx = (i - Curr.X) % texture.Width;
                    if (tx < 0) tx += texture.Width;
                    (pictureBox1.Image as Bitmap).SetPixel(i, y, texture.GetPixel(tx, ty)); 
                }
               

                //Image img = Image.FromFile(openFileDialog1.FileName);
                //g.DrawImage(img, xl + 1, y, xr - 1, y);

                pictureBox1.Invalidate();
                
                if (y - 1 >= 0)
                {
                    for (int i = xl + 1; i < xr; ++i)
                        fill(i, y - 1);
                }
                if (y + 1 < pictureBox1.Height)
                {
                    for (int i = xl + 1; i < xr; ++i)
                        fill(i, y + 1);
                }
                    
            }
        }
        
        private Graphics g;
        private Color bclr;
        private bool isPressed = false;
        private Point Curr, Prev;
        private Bitmap texture;
    }
}
