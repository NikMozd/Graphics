using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidpointDisplacement
{
    public partial class Form1 : Form
    {
        Point p1, p2;
        bool fin, paint;
        Graphics graph;
        List<Point> points;
        public Form1()
        {
            InitializeComponent();
            paint = false;
            fin = false;
            graph = this.CreateGraphics();
            points = new List<Point>();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            p1 = new Point(e.X, e.Y);
            paint = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, p1, p2);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            p2 = new Point(e.X, e.Y);
            fin = true;
            paint = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Готово")
            {
                graph.Clear(Color.DeepSkyBlue);
                
                points.Add(p1);
                points.Add(p2);
                double r = Convert.ToDouble(textBox1.Text);
                int num_it = Convert.ToInt16(textBox2.Text);
                for (int i = 0; i < num_it; i++)
                 addHights(r);
                

                 for (int i = 0; i < points.Count - 1; i++)
                 {
                     graph.DrawLine(Pens.Black, points[i], points[i + 1]);
                 }
                button1.Text = "Очистить";
            }
            else
            {
                button1.Text = "Готово";
                fin = false;
                graph.Clear(Form1.DefaultBackColor);
                points.Clear();
            }
        }

        private void addHights(double R)
        {
            List<Point> l1 = new List<Point>();
            Point p = points.First();
            l1.Add(p);
            Random rnd = new Random();

            for (int i = 1; i < points.Count; ++i)
            {
                int x = (points[i].X + p.X) / 2;
                double length = Math.Sqrt(Math.Pow(points[i].X - p.X, 2) + Math.Pow(points[i].Y - p.Y, 2)); // длина отрезка

                double min = -R * length;
                double max = R * length;
                int h = Convert.ToInt32(Math.Round((p.Y + points[i].Y) / 2 + rnd.NextDouble() * (max - min) + min));

                l1.Add(new Point(x, h));
                l1.Add(points[i]);
                p = points[i];
            }

            points.Clear();

            foreach (var p1 in l1)
                points.Add(p1);
            l1.Clear();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                p2 = e.Location;
            }
            if (!fin)
                this.Refresh();
        }

        class PointComparer : IComparer<Point>
        {
            public int Compare(Point p1, Point p2)
            {
                if (p1.X > p2.X)
                    return 1;
                else if (p1.X < p2.X)
                    return -1;
                else
                    return 0;
            }
        }

        

       /* double rand(double min, double max)
        {
            Random rnd = new Random();
            return min + rnd.NextDouble() * (max - min);
        }
        
         void midpoint(List<Point> points, int left, int right, double r)
         {
             if (right - left < 2)
                 return;
             int hl = points[left].Y; //высота левой точки
             int hr = points[right].Y; //высота правой
             int x = (points[left].X + points[right].X) / 2;
            double length = Math.Sqrt(Math.Pow(points[left].X - points[right].X, 2) + Math.Pow(points[left].Y - points[right].Y, 2)); // длина отрезка

            double min = -r * length;
            double max = r * length;
            double h = (hl + hr) / 2 + rand(-r * length, r * length); //считаем высоту
             int index = (int)(left + (right - left) / 2); //ищем середину
             points[index] = new Point(x, (int)h);
             //выполняем алгоритм для получившихся половин
             midpoint(points, left, index, r);
             midpoint(points, index, right, r);
         }*/

        

    }
}
