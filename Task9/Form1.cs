using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {
        Graphics g;
        Bitmap[] bmp;
        Polyhedron ph = null;
        public Form1()
        {
            InitializeComponent();
            //pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = pictureBox1.CreateGraphics();
            //g = Graphics.FromImage(pictureBox1.Image);
            bmp = new Bitmap[4];
            bmp[0] = new Bitmap("Iso.bmp");
            bmp[1] = new Bitmap("Oxy.bmp");
            bmp[2] = new Bitmap("Oxz.bmp");
            bmp[3] = new Bitmap("Oyz.bmp");
            pictureBox2.Image = bmp[0];
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void radioButtonRot_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButtonRot.Checked)
            {
                textBoxAngle.BackColor = Color.LimeGreen;
                textBoxPoint1.BackColor = Color.LimeGreen;
                textBoxPoint2.BackColor = Color.LimeGreen;
            }
            else
            {
                textBoxAngle.BackColor = Color.White;
                textBoxPoint1.BackColor = Color.White;
                textBoxPoint2.BackColor = Color.White;
            }
        }

        private void radioButtonReflect_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonReflect.Checked)
            {
                comboBoxPlane.BackColor = Color.LimeGreen;
            }
            else
            {
                comboBoxPlane.BackColor = Color.White;
            }
        }
        
        private void radioButtonTrans_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTrans.Checked)
            {
                textBoxTrans.BackColor = Color.LimeGreen;
            }
            else
            {
                textBoxTrans.BackColor = Color.White;
            }
        }

        private void radioButtonScale_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonScale.Checked)
            {
                textBoxScale.BackColor = Color.LimeGreen;
            }
            else
            {
                textBoxScale.BackColor = Color.White;
            }
        }

        private void radioButtonAxisRot_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAxisRot.Checked)
            {
                comboBoxAxis.BackColor = Color.LimeGreen;
                textBoxAngle.BackColor = Color.LimeGreen;
            }
            else
            {
                comboBoxAxis.BackColor = Color.White;
                textBoxAngle.BackColor = Color.White;
            }
        }

        private void buttonTetra_Click(object sender, EventArgs e)
        {
            ph = Polyhedron.CreateTetrahedron(new Point3d(0, 0, 0), new Point3d(2, 0.2, 0), new Point3d(0, 1, 0));
            Plot();
        }

        private void buttonHexa_Click(object sender, EventArgs e)
        {
            ph = Polyhedron.CreateHexahedron(new Point3d(0, 0, 0), new Point3d(1, 0, 0), new Point3d(0, 1, 0));
            Plot();
        }

        private void buttonOkta_Click(object sender, EventArgs e)
        {
            ph = Polyhedron.CreateOktahedron(new Point3d(0, 0, 0), new Point3d(2, 0, 0), new Point3d(0, 1, 0));
            Plot();
        }

        private void buttonPlot_Click(object sender, EventArgs e)
        {
            if (radioButtonTrans.Checked)
            {
                double[] parseRes = Parse(textBoxTrans.Text);
                CheckLen(parseRes, 3);
                Affine.Translate(ph, new Point3d(parseRes[0], parseRes[1], parseRes[2]));
            }
            else if (radioButtonRot.Checked)
            {
                double[] parseResPoint1 = Parse(textBoxPoint1.Text);
                CheckLen(parseResPoint1, 3);
                double[] parseResPoint2 = Parse(textBoxPoint2.Text);
                CheckLen(parseResPoint1, 3);
                double[] parseRes = Parse(textBoxAngle.Text);
                CheckLen(parseRes, 1);
                double angleInRad = parseRes[0] / 180.0 * Math.PI;
                Affine.RotateOverStreight(ph, new Point3d(parseResPoint1[0], parseResPoint1[1], parseResPoint1[2]), new Point3d(parseResPoint2[0], parseResPoint2[1], parseResPoint2[2]), angleInRad);
            }
            else if (radioButtonScale.Checked)
            {
                double[] parseRes = Parse(textBoxScale.Text);
                CheckLen(parseRes, 1);
                Affine.ScaleOverCenter(ph, parseRes[0]);
            }
            else if (radioButtonReflect.Checked)
            {
                switch (comboBoxPlane.SelectedIndex)
                {
                    case 0:
                        Affine.Reflect(ph, 'z');
                        break;
                    case 1:
                        Affine.Reflect(ph, 'y');
                        break;
                    case 2:
                        Affine.Reflect(ph, 'x');
                        break;
                    default:
                        throw new Exception("Bad plane.");
                }

            }
            else if (radioButtonAxisRot.Checked)
            {
                double[] parseRes = Parse(textBoxAngle.Text);
                CheckLen(parseRes, 1);
                double angleInRad = parseRes[0] / 180.0 * Math.PI;
                switch (comboBoxAxis.SelectedIndex)
                {
                    case 0:
                        Affine.RotateOverStreight(ph, ph.Center, new Point3d(ph.Center.X + 1, ph.Center.Y, ph.Center.Z), angleInRad);
                        break;
                    case 1:
                        Affine.RotateOverStreight(ph, ph.Center, new Point3d(ph.Center.X, ph.Center.Y + 1, ph.Center.Z), angleInRad);
                        break;
                    case 2:
                        Affine.RotateOverStreight(ph, ph.Center, new Point3d(ph.Center.X, ph.Center.Y, ph.Center.Z + 1), angleInRad);
                        break;
                    default:
                        throw new Exception("Bad axis.");
                }
            }
            else
            {
                throw new Exception("Plot error.");
            }
            Plot();
        }

        private void Plot()
        {
            if (ph == null)
                return;
            if (checkBox1.Checked)
                g.Clear(Color.Black);
            else
                g.Clear(Color.White);

            Point3d.DrawPoint0(g, Color.LimeGreen);

            Polyhedron proj = ph.Clone() as Polyhedron;

            if (radioButtonCenterProj.Checked)
            {
                Affine.CentralProjection(proj, 10);
                pictureBox2.Image = bmp[3];
            }
            else if (radioButtonIsoProj.Checked)
            {
                Affine.IsometricProjection(proj);
                pictureBox2.Image = bmp[0];
            }
            else if (radioButtonOrtoProj.Checked)
            {
                switch (comboBoxProjPlane.SelectedIndex)
                {
                    case 0:
                        Affine.OrtographicProjection(proj, 'z');
                        pictureBox2.Image = bmp[1];
                        break;
                    case 1:
                        Affine.OrtographicProjection(proj, 'y');
                        pictureBox2.Image = bmp[2];
                        break;
                    case 2:
                        Affine.OrtographicProjection(proj, 'x');
                        pictureBox2.Image = bmp[3];
                        break;
                    default:
                        throw new Exception("Bad projection plane.");
                }
            }
            else
            {
                throw new Exception("Projection error.");
            }

            if (checkBox1.Checked)
            {
                Polyhedron pproj = ph.Clone() as Polyhedron;
                GouraudShading(pproj);
                Point p = find_point_screen(new Point3d
                    (double.Parse(textBox17.Text), double.Parse(textBox18.Text), double.Parse(textBox19.Text)));
                g.FillRectangle(new SolidBrush(Color.White), p.X - 2, p.Y - 2, 5, 5);
                return;
            }

            proj.Draw(g, Color.Black);
            //pictureBox1.Invalidate();
        }

        private double[] Parse(string s)
        {
            string[] s1 = s.Split(',');
            double[] res = new double[s1.Length];
            for (int i = 0; i < res.Length; ++i)
            {
                res[i] = double.Parse(s1[i].Trim(), CultureInfo.InvariantCulture);
            }
            return res;
        }

        private void CheckLen(double[] arr, int len)
        {
            if (arr.Length != len)
                throw new Exception("Wrong length of input data.");
        }

        private void radioButtonCenterProj_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCenterProj.Checked)
                Plot();
        }

        private void radioButtonIsoProj_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonIsoProj.Checked)
                Plot();
        }

        private void radioButtonOrtoProj_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonOrtoProj.Checked)
            {
                comboBoxProjPlane.Enabled = true;
                Plot();
            }
            else
            {
                comboBoxProjPlane.Enabled = false;
            }
        }

        private void comboBoxProjPlane_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Plot();
        }

        private void textBoxAngle_TextChanged(object sender, EventArgs e)
        {

        }

        private void Icosahedron_Click(object sender, EventArgs e)
        {
            ph = Polyhedron.CreateIcosahedron();
            Plot();
        }

        private void Dodecahedron_Click(object sender, EventArgs e)
        {
            ph = Polyhedron.CreateDodecahedron();
            Plot();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Загрузить из файла")
            {
                OpenFileDialog fileDialog = new OpenFileDialog();

                DialogResult result = fileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    textBox1.Text = fileDialog.FileName;
                    button1.Text = "Построить";
                }
            }
            else
            {
                string file_name = textBox1.Text;
                // сохраняем текст в файл
                string file_content = "";
                using (FileStream fstream = File.OpenRead(file_name))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    file_content = System.Text.Encoding.Default.GetString(array);
                }
                Polyhedron result = ParseFileToPolyhedron(file_content);
                ph = result;
                Plot();
                button1.Text = "Загрузить из файла";
            }
        }

        private Polyhedron ParseFileToPolyhedron(string file_content)
        {
            string[] lines = file_content.Split('\n');
            Face[] faces = new Face[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                faces[i] = ParseLineToFace(lines[i]);
            }
            Polyhedron result = new Polyhedron();
            result.faces = faces;

           
            double c_x = 0;
            double c_y = 0;
            double c_z = 0;
            int cnt = 0;
            foreach (Face face in faces)
            {
                foreach (Edge edge in face.Edges)
                {
                    c_x += edge.First.X;
                    c_y += edge.First.Y;
                    c_z += edge.First.Z;

                    c_x += edge.Second.X;
                    c_y += edge.Second.Y;
                    c_z += edge.Second.Z;

                    cnt += 2;
                }
            }
            c_x /= cnt;
            c_y /= cnt;
            c_z /= cnt;
            Point3d center = new Point3d(c_x, c_y, c_z);
            result.center = center;

            return result;
        }

        private Face ParseLineToFace(string line)
        {
            string[] string_points = line.Split(')');
            Point3d[] points = new Point3d[string_points.Length - 1];
            for (int i = 0; i < string_points.Length - 1; i++)
            {
                points[i] = ParseStringToPoint(string_points[i]);
            }

            Face result = new Face();
            result.edges = new Edge[points.Length];
            for (int i = 1; i < points.Length; i++)
            {
                result.edges[i - 1] = new Edge(points[i - 1], points[i]);
            }
            result.edges[points.Length - 1] = new Edge(points[points.Length - 1], points[0]);
            return result;
        }

        private Point3d ParseStringToPoint(string v)
        {
            var charsToRemove = new string[] { "(", ")"};
            foreach (var c in charsToRemove)
            {
                v = v.Replace(c, string.Empty);
            }
            string[] coordinates = v.Split(' ');
            List<string> lcoordinates = new List<string>(coordinates);
            for (int i = 0; i < lcoordinates.Count; i++)
            {
                string s = lcoordinates[i];
                string ns = s.TrimEnd(',');
                lcoordinates[i] = ns;
                if (ns == "")
                {
                    lcoordinates.RemoveAt(i);
                    i--;
                }
            }
           
            double x = Convert.ToDouble(lcoordinates[0]);
            double y = Convert.ToDouble(lcoordinates[1]);
            double z = Convert.ToDouble(lcoordinates[2]);
            return new Point3d(x, y, z);
        }

        private void button2_Click(object sender, EventArgs e)
        {
                OpenFileDialog fileDialog = new OpenFileDialog();

                DialogResult result = fileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    textBox2.Text = fileDialog.FileName;
                }
                using (FileStream fstream = new FileStream(@textBox2.Text, FileMode.Create))
                {
                    string result_str = ParsePolyhedronToFile();
                    byte[] array = System.Text.Encoding.Default.GetBytes(result_str);
                    fstream.Write(array, 0, array.Length);
                }
        }

        private string ParsePolyhedronToFile()
        {
            string result = "";
            for (int i = 0; i < ph.faces.Length - 1; i++)
            {
                Point3d[] points = new Point3d[ph.faces[i].Edges.Length];
                for (int j = 0; j < ph.faces[i].Edges.Length; j++)
                {
                    points[j] = ph.faces[i].Edges[j].First;
                }
                string face = ParseFaceToLine(points);
                result += face;
                result += Environment.NewLine;
            }
            int i1 = ph.faces.Length - 1;
            Point3d[] points1 = new Point3d[ph.faces[i1].Edges.Length];
            for (int j = 0; j < ph.faces[i1].Edges.Length; j++)
            {
                points1[j] = ph.faces[i1].Edges[j].First;
            }
            string face1 = ParseFaceToLine(points1);
            result += face1;
            return result;
        }

        private Point3d[] RemoveDuplicates(Point3d[] myList)
        {
            List<Point3d> newList = new List<Point3d>();

            foreach (Point3d p in myList)
                if (!newList.Contains(p))
                    newList.Add(p);
            return newList.ToArray();
        }

        private string ParseFaceToLine(Point3d[] points_distinct)
        {
            string result = "";
            for (int i = 0; i < points_distinct.Length; i++)
            {
                result += "(" + points_distinct[i].X + ", " + points_distinct[i].Y + ", " + points_distinct[i].Z + ") ";
            }
            result.TrimEnd(' ');
            return result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            DialogResult result = fileDialog.ShowDialog();

            if (result == DialogResult.OK)
                label5.Text = fileDialog.FileName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string file_name = label5.Text;
            // сохраняем текст в файл
            string file_content = "";
            using (FileStream fstream = File.OpenRead(file_name))
            {
                // преобразуем строку в байты
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                // декодируем байты в строку
                file_content = System.Text.Encoding.Default.GetString(array);
            }

            string[] strs = file_content.Split('\n');
            string[] param = strs[0].Split(' ');
            string axis = param[0];
            int steps = int.Parse(param[1]);

            Point3d[] points = new Point3d[strs.Length - 1];
            for (int i = 0; i < strs.Length - 1; ++i)
            {
                string[] coords = strs[i + 1].Split(' ');
                double x = 0, y = 0, z = 0;
                switch (axis)
                {
                    case "OX":
                        x = double.Parse(coords[0]);
                        y = double.Parse(coords[1]);
                        break;
                    case "OY":
                        y = double.Parse(coords[0]);
                        z = double.Parse(coords[1]);
                        break;
                    case "OZ":
                        z = double.Parse(coords[0]);
                        x = double.Parse(coords[1]);
                        break;
                }
                points[i] = new Point3d(x, y, z);
            }

            Polyhedron p = new Polyhedron();
            p.faces = new Face[steps * (points.Count() - 1)];
            Point3d[] curr_points = points;
            double angle = 2 * Math.PI / steps;
            double curr_angle = angle;
            int pos = 0;
            int pcount = points.Count();

            double x_center = 0, y_center = 0, z_center = 0;
            for (int i = 0; i < pcount; ++i)
            {
                x_center += points[i].X;
                y_center += points[i].Y;
                z_center += points[i].Z;
            }

            for (int i = 0; i < steps - 1; ++i)
            {
                Point3d[] next_points = new Point3d[pcount];
                for (int j = 0; j < pcount; ++j)
                {
                    double x = 0, y = 0, z = 0;
                    switch (axis)
                    {
                        case "OX":
                            x = points[j].X;
                            y = points[j].Y * Math.Cos(curr_angle);
                            z = points[j].Y * Math.Sin(curr_angle);
                            break;
                        case "OY":
                            y = points[j].Y;
                            z = points[j].Z * Math.Cos(curr_angle);
                            x = points[j].Z * Math.Sin(curr_angle);
                            break;
                        case "OZ":
                            z = points[j].Z;
                            x = points[j].X * Math.Cos(curr_angle);
                            y = points[j].X * Math.Sin(curr_angle);
                            break;
                    }
                    next_points[j] = new Point3d(x, y, z);
                }

                for (int j = 1; j < pcount - 1; ++j)
                {
                    x_center += points[j].X;
                    y_center += points[j].Y;
                    z_center += points[j].Z;
                }

                p.faces[pos++] = Face.CreateTriangle(points[0], curr_points[1], next_points[1]);
                p.faces[pos++] = Face.CreateTriangle(points[pcount - 1], curr_points[pcount - 2], next_points[pcount - 2]);
                for (int j = 1; j < pcount - 2; ++j)
                    p.faces[pos++] = Face.CreateSquare(curr_points[j], next_points[j], next_points[j + 1], curr_points[j + 1]);

                curr_points = next_points;
                curr_angle += angle;
            }

            p.faces[pos++] = Face.CreateTriangle(points[0], curr_points[1], points[1]);
            p.faces[pos++] = Face.CreateTriangle(points[pcount - 1], curr_points[pcount - 2], points[pcount - 2]);
            for (int j = 1; j < pcount - 2; ++j)
                p.faces[pos++] = Face.CreateSquare(curr_points[j], points[j], points[j + 1], curr_points[j + 1]);

            x_center /= steps * (pcount - 2) + 2;
            y_center /= steps * (pcount - 2) + 2;
            z_center /= steps * (pcount - 2) + 2;

            double center = 0;
            for (int i = 0; i < pcount; ++i)
                center += points[i].X;
            center /= pcount;
            switch (axis)
            {
                case "OX":
                    p.center = new Point3d(center, 0, 0);
                    break;
                case "OY":
                    p.center = new Point3d(0, center, 0);
                    break;
                case "OZ":
                    p.center = new Point3d(0, 0, center);
                    break;
            }

            //p.center = new Point3d(x_center, y_center, z_center);

            ph = p;
            Plot();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            double vi = double.Parse(textBox3.Text);
            double vj = double.Parse(textBox5.Text);//double.Parse(textBox4.Text);
            double vk = double.Parse(textBox4.Text);//double.Parse(textBox5.Text);
            Polyhedron ph_copy = ph;

            for (int i = 0; i < 180; ++i)
            {
                double angleInRad = 2 / 180.0 * Math.PI;
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        Affine.RotateOverStreight(ph_copy, new Point3d(1, 0, 0), new Point3d(0, 0, 0), angleInRad);
                        //Affine.RotateOverStreight(ph_copy, ph_copy.Center, new Point3d(ph_copy.Center.X + 1, ph_copy.Center.Y, ph_copy.Center.Z), angleInRad);
                        break;
                    case 1:
                        Affine.RotateOverStreight(ph_copy, new Point3d(0, 1, 0), new Point3d(0, 0, 0), angleInRad);
                        //Affine.RotateOverStreight(ph_copy, ph_copy.Center, new Point3d(ph_copy.Center.X, ph_copy.Center.Y + 1, ph_copy.Center.Z), angleInRad);
                        break;
                    case 2:
                        Affine.RotateOverStreight(ph_copy, new Point3d(0, 0, 1), new Point3d(0, 0, 0), angleInRad);
                        //Affine.RotateOverStreight(ph_copy, ph_copy.Center, new Point3d(ph_copy.Center.X, ph_copy.Center.Y, ph_copy.Center.Z + 1), angleInRad);
                        break;
                    default:
                        throw new Exception("Bad axis.");
                }

                List<Face> faces = new List<Face>(ph_copy.faces);
                for (int j = faces.Count() - 1; j >= 0; --j)
                {
                    Point3d p1 = faces[j].edges[0].First;
                    Point3d p2 = faces[j].edges[1].First;
                    Point3d p3 = faces[j].edges[2].First;

                    double[,] matrix = new double[2, 3];
                    matrix[0, 0] = p2.X - p1.X;
                    matrix[0, 1] = p2.Y - p1.Y;
                    matrix[0, 2] = p2.Z - p1.Z;
                    matrix[1, 0] = p3.X - p1.X;
                    matrix[1, 1] = p3.Y - p1.Y;
                    matrix[1, 2] = p3.Z - p1.Z;

                    double ni = matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1];
                    double nj = matrix[0, 2] * matrix[1, 0] - matrix[0, 0] * matrix[1, 2];
                    double nk = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
                    double d = -(ni * p1.X + nj * p1.Y + nk * p1.Z);

                    Point3d pp = new Point3d(p1.X + ni, p1.Y + nj, p1.Z + nk);
                    double val1 = ni * pp.X + nj * pp.Y + nk * pp.Z + d;
                    double val2 = ni * ph_copy.center.X + nj * ph_copy.center.Y + nk * ph_copy.center.Z + d;
                    if (val1 * val2 > 0)
                    {
                        ni = -ni;
                        nj = -nj;
                        nk = -nk;
                    }

                    if (ni * vi + nj * vj + nk * vk < 0)
                        faces.RemoveAt(j);
                }

                ph = new Polyhedron();
                ph.faces = new Face[faces.Count()];
                for (int j = 0; j < faces.Count(); ++j)
                    ph.faces[j] = faces[j];
                ph.center = ph_copy.center;
                Plot();
                System.Threading.Thread.Sleep(40);
            }

            ph = ph_copy;
        }

        Cam CreateCam()
        {
            double pos_x = double.Parse(textBox6.Text);
            double pos_y = double.Parse(textBox7.Text);
            double pos_z = double.Parse(textBox8.Text);

            double view_x = double.Parse(textBox9.Text);
            double view_y = double.Parse(textBox10.Text);
            double view_z = double.Parse(textBox11.Text);

            double vert_x = double.Parse(textBox12.Text);
            double vert_y = double.Parse(textBox13.Text);
            double vert_z = double.Parse(textBox14.Text);

            // проверка на ортогональность
            if (Math.Abs(view_x * vert_x + view_y * vert_y + view_z * vert_z) > 1e-6)
                throw new ArgumentException("bad vectors");

            // нормировать
            double view_len = Math.Sqrt(view_x * view_x + view_y * view_y + view_z * view_z);
            view_x /= view_len;
            view_y /= view_len;
            view_z /= view_len;

            double vert_len = Math.Sqrt(vert_x * vert_x + vert_y * vert_y + vert_z * vert_z);
            vert_x /= vert_len;
            vert_y /= vert_len;
            vert_z /= vert_len;

            // найти третий вектор
            double hor_x = view_y * vert_z - view_z * vert_y;
            double hor_y = view_z * vert_x - view_x * vert_z;
            double hor_z = view_x * vert_y - view_y * vert_x;
            double hor_len = Math.Sqrt(hor_x * hor_x + hor_y * hor_y + hor_z * hor_z);
            hor_x /= hor_len;
            hor_y /= hor_len;
            hor_z /= hor_len;

            // проверить, что тройка правая
            double tr_prod = hor_x * vert_y * view_z + hor_y * vert_z * view_x + hor_z * vert_x * view_y
                - hor_z * vert_y * view_x - hor_y * vert_x * view_z - hor_x * vert_z * view_y;
            if (tr_prod < 0)
            {
                hor_x = -hor_x;
                hor_y = -hor_y;
                hor_z = -hor_z;
            }

            return new Cam(new Point3d(pos_x, pos_y, pos_z), new Point3d(view_x, view_y, view_z),
                    new Point3d(hor_x, hor_y, hor_z), new Point3d(vert_x, vert_y, vert_z));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Cam cam = CreateCam();
            
            for (int i = 0; i < 30; i++)
            {
                // видовая матрица
                g.Clear(Color.White);

                double angle = 10 * i * Math.PI / 180;
                if (angle >= 2 * Math.PI)
                    angle -= 2 * Math.PI;

                Polyhedron ph_copy = ph.Clone() as Polyhedron;
                //Affine.MakePerspectiveProjection(ph_copy, 45 * Math.PI / 180, 100, 5);
                Affine.MakeView(ph_copy, cam);
                Affine.RotateOverCenter(ph_copy, 'z', -angle);
                // вывести
                Affine.CentralProjection(ph_copy, 10);
                //Affine.IsometricProjection(ph_copy);
                ph_copy.Draw(g, Color.Black);
                //pictureBox1.Invalidate();
                System.Threading.Thread.Sleep(100);
            }
        }

        private void buttonZbuffer_Click(object sender, EventArgs e)
        {
            Polyhedron ph_copy = ph.Clone() as Polyhedron;
            Affine.RotateOverStreight(ph_copy, new Point3d(0, 0, 0), new Point3d(0, 0, 1), -Math.PI / 4);
            Affine.RotateOverStreight(ph_copy, new Point3d(0, 0, 0), new Point3d(0, 1, 0), Math.Atan(1.0 / Math.Sqrt(2)));
            //Affine.OrtographicProjection(ph_copy, 'x');
            //ph_copy.Draw(g, Color.Black);
            g.Clear(Color.White);

            Zbuffer zb = new Zbuffer(pictureBox1.Width, pictureBox1.Height);
            foreach (var x in ph_copy.Faces)
            {
                zb.ProcessFace(x);
            }
            //for (int i = 0; i < 4; ++i)
            //{
            //    zb.ProcessFace(ph_copy.Faces[i]);
            //}
            pictureBox1.Image = zb.GetImage();
            pictureBox1.Invalidate();
        }

        private void buttonZreset_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Invalidate();
        }

        public delegate double delFunct(double x, double y);
        private delFunct[] DicFun = new delFunct[] { (x, y) => Math.Sin(x + y), (x, y) => x * x + x * y + y * y,
                                                     (x, y) => 0.5 * (x * x + y * y)};

        private void buttonGraph_Click(object sender, EventArgs e)
        {
            double[] X = Parse(textBoxGraphX.Text);
            CheckLen(X, 2);
            double[] Y = Parse(textBoxGraphY.Text);
            CheckLen(Y, 2);
            double[] Step = Parse(textBoxGraphStep.Text);
            CheckLen(Step, 1);
            int step = (int)Step[0];
            ph = new Polyhedron();
            ph.faces = new Face[step * step * 2];
            double dx = (X[1] - X[0]) / step;
            double dy = (Y[1] - Y[0]) / step;
            delFunct fun = DicFun[comboBoxGraph.SelectedIndex];

            Point3d[,] points = new Point3d[step + 1, step + 1];
            for (int i = 0; i < step + 1; ++i)
            {
                for (int j = 0; j < step + 1; ++j)
                {
                    points[i, j] = new Point3d(X[0] + i * dx, Y[0] + j * dy, fun(X[0] + i * dx, Y[0] + j * dy));
                }
            }

            for (int i = 0; i < step; ++i)
            {
                for (int j = 0; j < step; ++j)
                {
                    ph.faces[2 * (i * step + j)] = Face.CreateTriangle(points[i, j], points[i, j + 1], points[i + 1, j + 1]);
                    ph.faces[2 * (i * step + j) + 1] = Face.CreateTriangle(points[i, j], points[i + 1, j], points[i + 1, j + 1]);
                }
            }

            ph.center = new Point3d((X[1] - X[0]) / 2, (Y[1] - Y[0]) / 2, fun((X[1] - X[0]) / 2, (Y[1] - Y[0]) / 2));

            Plot();
        }

        class Point3dCompare : IEqualityComparer<Point3d>
        {
            public bool Equals(Point3d p1, Point3d p2)
            {
                return p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
            }

            public int GetHashCode(Point3d p)
            {
                return p.X.GetHashCode() ^ p.Y.GetHashCode() ^ p.Z.GetHashCode();
            }
        }

        private Point find_point_screen(Point3d p)
        {
            Polyhedron temp = new Polyhedron();
            temp.faces = new Face[1];
            temp.faces[0] = Face.CreateTriangle(p, p, p);
            temp.center = p;

            if (radioButtonCenterProj.Checked)
            {
                Affine.CentralProjection(temp, 10);
            }
            else if (radioButtonIsoProj.Checked)
            {
                Affine.IsometricProjection(temp);
            }
            else if (radioButtonOrtoProj.Checked)
            {
                switch (comboBoxProjPlane.SelectedIndex)
                {
                    case 0:
                        Affine.OrtographicProjection(temp, 'z');
                        break;
                    case 1:
                        Affine.OrtographicProjection(temp, 'y');
                        break;
                    case 2:
                        Affine.OrtographicProjection(temp, 'x');
                        break;
                    default:
                        throw new Exception("Bad projection plane.");
                }
            }
            else
            {
                throw new Exception("Projection error.");
            }

            return temp.faces[0].edges[0].First.To2d();
        }

        private Color light_color(Color clr, double light)
        {
            Color res = Color.FromArgb(clr.A, clr);
            if (light < 0)
                res = Color.Black;
            else if (light <= 1)
            {
                int red = (int)(res.R * light);
                if (red < 0)
                    red = 0;
                int green = (int)(res.G * light);
                if (green < 0)
                    green = 0;
                int blue = (int)(res.B * light);
                if (blue < 0)
                    blue = 0;
                res = Color.FromArgb(red, green, blue);
            }
            else
            {
                int red = (int)(res.R + (255 - res.R) * (light - 1));
                if (red > 255)
                    red = 255;
                int green = (int)(res.G + (255 - res.G) * (light - 1));
                if (green > 255)
                    green = 255;
                int blue = (int)(res.B + (255 - res.B) * (light - 1));
                if (blue > 255)
                    blue = 255;
                res = Color.FromArgb(red, green, blue);
            }
            return res;
        }

        private Dictionary<int, int> interpolate(int i0, int d0, int i1, int d1)
        {
            Dictionary<int, int> res = new Dictionary<int, int>();
            if (i0 == i1)
            {
                res.Add(i0, d0);
                return res;
            }

            double a = (d1 - d0) / (double)(i1 - i0);
            double d = d0;
            for (int i = i0; i <= i1; ++i)
            {
                res.Add(i, (int)Math.Round(d));
                d += a;
            }

            return res;
        }

        private void GouraudShading(Polyhedron phd)
        {
            double vi = double.Parse(textBox3.Text);
            double vj = double.Parse(textBox5.Text);
            double vk = double.Parse(textBox4.Text);

            Polyhedron ph_copy = phd;
            List<Face> faces = new List<Face>(ph_copy.faces);

            Point3dCompare comp = new Point3dCompare();
            Dictionary<Point3d, List<Point3d>> normals = new Dictionary<Point3d, List<Point3d>>(comp);
            for (int i = phd.faces.Count() - 1; i >= 0; --i)
            {
                var face = phd.faces[i];
                Point3d p1 = face.edges[0].First;
                Point3d p2 = face.edges[1].First;
                Point3d p3 = face.edges[2].First;

                double[,] matrix = new double[2, 3];
                matrix[0, 0] = p2.X - p1.X;
                matrix[0, 1] = p2.Y - p1.Y;
                matrix[0, 2] = p2.Z - p1.Z;
                matrix[1, 0] = p3.X - p1.X;
                matrix[1, 1] = p3.Y - p1.Y;
                matrix[1, 2] = p3.Z - p1.Z;

                double ni = matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1];
                double nj = matrix[0, 2] * matrix[1, 0] - matrix[0, 0] * matrix[1, 2];
                double nk = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
                double d = -(ni * p1.X + nj * p1.Y + nk * p1.Z);

                Point3d pp = new Point3d(p1.X + ni, p1.Y + nj, p1.Z + nk);
                double val1 = ni * pp.X + nj * pp.Y + nk * pp.Z + d;
                double val2 = ni * phd.center.X + nj * phd.center.Y + nk * phd.center.Z + d;
                if (val1 * val2 > 0)
                {
                    ni = -ni;
                    nj = -nj;
                    nk = -nk;
                }

                Point3d norm = new Point3d(ni, nj, nk);
                foreach (var edge in face.edges)
                {
                    if (!normals.ContainsKey(edge.First))
                        normals.Add(edge.First, new List<Point3d>());
                    normals[edge.First].Add(norm);
                }

                if (ni * vi + nj * vj + nk * vk < 0)
                    faces.RemoveAt(i);
            }
            ph = new Polyhedron();
            ph.faces = new Face[faces.Count()];
            for (int j = 0; j < faces.Count(); ++j)
                ph.faces[j] = faces[j];
            ph.center = ph_copy.center;

            Dictionary<Point3d, Point3d> norm_verts = new Dictionary<Point3d, Point3d>(comp);
            foreach (var point in normals.Keys)
            {
                double x = normals[point].Sum(p => p.X) / normals[point].Count();
                double y = normals[point].Sum(p => p.Y) / normals[point].Count();
                double z = normals[point].Sum(p => p.Z) / normals[point].Count();
                norm_verts.Add(point, new Point3d(x, y, z));
            }

            Color color = Color.Black;
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    color = Color.Red;
                    break;
                case 1:
                    color = Color.Orange;
                    break;
                case 2:
                    color = Color.Yellow;
                    break;
                case 3:
                    color = Color.Green;
                    break;
                case 4:
                    color = Color.Cyan;
                    break;
                case 5:
                    color = Color.Blue;
                    break;
                case 6:
                    color = Color.DarkViolet;
                    break;
            }
            double light_x = double.Parse(textBox17.Text);
            double light_y = double.Parse(textBox18.Text);
            double light_z = double.Parse(textBox19.Text);

            double kd = double.Parse(textBox16.Text);
            double l0 = double.Parse(textBox15.Text);
            Dictionary<Point3d, double> lightness = new Dictionary<Point3d, double>(comp);
            foreach (var point in norm_verts.Keys)
            {
                Point3d n = norm_verts[point];
                double len_n = Math.Sqrt(n.X * n.X + n.Y * n.Y + n.Z * n.Z);
                Point3d l = new Point3d(light_x - point.X, light_y - point.Y, light_z - point.Z);
                double len_l = Math.Sqrt(l.X * l.X + l.Y * l.Y + l.Z * l.Z);
                double cos = (n.X * l.X + n.Y * l.Y + n.Z * l.Z) / len_n / len_l;
                double ll = kd * cos * l0;
                lightness.Add(point, ll);
            }

            Dictionary<Point3d, Point> points_screen = new Dictionary<Point3d, Point>(comp);
            foreach (var point in lightness.Keys)
                points_screen.Add(point, find_point_screen(point.Clone() as Point3d));

            Dictionary<Point3d, Color> colors = new Dictionary<Point3d, Color>(comp);
            foreach (var point in lightness.Keys)
            {
                //Point p = points_screen[point];
                //g.FillEllipse(new SolidBrush(color), p.X - 2, p.Y - 2, 5, 5);
                colors.Add(point, light_color(color, lightness[point]));
            }

            foreach (var face in ph.faces)
            {
                //try
                {
                    Point[] corners = new Point[face.edges.Count() + 1];
                    Point top = points_screen[face.edges[0].First];
                    Point bottom = top;

                    for (int i = 0; i < face.edges.Count(); ++i)
                    {
                        corners[i] = points_screen[face.edges[i].First];
                        if (corners[i].Y < top.Y)
                            top = corners[i];
                        if (corners[i].Y > bottom.Y)
                            bottom = corners[i];
                    }
                    corners[corners.Count() - 1] = corners[0];

                    //List<Point> border_points = new List<Point>();
                    Dictionary<int, List<Point>> border_points = new Dictionary<int, List<Point>>();
                    Dictionary<Point, double> lights = new Dictionary<Point, double>();
                    for (int i = 0; i < corners.Count() - 1; ++i)
                    {
                        Point p0 = corners[i], p1 = corners[i + 1];
                        double l_0 = lightness[face.edges[i].First];
                        double l_1 = lightness[face.edges[0].First];
                        if (i != corners.Count() - 2)
                            l_1 = lightness[face.edges[i + 1].First];

                        if (Math.Abs(p0.X - p1.X) > Math.Abs(p0.Y - p1.Y))
                        {
                            Dictionary<int, int> line = new Dictionary<int, int>();
                            if (p0.X > p1.X)
                                line = interpolate(p1.X, p1.Y, p0.X, p0.Y);
                            else
                                line = interpolate(p0.X, p0.Y, p1.X, p1.Y);
                            for (int x = p0.X; ; x += p0.X > p1.X ? -1 : 1)
                            {
                                Point p = new Point(x, line[x]);
                                //border_points.Add(p);
                                if (!border_points.ContainsKey(p.Y))
                                    border_points.Add(p.Y, new List<Point>());
                                border_points[p.Y].Add(p);
                                double l = l_0 + (l_1 - l_0) / line.Count() * Math.Abs(x - p0.X);
                                if (!lights.ContainsKey(p))
                                    lights.Add(p, l);
                                g.FillRectangle(new SolidBrush(light_color(color, l)), p.X, p.Y, 1, 1);
                                if (x == p1.X)
                                    break;
                            }
                        }
                        else
                        {
                            Dictionary<int, int> line = new Dictionary<int, int>();
                            if (p0.Y > p1.Y)
                                line = interpolate(p1.Y, p1.X, p0.Y, p0.X);
                            else
                                line = interpolate(p0.Y, p0.X, p1.Y, p1.X);
                            for (int y = p0.Y; ; y += p0.Y > p1.Y ? -1 : 1)
                            {
                                Point p = new Point(line[y], y);
                                //border_points.Add(p);
                                if (!border_points.ContainsKey(p.Y))
                                    border_points.Add(p.Y, new List<Point>());
                                border_points[p.Y].Add(p);
                                double l = l_0 + (l_1 - l_0) / line.Count() * Math.Abs(y - p0.Y);
                                if (!lights.ContainsKey(p))
                                    lights.Add(p, l);
                                g.FillRectangle(new SolidBrush(light_color(color, l)), p.X, p.Y, 1, 1);
                                if (y == p1.Y)
                                    break;
                            }

                        }
                    }

                    foreach (var y in border_points.Keys)
                    {
                        border_points[y].Sort((p_1, p_2) => p_1.X.CompareTo(p_2.X));
                        Point p0 = border_points[y].First();
                        Point p1 = border_points[y].Last();
                        double l_0 = lights[p0];
                        double l_1 = lights[p1];
                        for (int x = p0.X; x < p1.X; ++x)
                        {
                            double l = l_0 + (l_1 - l_0) / (p1.X - p0.X) * (x - p0.X);
                            g.FillRectangle(new SolidBrush(light_color(color, l)), x, y, 1, 1);
                        }
                    }
                }
                //catch { }


                /*
                Point[] points = new Point[face.edges.Count() * 2];
                for (int i = 0; i < face.edges.Count(); ++i)
                {
                    points[2 * i] = points_screen[face.edges[i].First];
                    if (i != 0)
                    {
                        int xx = (int)Math.Round((points[2 * i - 2].X + points[2 * i].X) / 2.0);
                        int yy = (int)Math.Round((points[2 * i - 2].Y + points[2 * i].Y) / 2.0);
                        points[2 * i - 1] = new Point(xx, yy);
                    }
                }
                int x = (int)Math.Round((points[0].X + points[face.edges.Count() * 2 - 2].X) / 2.0);
                int y = (int)Math.Round((points[0].Y + points[face.edges.Count() * 2 - 2].Y) / 2.0);
                points[face.edges.Count() * 2 - 1] = new Point(x, y);

                double sum_light = 0;
                Color[] clrs = new Color[face.edges.Count() * 2];
                for (int i = 0; i < face.edges.Count(); ++i)
                {
                    Color c = colors[face.edges[i].First];
                    clrs[2 * i] = c;
                    sum_light += lightness[face.edges[i].First];

                    if (i != 0)
                    {
                        double ll = (lightness[face.edges[i - 1].First] + lightness[face.edges[i].First]) / 2.0;
                        clrs[2 * i - 1] = light_color(color, ll);
                    }
                }
                double l = (lightness[face.edges[0].First] + lightness[face.edges[face.edges.Count() - 1].First]) / 2.0;
                clrs[face.edges.Count() * 2 - 1] = light_color(color, l);

                sum_light /= face.edges.Count();

                try
                {
                    System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                    //gp.AddLines(points);
                    gp.AddPolygon(points);
                    System.Drawing.Drawing2D.PathGradientBrush pgbrush = new System.Drawing.Drawing2D.PathGradientBrush(points);
                    pgbrush.SurroundColors = clrs;
                    pgbrush.CenterColor = light_color(color, sum_light);
                    g.FillPath(pgbrush, gp);
                }
                catch { }
                */

                /*
                Point start = points_screen[face.edges[0].First];
                Color clrstart = colors[face.edges[0].First];
                for (int pos = 2; pos < face.edges.Count(); ++pos)
                {
                    Point p1 = points_screen[face.edges[pos - 1].First];
                    Point p2 = points_screen[face.edges[pos].First];
                    int line_len = Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);

                    Color clr1 = colors[face.edges[pos - 1].First];
                    Color clr2 = colors[face.edges[pos].First];
                    double rstep = (double)(clr2.R - clr1.R) / line_len;
                    double gstep = (double)(clr2.G - clr1.G) / line_len;
                    double bstep = (double)(clr2.B - clr1.B) / line_len;
                    double xstep = (double)(p2.X - p1.X) / line_len;
                    double ystep = (double)(p2.Y - p1.Y) / line_len;

                    for (int i = 0; i <= line_len; ++i)
                    {
                        int x = (int)Math.Round(p1.X + xstep * i);
                        int y = (int)Math.Round(p1.Y + ystep * i);
                        //if (x == p2.X && y == p2.Y)
                          //  break;
                        int red = (int)Math.Round(clr1.R + rstep * i);
                        int green = (int)Math.Round(clr1.G + gstep * i);
                        int blue = (int)Math.Round(clr1.B + bstep * i);

                        int xnext = (int)Math.Round(x + xstep);
                        int ynext = (int)Math.Round(y + ystep);
                        if (x - xnext == 0 && y - ynext == 0)
                            if (Math.Abs(xstep) > Math.Abs(ystep))
                                xnext += xstep > 0 ? 1 : -1;
                            else
                                ynext += ystep > 0 ? 1 : -1;
                        g.DrawLine(new Pen(Color.FromArgb(red, green, blue)), x, y, xnext, ynext);

                        int lline_len = Math.Abs(start.X - x) + Math.Abs(start.Y - y);
                        double xxstep = (double)(start.X - x) / lline_len;
                        double yystep = (double)(start.Y - y) / lline_len;
                        double rrstep = (double)(clrstart.R - red) / lline_len;
                        double ggstep = (double)(clrstart.G - green) / lline_len;
                        double bbstep = (double)(clrstart.B - blue) / lline_len;
                        for (int j = 1; j < lline_len; ++j)
                        {
                            int xx = (int)Math.Round(x + xxstep * j);
                            int yy = (int)Math.Round(y + yystep * j);
                            //if (xx == start.X && yy == start.Y)
                              //  break;
                            int rred = (int)Math.Round(red + rrstep * j);
                            int ggreen = (int)Math.Round(green + ggstep * j);
                            int bblue = (int)Math.Round(blue + bbstep * j);

                            int xxnext = (int)Math.Round(xx + xxstep);
                            int yynext = (int)Math.Round(yy + yystep);
                            if (xx - xxnext == 0 && yy - yynext == 0)
                                if (Math.Abs(xxstep) > Math.Abs(yystep))
                                    xxnext += xxstep > 0 ? 1 : -1;
                                else
                                    yynext += yystep > 0 ? 1 : -1;
                            g.DrawLine(new Pen(Color.FromArgb(rred, ggreen, bblue)), xx, yy, xxnext, yynext);
                        }
                    }
                }*/

            }
            ph = phd;
            //System.Threading.Thread.Sleep(40);
        }
    }
}
