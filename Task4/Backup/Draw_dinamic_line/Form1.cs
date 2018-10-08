using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Draw_dinamic_line
{
    public partial class Form1 : Form
    {
        List<twoPoints> points = new List<twoPoints>();
        Point point1, point2;
        bool paint = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            point1 = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                point2 = e.Location;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            points.Add(new twoPoints(point1, point2));
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (twoPoints tp in points)
            {
                e.Graphics.DrawLine(Pens.Red, tp.p1, tp.p2);
            }
            e.Graphics.DrawLine(Pens.Red, point1, point2);
        }
    }
}