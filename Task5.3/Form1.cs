using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            Points = new List<Point>(10);
        }

        private Graphics g;
        private int but = 0;
        private List<Point> Points;
        private Point dragP;
        private bool isDrawn = false;
        
        private void button3_Click(object sender, EventArgs e)
        {
            but = 0;
            button3.BackColor = Color.Lime;
            button1.BackColor = Color.Gainsboro;
            button4.BackColor = Color.Gainsboro;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            but = 1;
            button1.BackColor = Color.Lime;
            button3.BackColor = Color.Gainsboro;
            button4.BackColor = Color.Gainsboro;

        }

        private void button4_Click(object sender, EventArgs e)
        {

            but = 2;
            button4.BackColor = Color.Lime;
            button1.BackColor = Color.Gainsboro;
            button3.BackColor = Color.Gainsboro;
        }
        private void redrawPoints()
        {
            g.Clear(Color.White);
            SolidBrush b = new SolidBrush(Color.Black);
            for (int j = 0; j < Points.Count; ++j)
                g.FillEllipse(b, Points[j].X - 2, Points[j].Y - 2, 5, 5);

            pictureBox1.Invalidate();
        }
        private int  findPoint(Point e)
        {
            int i = 0;
            while (i < Points.Count && !(Math.Abs(Points[i].X - e.X) < 5 && Math.Abs(Points[i].Y - e.Y) < 5))
            {
                ++i;
            }
            if (i < Points.Count)
                return i;
            else return -1;
        }
        private int remPoint (Point e)
        {
            int i = findPoint(e);
            if (i != -1)
                Points.RemoveAt(i);
            redrawPoints();
            return i;
        }
        private void addPoint(Point e, int pos = -1)
        {
            if (pos != -1  && pos < Points.Count)
            Points.Insert(pos, e);
             else    Points.Add(e);
            SolidBrush b = new SolidBrush(Color.Black);
            g.FillEllipse(b, e.X-2, e.Y-2, 5, 5);

            pictureBox1.Invalidate();
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (but == 0)
            {
                addPoint(e.Location);
                if (isDrawn)
                {
                    redrawPoints();
                    DrawCompositeBezier();
                }
            }
            if (but == 1)
            {
                
                remPoint(e.Location);
                if (isDrawn)
                {
                    redrawPoints();
                    DrawCompositeBezier();
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (but == 2)
                dragP = e.Location;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (but == 2 && e.Location != dragP && findPoint(dragP) != -1)
             {
                 int pos = remPoint(dragP);
                 addPoint(e.Location, pos);
                 if (isDrawn)
                 {
                     redrawPoints();
                     DrawCompositeBezier();
                 }
             }
        }
        private void DrawBezier(Point p0, Point p1, Point p2)
        {
            Point p1_1 = new Point(p0.X + 2 * (p1.X - p0.X) / 3,
                                    p0.Y + 2 * (p1.Y - p0.Y) / 3);

            Point p1_2 = new Point(p1.X + (p2.X - p1.X) / 3,
                                p1.Y + (p2.Y - p1.Y) / 3);

            DrawBezier(p0, p1_1, p1_2, p2);

        }
        private void DrawBezier(Point p0, Point p1, Point p2, Point p3)
        {

            Point[] V = new Point[4] { p0, p1, p2, p3 }; 
            int[,] M = new int[4, 4] { { 1, -3, 3, -1 }, { 0, 3, -6, 3 }, { 0, 0, 3, -3 }, { 0, 0, 0, 1 } };
            float [] T = new float[4];

            Point[] VM = new Point[4] { new Point(0, 0), new Point(0, 0), new Point(0, 0), new Point(0, 0) };
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                {
                    VM[i].X += V[j].X * M[j, i];
                    VM[i].Y += V[j].Y * M[j, i];
                }

            for (float t = 0; t <= 1; t += (float)0.001)
            {
                T[0] = 1;
                T[1] = t;
                T[2] = t * t;
                T[3] = T[2] * t;

                PointF VMT = new PointF(0, 0);
                for (int i = 0; i < 4; ++i)
                {
                    VMT.X += VM[i].X * T[i];
                    VMT.Y += VM[i].Y * T[i];
                }

                (pictureBox1.Image as Bitmap).SetPixel((int)VMT.X, (int)VMT.Y, Color.Black);
            }
            pictureBox1.Invalidate();
        }

        private void DrawCompositeBezier()
        {
            if (Points.Count < 2)
                    return;

            if (Points.Count == 2)
            {
                Pen p = new Pen(Color.Black);
                g.DrawLine(p, Points[0], Points[1]);
                return;
            }

            Point Next, Prev;

            Prev = Points[0];
            for (int i = 0; i < Points.Count - 4; i += 2)
            {
                Next = new Point((Points[i + 2].X + Points[i + 3].X) / 2, (Points[i + 2].Y + Points[i + 3].Y) / 2);
                DrawBezier(Prev, Points[i + 1], Points[i + 2], Next);
                Prev = Next;
            }

            if (Points.Count % 2 == 0)
                DrawBezier(Prev, Points[Points.Count - 3], Points[Points.Count - 2], Points[Points.Count - 1]);
            else DrawBezier(Prev, Points[Points.Count - 2], Points[Points.Count - 1]);
            
        
        }
            

        private void button2_Click(object sender, EventArgs e)
        {
            if (isDrawn)
            {
                redrawPoints();
            }
            isDrawn = true;
            DrawCompositeBezier();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            isDrawn = false;
            Points.Clear();
            g.Clear(Color.White);
            pictureBox1.Invalidate();
        }


    }
}
