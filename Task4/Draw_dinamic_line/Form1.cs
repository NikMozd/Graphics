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
        Point point1, point2, pointStart;
        Point[] pnts, pnts2;
        bool paint = false;
        Graphics graph;
        List<Point> points1;
        bool fin = false;

        public Form1()
        {
            InitializeComponent();
            graph = this.CreateGraphics();
            points1 = new List<Point>();
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button8.Visible = false;
            button9.Visible = false;
        }

        private bool isLayed(int x1, int y1, int x2, int y2, double x, double y) // Проверка принадлежности точки отрезку
        {
            return (((Math.Abs((x - x1) / (y - y1) - (x - x2) / (y - y2)) < 0.0001)) && (x < Math.Max(x1, x2))) && (x > Math.Min(y1, y2) && (y < Math.Max(y1, y2)) && (y > Math.Min(y1, y2)));
        }

        private void Obj_Intersect(Point[] pnts, Point[] pnts2) //Точка пересечения отрезков
        {
            double xo = pnts[0].X, yo = pnts[0].Y;
            double p = pnts[1].X - pnts[0].X, q = pnts[1].Y - pnts[0].Y;

            double x1 = pnts2[0].X, y1 = pnts2[0].Y;
            double p1 = pnts2[1].X - pnts2[0].X, q1 = pnts2[1].Y - pnts2[0].Y;

            double x = (xo * q * p1 - x1 * q1 * p - yo * p * p1 + y1 * p * p1) /
                (q * p1 - q1 * p);
            double y = (yo * p * q1 - y1 * p1 * q - xo * q * q1 + x1 * q * q1) /
                (p * q1 - p1 * q);

            bool fl = false;
            
        
            if (isLayed(pnts[0].X, pnts[0].Y, pnts[1].X, pnts[1].Y, x, y))
            {
                if (isLayed(pnts2[0].X, pnts2[0].Y, pnts2[1].X, pnts2[1].Y, x, y))
                    fl = true;
            }
            
            if (fl)
                MessageBox.Show("Точка пересечения: " + (int)x + " " + (int)y, "Точка пересечения", MessageBoxButtons.OK);
            
            else
            {
                MessageBox.Show("Отрезки не пересекаются", "Точка пересечения", MessageBoxButtons.OK);
            }


        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Готово")
            {
                graph.Clear(Form1.DefaultBackColor);
                pnts = new Point[points1.Count - 1];
                if (points.Count > 2)
                {
                    for (int i = 0; i < points1.Count - 1; i++)
                    {
                        pnts[i] = points1[i];
                    }
                    graph.DrawPolygon(Pens.Blue, pnts);
                    
                    button6.Visible = true;
                }
                else if (points.Count <= 2)
                {
                    pnts = new Point[2];
                    pnts[0] = pointStart;
                    pnts[1] = point1;
                    graph.DrawLine(Pens.Blue, point1, pointStart);
                    
                    button3.Visible = true;
                    button4.Visible = true;
                    button5.Visible = true;
                }

                button2.Visible = true;
                button8.Visible = true;
                button9.Visible = true;

                fin = true;

                //if (points.Count ==)

                button1.Text = "Очистить";
                
            }

            

            else
            {
                fin = false;
                graph.Clear(Form1.DefaultBackColor);
                points.Clear();
                points1.Clear();
                point1.X = 0;
                point1.Y = 0;
                point2.X = 0;
                point2.Y = 0;
                button1.Text = "Готово";
            }

        }

        

        public void Mult_Matrix(List<double> v1, int c1, List<double> v2, int c2, List<double> res, int c3)
        {
            c3 = Math.Max(c1, c2);
            for (int k = 0; k < v1.Count; k += c1)
                for (int i = 0; i < c2; i ++)
                {
                    double r = 0;
                    for (int j = 0; j < c1; j++)
                    {
                        r += v1[k + j] * v2[i + j * c2];
                    
                    }
                    res.Add(r);
                }
        }

        public void Obj_Rotate(List<Point> obj, Point rotate_p, double alpha, List<Point> res)//Поворот объекта, заданного массивом точек obj,
                                                                                              //вокруг заданной точки rotate_p на угол alpha
        {
            double rad_alpha = alpha * Math.PI / 180;

            List<double> rotate_matrix = new List<double>();
            rotate_matrix.Add(Math.Cos(rad_alpha));
            rotate_matrix.Add(Math.Sin(rad_alpha));
            rotate_matrix.Add(0);
            rotate_matrix.Add(Math.Sin(rad_alpha) * (-1));
            rotate_matrix.Add(Math.Cos(rad_alpha));
            rotate_matrix.Add(0);
            rotate_matrix.Add((-1) * rotate_p.X * Math.Cos(rad_alpha) + rotate_p.Y * Math.Sin(rad_alpha) + rotate_p.X);
            rotate_matrix.Add((-1) * rotate_p.X * Math.Sin(rad_alpha) - rotate_p.Y * Math.Cos(rad_alpha) + rotate_p.Y);
            rotate_matrix.Add(1);

            foreach (Point x in obj)
            {
                List<double> ar = new List<double>();
                ar.Add(x.X);
                ar.Add(x.Y);
                ar.Add(1);

                List<double> r = new List<double>();
                Mult_Matrix(ar, 3, rotate_matrix, 3, r, 3);
                res.Add(new Point((int)r[0], (int)r[1]));
            }
        }

        private void Obj_Offset(List<Point> obj, int x, int y, List<Point> res)
        {
            List<double> offset_matrix = new List<double>();
            offset_matrix.Add(1);
            offset_matrix.Add(0);
            offset_matrix.Add(0);
            offset_matrix.Add(0);
            offset_matrix.Add(1);
            offset_matrix.Add(0);
            offset_matrix.Add(x);
            offset_matrix.Add(y);
            offset_matrix.Add(1);

            foreach (Point p in obj)
            {
                List<double> ar = new List<double>();
                ar.Add(p.X);
                ar.Add(p.Y);
                ar.Add(1);

                List<double> r = new List<double>();
                Mult_Matrix(ar, 3, offset_matrix, 3, r, 3);
                res.Add(new Point((int)r[0], (int)r[1]));
            }
        }

        private void Obj_Zoom(List<Point> obj, double x, double y, Point p, List<Point> res)
        {
            List<double> offset_matrix = new List<double>();
            offset_matrix.Add(x);
            offset_matrix.Add(0);
            offset_matrix.Add(0);
            offset_matrix.Add(0);
            offset_matrix.Add(y);
            offset_matrix.Add(0);
            offset_matrix.Add((1 - x) * p.X);
            offset_matrix.Add((1 - y) * p.Y);
            offset_matrix.Add(1);

            foreach (Point np in obj)
            {
                List<double> ar = new List<double>();
                ar.Add(np.X);
                ar.Add(np.Y);
                ar.Add(1);

                List<double> r = new List<double>();
                Mult_Matrix(ar, 3, offset_matrix, 3, r, 3);
                res.Add(new Point((int)r[0], (int)r[1]));
            }
        }

        private int point_rel_ribs(Point[] pnts, Point p) //Положение точки относительно ребра
        {
            int s =  (pnts[1].X - pnts[0].X) * (p.Y - pnts[0].Y) - (pnts[1].Y - pnts[0].Y) * (p.X - pnts[0].X);
            if (s > 0)
                return 1;
            else if (s < 0)
                return -1;
            else
                return 0;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
           if (!fin)
            {
                if (point1.X == 0)
                {
                    point1 = new Point(e.X, e.Y);
                    pointStart = point1;
                    points1.Add(new Point(e.X, e.Y));
                }
                else if (button4.Text == "Готово 2 ребро")
                {
                    point1 = new Point(e.X, e.Y);
                    pointStart = point1;
                    points1.Add(new Point(e.X, e.Y));
                }
                paint = true;
            }
            else
            {
                textBox1.Text = e.X.ToString() + " " + e.Y.ToString();
            }
          
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                point2 = e.Location;
                
            }
            if (!fin)
                this.Refresh();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            points.Add(new twoPoints(point1, point2));
            points1.Add(new Point(e.X, e.Y));
            point1 = point2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text;
            string[] ss = s.Split(' ');
            int x = Convert.ToInt16(ss[0]);
            int y = Convert.ToInt16(ss[1]);

            s = textBox4.Text;
            int alpha = Convert.ToInt16(s);

            List<Point> res = new List<Point>();
            Point p = new Point((int)x, (int)y);
            List<Point> stp = new List<Point>();
            for (int i = 0; i < pnts.Length; i++)
            {
                stp.Add(pnts[i]);
            }

            Obj_Rotate(stp, p, alpha, res);
            graph.Clear(Form1.DefaultBackColor);

            if (pnts.Length > 2)
                graph.DrawPolygon(Pens.Blue, res.ToArray());
            else
            {
                Point p1 = new Point((int)res[0].X, (int)res[0].Y);
                Point p2 = new Point((int)res[1].X, (int)res[1].Y);
                graph.DrawLine(Pens.Blue, p1, p2);
            }
        }

        private void button3_Click(object sender, EventArgs e)//поворот на 90 градусов относительно центра отрезка
        {
            double a = (pnts[0].X + pnts[1].X) / 2;
            double b = (pnts[0].Y + pnts[1].Y) / 2;
            /*double alpha = 1.5708;
            double x1 = pnts[0].X * Math.Cos(alpha) - Math.Sin(alpha) * pnts[0].Y - a * Math.Cos(alpha) + b * Math.Sin(alpha) + a;
            double x2 = pnts[1].X * Math.Cos(alpha) - Math.Sin(alpha) * pnts[1].Y - a * Math.Cos(alpha) + b * Math.Sin(alpha) + a;

            double y1 = pnts[0].X * Math.Sin(alpha) + Math.Cos(alpha) * pnts[0].Y - a * Math.Sin(alpha) - b * Math.Cos(alpha) + b;
            double y2 = pnts[1].X * Math.Sin(alpha) + Math.Cos(alpha) * pnts[1].Y - a * Math.Sin(alpha) - b * Math.Cos(alpha) + b;

            Point p1 = new Point((int)x1, (int)y1);
            Point p2 = new Point((int)x2, (int)y2);
            graph.Clear(Form1.DefaultBackColor);
            graph.DrawLine(Pens.Blue, p1, p2);*/

             List<Point> res = new List<Point>();
             Point p = new Point((int)a, (int)b);
             List<Point> stp = new List<Point>();
             stp.Add(pnts[0]);
             stp.Add(pnts[1]);
             
             Obj_Rotate(stp, p, 90, res);
             graph.Clear(Form1.DefaultBackColor);
             Point p1 = new Point((int)res[0].X, (int)res[0].Y);
             Point p2 = new Point((int)res[1].X, (int)res[1].Y);
             graph.DrawLine(Pens.Blue, p1, p2);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string s = textBox2.Text;
            string[] ss = s.Split(' ');
            int x = Convert.ToInt16(ss[0]);
            int y = Convert.ToInt16(ss[1]);
            

            List<Point> res = new List<Point>();
            List<Point> stp = new List<Point>();
            for (int i = 0; i < pnts.Length; i++)
            {
                stp.Add(pnts[i]);
            }

            Obj_Offset(stp, x, y, res);
            graph.Clear(Form1.DefaultBackColor);

            graph.DrawPolygon(Pens.Blue, res.ToArray());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string s = textBox3.Text;
            string[] ss = s.Split(' ');
            double x = Convert.ToInt16(ss[0]) / 100d;//Коэффициенты растяжения задаются пользователем в процентах
            double y = Convert.ToInt16(ss[1]) / 100d;

            s = textBox1.Text;
            ss = s.Split(' ');
            Point p = new Point(Convert.ToInt16(ss[0]), Convert.ToInt16(ss[1]));//Точка, относительно которой масштабируется

            List<Point> res = new List<Point>();
            List<Point> stp = new List<Point>();
            for (int i = 0; i < pnts.Length; i++)
            {
                stp.Add(pnts[i]);
            }

            Obj_Zoom(stp, x, y, p, res);
            graph.Clear(Form1.DefaultBackColor);

            if (pnts.Length > 2)
                graph.DrawPolygon(Pens.Blue, res.ToArray());
            else
            {
                Point p1 = new Point((int)res[0].X, (int)res[0].Y);
                Point p2 = new Point((int)res[1].X, (int)res[1].Y);
                graph.DrawLine(Pens.Blue, p1, p2);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text;
            string[] ss = s.Split(' ');
            Point p = new Point(Convert.ToInt16(ss[0]), Convert.ToInt16(ss[1]));
            int res = point_rel_ribs(pnts, p);
            if (res > 0)
                MessageBox.Show("Точка справа от ребра", "Положение точки", MessageBoxButtons.OK);
            else if (res < 0)
                MessageBox.Show("Точка слева от ребра", "Положение точки", MessageBoxButtons.OK);
            else
                MessageBox.Show("Точка на ребре, прямо по ребру или сзади ребра", "Положение точки", MessageBoxButtons.OK);
        }

        

        private void button4_Click(object sender, EventArgs e)
        {
            
            if (button4.Text == "Готово 2 ребро")
            {

                pnts2 = new Point[points1.Count - 1];
                if (points1.Count <= 2)
                {
                    pnts2 = new Point[2];
                    pnts2[0] = pointStart;
                    pnts2[1] = point1;
                    graph.DrawLine(Pens.Blue, point1, pointStart);
                }
                button4.Text = "Пересечение ребер";
                fin = true;

                Obj_Intersect(pnts, pnts2);
            }
            else
            {
                pnts2 = new Point[2];
                button4.Text = "Готово 2 ребро";
                fin = false;
                points1.Clear();
            }
        }

        private int classify(Point p, Point v, Point w) //положение точки p относительно отрезка vw
        {
            //коэффициенты уравнения прямой
            int a = v.Y - w.Y;
            int b = w.X - v.X;
            int c = v.X * w.Y - w.X * v.Y;

            //подставим точку в уравнение прямой
            int f = a * p.X + b * p.Y + c;
            if (f > 0)
                return -1; //точка лежит справа от отрезка
            if (f < 0)
                return 1; //слева от отрезка

            int minX = Math.Min(v.X, w.X);
            int maxX = Math.Max(v.X, w.X);
            int minY = Math.Min(v.Y, w.Y);
            int maxY = Math.Max(v.Y, w.Y);

            if (minX <= p.X && p.X <= maxX && minY <= p.Y && p.Y <= maxY)
                return 0; //точка лежит на отрезке
            return 2; //точка лежит на прямой, но не на отрезке
        }

        private int edgeType(Point a, Point v, Point w) //тип ребра vw для точки a
        {
            switch (classify(a, v, w))
            {
                case 1:
                    return ((v.Y < a.Y) && (a.Y <= w.Y)) ? 1 : -1;
                case -1:
                    return ((w.Y < a.Y) && (a.Y <= v.Y)) ? 1 : -1;
                case 0:
                    return 0;
                default:
                    return -1;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public int pointInPolygon(Point a) //положение точки в многоугольнике
        {
            bool parity = true;
            for (int i = 0; i < pnts.Length; i++)
            {
                Point v = pnts[i];
                Point w = pnts[(i + 1) % pnts.Length];

                switch (edgeType(a, v, w))
                {
                    case 0:
                        return 0;
                    case 1:
                        parity = !parity;
                        break;
                }
            }

            return parity ? -1 : 1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text;
            string[] ss = s.Split(' ');
            Point p = new Point(Convert.ToInt16(ss[0]), Convert.ToInt16(ss[1]));

            int res = pointInPolygon(p);
            string res_string;
            if (res == 1)
                res_string = "Точка внутри многоугольника";
            else if (res == -1)
                res_string = "Точка снаружи многоугольника";
            else
                res_string = "Точка на ребре многоугольника";
            MessageBox.Show(res_string, "Принадлежность точки многоугольнику", MessageBoxButtons.OK);

           
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
          
            foreach (twoPoints tp in points)
            {
                e.Graphics.DrawLine(Pens.Red, tp.p1, tp.p2);
            }
                e.Graphics.DrawLine(Pens.Red, point1, point2);
           
        }
        
    }
}