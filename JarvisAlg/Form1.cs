using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JarvisAlg
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private List<Point> points = new List<Point>();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
        }

        private double cos_points(Point curr, Point prev, Point next)
        {
            if (curr == next)
                return double.MaxValue;

            Tuple<int, int> a = new Tuple<int, int>(prev.X - curr.X, prev.Y - curr.Y);
            Tuple<int, int> b = new Tuple<int, int>(next.X - curr.X, next.Y - curr.Y);
            double la = Math.Sqrt(a.Item1 * a.Item1 + a.Item2 * a.Item2);
            double lb = Math.Sqrt(b.Item1 * b.Item1 + b.Item2 * b.Item2);

            return (a.Item1 * b.Item1 + a.Item2 * b.Item2) / la / lb;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Point> visited = new List<Point>();
            List<Point> remaining = points;
            remaining = remaining.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            Point curr = remaining[0];
            visited.Add(curr);
            Point prev = curr;
            --prev.Y;

            do
            {
                Point next = remaining[0];
                int pos = 0;
                for (int i = 1; i < remaining.Count(); ++i)
                    if (cos_points(curr, prev, next) > cos_points(curr, prev, remaining[i]))
                    {
                        next = remaining[i];
                        pos = i;
                    }

                visited.Add(next);
                remaining.RemoveAt(pos);
                prev = curr;
                curr = next;
            } while (curr != visited[0]);

            Pen pen = new Pen(Color.PowderBlue, 3);
            for (int i = 0; i < visited.Count() - 1; ++i)
            {
                g.DrawLine(pen, visited[i], visited[i + 1]);
            }
            pen.Dispose();
            pictureBox1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            points.Clear();
            g.Clear(Color.White);
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            points.Add(e.Location);
            SolidBrush b = new SolidBrush(Color.BlueViolet);
            g.FillEllipse(b, e.X - 5, e.Y - 5, 11, 11);
            b.Dispose();
            pictureBox1.Invalidate();
        }
    }
}
