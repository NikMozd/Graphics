using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L_systems
{
    public partial class Form1 : Form
    {
        private Graphics g;

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

        private void print_state(string state, double angle)
        {
            List<Point> points = new List<Point>();

            int length = 20;
            double current_angle = 0;
            Point current_point = new Point(0, 0);
            points.Add(current_point);

            //Pen pen = new Pen(Color.RoyalBlue);
            foreach (var c in state)
            {
                if (c == 'F')
                {
                    int x_new = current_point.X + (int)(length * Math.Cos(current_angle / 180 * Math.PI));
                    int y_new = current_point.Y + (int)(length * Math.Sin(current_angle / 180 * Math.PI));
                    Point next_point = new Point(x_new, y_new);
                    //g.DrawLine(pen, current_point, next_point);
                    current_point = next_point;
                    points.Add(current_point);
                }
                else if (c == '-')
                    current_angle -= angle;
                else if (c == '+')
                    current_angle += angle;
            }

            int x_min = int.MaxValue;
            int x_max = int.MinValue;
            int y_min = int.MaxValue;
            int y_max = int.MinValue;
            for (int i = 0; i < points.Count(); ++i)
            {
                if (points[i].X < x_min)
                    x_min = points[i].X;
                if (points[i].X > x_max)
                    x_max = points[i].X;
                if (points[i].Y < y_min)
                    y_min = points[i].Y;
                if (points[i].Y > y_max)
                    y_max = points[i].Y;
            }

            // Найти два соотношения сторон (то, где рисуем, к области точек) и берём МЕНЬШЕЕ число
            Point point_middle = new Point((x_min + x_max) / 2, (y_min + y_max) / 2);
            Point window_middle = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);

            double coef_x = (double)pictureBox1.Width / (x_max - x_min + 1);
            double coef_y = (double)pictureBox1.Height / (y_max - y_min);
            double coef = Math.Min(coef_x, coef_y);

            List<Point> new_points = new List<Point>();
            for (int i = 0; i < points.Count(); ++i)
            {
                int dist_x = points[i].X - point_middle.X;
                int dist_y = points[i].Y - point_middle.Y;
                dist_x = (int)(dist_x * coef);
                dist_y = (int)(dist_y * coef);

                Point np = window_middle;
                np.X += dist_x;
                np.Y += dist_y;
                new_points.Add(np);
            }

            Pen pen = new Pen(Color.RoyalBlue);
            for (int i = 0; i < new_points.Count() - 1; ++i)
                g.DrawLine(pen, new_points[i], new_points[i + 1]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            pictureBox1.Invalidate();

            System.IO.StreamReader sr = new System.IO.StreamReader(label2.Text);
            Dictionary<char, string> rules = new Dictionary<char, string>();

            string current_state = sr.ReadLine();
            double angle = 0;
            
            while (!sr.EndOfStream)
            {
                string str;
                str = sr.ReadLine();
                if (!sr.EndOfStream)
                    rules.Add(str[0], str.Substring(2));
                else
                    angle = int.Parse(str);
            }
            sr.Close();

            for (int i = 0; i < numericUpDown1.Value; ++i)
            {
                string next_state = "";
                foreach (var c in current_state)
                {
                    if (rules.ContainsKey(c))
                        next_state += rules[c];
                    else
                        next_state += c;
                }
                current_state = next_state;
            }

            print_state(current_state, angle);

            pictureBox1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            label2.Text = openFileDialog1.FileName;
        }
    }
}
