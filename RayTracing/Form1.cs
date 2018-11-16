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
            g = pictureBox1.CreateGraphics();
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
            ph = Polyhedron.CreateTetrahedron(new Point3d(0, 0, 0), new Point3d(2, 0, 0), new Point3d(0, 1, 0));
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
            if (checkBoxGouraud.Checked)
            {
                g.Clear(Color.Black);
                Point p = find_point_screen(new Point3d
                    (double.Parse(textBoxGouraudSrcX.Text), double.Parse(textBoxGouraudSrcY.Text), double.Parse(textBoxGouraudSrcZ.Text)));
                g.FillRectangle(new SolidBrush(Color.White), p.X - 2, p.Y - 2, 5, 5);
                Polyhedron pproj = ph.Clone() as Polyhedron;
                GouraudShading(pproj);
                return;
            }

            if (checkBoxZbuffer.Checked)
            {
                PlotZbuffer(CreateCam());
                return;
            }

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

            proj.Draw(g, Color.Black);
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
            //Polyhedron result = new Polyhedron();
            //result.faces = faces;

           
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
            //result.center = center;

            Polyhedron result = new Polyhedron(faces, center);
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

            Face result = new Face(points);
            //Face result = new Face();
            //result.edges = new Edge[points.Length];
            //for (int i = 1; i < points.Length; i++)
            //{
            //    result.edges[i - 1] = new Edge(points[i - 1], points[i]);
            //}
            //result.edges[points.Length - 1] = new Edge(points[points.Length - 1], points[0]);
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
            for (int i = 0; i < ph.Faces.Length - 1; i++)
            {
                Point3d[] points = new Point3d[ph.Faces[i].Edges.Length];
                for (int j = 0; j < ph.Faces[i].Edges.Length; j++)
                {
                    points[j] = ph.Faces[i].Edges[j].First;
                }
                string face = ParseFaceToLine(points);
                result += face;
                result += Environment.NewLine;
            }
            int i1 = ph.Faces.Length - 1;
            Point3d[] points1 = new Point3d[ph.Faces[i1].Edges.Length];
            for (int j = 0; j < ph.Faces[i1].Edges.Length; j++)
            {
                points1[j] = ph.Faces[i1].Edges[j].First;
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

            //Polyhedron p = new Polyhedron();
            Face[] ph_faces = new Face[steps * (points.Count() - 1)];
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

                ph_faces[pos++] = Face.CreateTriangle(points[0], curr_points[1], next_points[1]);
                ph_faces[pos++] = Face.CreateTriangle(points[pcount - 1], curr_points[pcount - 2], next_points[pcount - 2]);
                for (int j = 1; j < pcount - 2; ++j)
                    ph_faces[pos++] = Face.CreateSquare(curr_points[j], next_points[j], next_points[j + 1], curr_points[j + 1]);

                curr_points = next_points;
                curr_angle += angle;
            }

            ph_faces[pos++] = Face.CreateTriangle(points[0], curr_points[1], points[1]);
            ph_faces[pos++] = Face.CreateTriangle(points[pcount - 1], curr_points[pcount - 2], points[pcount - 2]);
            for (int j = 1; j < pcount - 2; ++j)
                ph_faces[pos++] = Face.CreateSquare(curr_points[j], points[j], points[j + 1], curr_points[j + 1]);

            x_center /= steps * (pcount - 2) + 2;
            y_center /= steps * (pcount - 2) + 2;
            z_center /= steps * (pcount - 2) + 2;

            double center = 0;
            for (int i = 0; i < pcount; ++i)
                center += points[i].X;
            center /= pcount;
            Point3d ph_center = new Point3d(0, 0, 0);
            switch (axis)
            {
                case "OX":
                    ph_center = new Point3d(center, 0, 0);
                    break;
                case "OY":
                    ph_center = new Point3d(0, center, 0);
                    break;
                case "OZ":
                    ph_center = new Point3d(0, 0, center);
                    break;
            }

            //p.center = new Point3d(x_center, y_center, z_center);

            //ph = p;
            ph = new Polyhedron(ph_faces, ph_center);
            Plot();
        }

        private void buttonRotate_Click(object sender, EventArgs e)
        {
            double vi = double.Parse(textBoxViewVectorX.Text);
            double vj = double.Parse(textBoxViewVectorY.Text);//double.Parse(textBox4.Text);
            double vk = double.Parse(textBoxViewVectorZ.Text);//double.Parse(textBox5.Text);
            Vector ViewVec = new Vector(vi, vj, vk);
            Polyhedron ph_copy = ph;

            for (int i = 0; i < 180; ++i)
            {
                double angleInRad = 2 / 180.0 * Math.PI;
                switch (comboBoxAxisRot.SelectedIndex)
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

                //List<Face> faces = new List<Face>(ph_copy.faces);
                //for (int j = faces.Count() - 1; j >= 0; --j)
                //{
                //    Point3d p1 = faces[j].edges[0].First;
                //    Point3d p2 = faces[j].edges[1].First;
                //    Point3d p3 = faces[j].edges[2].First;

                //    double[,] matrix = new double[2, 3];
                //    matrix[0, 0] = p2.X - p1.X;
                //    matrix[0, 1] = p2.Y - p1.Y;
                //    matrix[0, 2] = p2.Z - p1.Z;
                //    matrix[1, 0] = p3.X - p1.X;
                //    matrix[1, 1] = p3.Y - p1.Y;
                //    matrix[1, 2] = p3.Z - p1.Z;

                //    double ni = matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1];
                //    double nj = matrix[0, 2] * matrix[1, 0] - matrix[0, 0] * matrix[1, 2];
                //    double nk = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
                //    double d = -(ni * p1.X + nj * p1.Y + nk * p1.Z);

                //    Point3d pp = new Point3d(p1.X + ni, p1.Y + nj, p1.Z + nk);
                //    double val1 = ni * pp.X + nj * pp.Y + nk * pp.Z + d;
                //    double val2 = ni * ph_copy.center.X + nj * ph_copy.center.Y + nk * ph_copy.center.Z + d;
                //    if (val1 * val2 > 0)
                //    {
                //        ni = -ni;
                //        nj = -nj;
                //        nk = -nk;
                //    }

                //    if (ni * vi + nj * vj + nk * vk < 0)
                //        faces.RemoveAt(j);
                //}

                //ph = new Polyhedron();
                //ph.faces = new Face[faces.Count()];
                //for (int j = 0; j < faces.Count(); ++j)
                //    ph.faces[j] = faces[j];
                //ph.center = ph_copy.center;

                ph = FaceClipping(ph_copy, ViewVec);
                Plot();
                System.Threading.Thread.Sleep(40);
            }

            ph = ph_copy;
        }

        private Polyhedron FaceClipping(Polyhedron ph0, Vector ViewVec)
        {
            List<Face> faces = new List<Face>(ph0.Faces);
            
            for (int j = faces.Count() - 1; j >= 0; --j)
            {
                Vector v1 = new Vector(faces[j].Edges[0].First, faces[j].Edges[1].First);
                Vector v2 = new Vector(faces[j].Edges[0].First, faces[j].Edges[2].First);
                Vector NormVec = v1[v2];
                Vector Internal = new Vector(faces[j].Edges[0].First, ph0.Center);
                if (NormVec * Internal > 0)
                {
                    NormVec = -1 * NormVec;
                }

                if (NormVec * ViewVec <= 0)
                    faces.RemoveAt(j);
            }

            Polyhedron res = new Polyhedron(faces.ToArray(), ph0.Center);
            //Polyhedron res = new Polyhedron();
            //res.faces = new Face[faces.Count()];
            //for (int j = 0; j < faces.Count(); ++j)
            //    res.faces[j] = faces[j];
            //res.center = ph0.Center;
            return res;
        }

        public delegate double delFunct(double x, double y);
        private delFunct[] DicFun = new delFunct[] { (x, y) => Math.Sin(x + y), (x, y) => x * x + x * y + y * y - 3, 
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
            Face[] ph_faces = new Face[step * step * 2];
            double dx = (X[1] - X[0]) / step;
            double dy = (Y[1] - Y[0]) / step;
            delFunct fun = DicFun[comboBoxGraph.SelectedIndex];

            Point3d[] points = new Point3d[(step + 1) * (step + 1)];
            for (int i = 0; i < step + 1; ++i)
            {
                for (int j = 0; j < step + 1; ++j)
                {
                    points[i * (step + 1) + j] = new Point3d(X[0] + i * dx, Y[0] + j * dy, fun(X[0] + i * dx, Y[0] + j * dy));
                }
            }

            if (checkBoxHiddenLines.Checked)
            {
                // плавающий горизонт
                Vector[] corners = new Vector[4];
                corners[0] = new Vector(X[0], Y[0], 0).Normalize();
                corners[1] = new Vector(X[1], Y[0], 0).Normalize();
                corners[2] = new Vector(X[1], Y[1], 0).Normalize();
                corners[3] = new Vector(X[0], Y[1], 0).Normalize();
                HiddenLines(points, step + 1, corners);
            }
            else
            {
                // простая отрисовка
                for (int i = 0; i < step; ++i)
                {
                    for (int j = 0; j < step; ++j)
                    {
                        ph_faces[2 * (i * step + j)] = Face.CreateTriangle(points[i * (step + 1) + j], points[i * (step + 1) + j + 1], points[(i + 1) * (step + 1) + j + 1]);
                        ph_faces[2 * (i * step + j) + 1] = Face.CreateTriangle(points[i * (step + 1) + j], points[(i + 1) * (step + 1) + j], points[(i + 1) * (step + 1) + j + 1]);
                    }
                }

                Point3d ph_center = new Point3d((X[1] - X[0]) / 2, (Y[1] - Y[0]) / 2, fun((X[1] - X[0]) / 2, (Y[1] - Y[0]) / 2));

                ph = new Polyhedron(ph_faces, ph_center);
                Plot();
            }
        }

        private void HiddenLines(Point3d[] grid, int steps, Vector[] corners)
        {
            // преобразование всех точек к камере
            Polyhedron ptmp = new Polyhedron(new Face[1] { new Face(grid) }, new Point3d(0, 0, 0));
            Affine.MakeView(ptmp, CreateCam());
            grid = ptmp.Faces[0].GetPoints();

            // задание минимумов и максимумов для плавающего горизонта
            int[] min = new int[pictureBox1.Width];
            int[] max = new int[pictureBox1.Width];
            for (int i = 0; i < pictureBox1.Width; ++i)
            {
                min[i] = pictureBox1.Height;
                max[i] = -1;
            }

            // получение направления отрисовки
            Vector view = new Vector(double.Parse(textBoxViewVectorX.Text),
                                     double.Parse(textBoxViewVectorY.Text), 
                                     double.Parse(textBoxViewVectorZ.Text));
            int ind1 = -1, ind2 = -1;
            double max_scal1 = double.MinValue, max_scal2 = double.MinValue;
            for (int i = 0; i < 4; ++i)
            {
                double curr_scal = corners[i] * view;
                if (curr_scal > max_scal1)
                {
                    ind2 = ind1;
                    ind1 = i;
                    max_scal2 = max_scal1;
                    max_scal1 = curr_scal;
                }
                else if (curr_scal > max_scal2)
                {
                    ind2 = i;
                    max_scal2 = curr_scal;
                }
            }

            // разворот решетки
            Point3d[,] tmp_grid = new Point3d[steps, steps];
            switch (ind1)
            {
                case 0:
                    for (int i = 0; i < steps; ++i)
                        for (int j = 0; j < steps; ++j)
                            tmp_grid[i, j] = grid[i * steps + j];
                    break;
                case 1:
                    for (int i = 0; i < steps; ++i)
                        for (int j = 0; j < steps; ++j)
                            tmp_grid[steps - j - 1, i] = grid[i * steps + j];
                    break;
                case 2:
                    for (int i = 0; i < steps; ++i)
                        for (int j = 0; j < steps; ++j)
                            tmp_grid[steps - i - 1, steps - j - 1] = grid[i * steps + j];
                    break;
                case 3:
                    for (int i = 0; i < steps; ++i)
                        for (int j = 0; j < steps; ++j)
                            tmp_grid[j, steps - i - 1] = grid[i * steps + j];
                    break;
                default:
                    throw new Exception("Bad ind1");
            }
            if ((ind2 - ind1) == 1 && (ind2 - ind1) == -3)
            {
                for (int i = 0; i < steps; ++i)
                    for (int j = 0; j < steps; ++j)
                        grid[i * steps + j] = tmp_grid[i, j];
            }
            else
            {
                for (int i = 0; i < steps; ++i)
                    for (int j = 0; j < steps; ++j)
                        grid[i * steps + j] = tmp_grid[j, i];
            }

            // отрисовка
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Pen p = new Pen(Color.Black);
            for (int i = 0; i < steps * steps; i += steps)
            {
                Point first;
                Point second = grid[i].To2d();
                for (int j = 1; j < steps; ++j)
                {
                    first = second;
                    second = grid[i + j].To2d();
                    PlotHiddenLine(first, second, ref max, ref min, ref bmp, p);
                }
                
                if (i + steps >= steps * steps)
                    break;

                second = grid[i].To2d();
                for (int j = 0; j < steps; ++j)
                {
                    first = second;
                    second = grid[i + steps + j].To2d();
                    PlotHiddenLine(first, second, ref max, ref min, ref bmp, p);
                    
                    if (j == steps - 1)
                        break;

                    first = second;
                    second = grid[i + 1 + j].To2d();
                    PlotHiddenLine(first, second, ref max, ref min, ref bmp, p);
                }

            }
            pictureBox1.Image = bmp;
            pictureBox1.Refresh();
            p.Dispose();
        }

        private void PlotHiddenLine(Point first, Point second, ref int[] max, ref int[] min, ref Bitmap bmp, Pen p)
        {
            int dir_inc = second.X > first.X ? 1 : -1;
            for (int xi = first.X; xi != second.X + dir_inc; xi += dir_inc)
            {
                double ratio = 0;
                if ((second.X - first.X) == 0)
                {
                    ratio = 1;
                }
                else
                {
                    ratio = (double)(xi - first.X) / (double)(second.X - first.X);
                }
                int yi = (int)Math.Round(first.Y + ratio * (second.Y - first.Y));
                if (yi > max[xi])
                {
                    if ((second.X - first.X) == 0)
                        Graphics.FromImage(bmp).DrawLine(p, first, second);
                    else if ((xi == first.X) || max[xi - dir_inc] == -1)
                        bmp.SetPixel(xi, yi, Color.Black);
                    else
                        Graphics.FromImage(bmp).DrawLine(p, xi - dir_inc, max[xi - dir_inc], xi, yi);
                    max[xi] = yi;
                }
                if (yi < min[xi])
                {
                    if ((second.X - first.X) == 0)
                        Graphics.FromImage(bmp).DrawLine(p, first, second);
                    else if ((xi == first.X) || min[xi - dir_inc] == pictureBox1.Height)
                        bmp.SetPixel(xi, yi, Color.Black);
                    else
                        Graphics.FromImage(bmp).DrawLine(p, xi - dir_inc, min[xi - dir_inc], xi, yi);
                    min[xi] = yi;
                }
            }
        }

        Cam CreateCam()
        {
            double pos_x = double.Parse(textBoxCameraX.Text);
            double pos_y = double.Parse(textBoxCameraY.Text);
            double pos_z = double.Parse(textBoxCameraZ.Text);

            double view_x = double.Parse(textBoxViewVectorX.Text);
            double view_y = double.Parse(textBoxViewVectorY.Text);
            double view_z = double.Parse(textBoxViewVectorZ.Text);

            double vert_angle = double.Parse(textBoxVerticalAngle.Text) / 180.0 * Math.PI;

            Vector pos = new Vector(pos_x, pos_y, pos_z);
            Vector view = new Vector(view_x, view_y, view_z);
            view = view.Normalize();

            // вектор вертикали без учета угла наклона
            Vector vert;
            if (Math.Abs(view_x) < 1e-6 && Math.Abs(view_y) < 1e-6)
                vert = new Vector(-1, 0, 0);
            else
                vert = new Vector(0, 0, 1);
            double scalar_prod_vert_view = view * vert;
            vert = (vert - (view * scalar_prod_vert_view)).Normalize();

            // вектор горизонтали без учета угла наклона
            Vector hor = vert[view].Normalize();

            // учет угла наклона вертикали
            vert = (vert * Math.Cos(vert_angle) + hor * Math.Sin(vert_angle)).Normalize();
            hor = vert[view].Normalize();

            return new Cam(pos, view, -hor, -vert);
        }

        private void buttonRotCam_Click(object sender, EventArgs e)
        {
            Cam cam = CreateCam();
            double dist = cam.Pos.Norm();
            double angle = double.Parse(textBoxRotCam.Text) * Math.PI / 180;

            for (int i = 0; i < 25; i++)
            {
                // видовая матрица
                

                Polyhedron ph_copy = ph.Clone() as Polyhedron;
                Affine.MakeView(ph_copy, cam);
                // вывести
                if (!checkBoxZbuffer.Checked)
                {
                    Affine.CentralProjection(ph_copy, dist);
                    g.Clear(Color.White);
                    ph_copy.Draw(g, Color.Black);
                }
                else
                {
                    PlotZbuffer(cam);
                }
                System.Threading.Thread.Sleep(100);
                Affine.RotateCam(cam, dist, -angle);
                //MessageBox.Show("111");
            }
        }

        private void PlotZbuffer(Cam cam)
        {
            if (!ph.Colorized)
                ph.Colorize(new Random());
            Polyhedron ph_copy = ph.Clone() as Polyhedron;
            Affine.MakeView(ph_copy, cam);

            Zbuffer zb = new Zbuffer(pictureBox1.Width, pictureBox1.Height);
            
            foreach (var x in ph_copy.Faces)
            {
                zb.ProcessFace(x);
            }
            pictureBox1.Image = zb.GetImage();
            pictureBox1.Refresh(); 
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
            //Polyhedron temp = new Polyhedron();
            //temp.faces = new Face[1];
            //temp.faces[0] = Face.CreateTriangle(p, p, p);
            //temp.center = p;

            Polyhedron temp = new Polyhedron(new Face[] { Face.CreateTriangle(p, p, p) }, p);

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

            return temp.Faces[0].Edges[0].First.To2d();
        }

        private Color light_color(Color clr, double light)
        {
            Color res = Color.FromArgb(clr.A, clr);
            if (light < 0)
                res = Color.Black;
            else if (light <= 1)
            {
                /*int red = (int)(res.R * light);
                if (red < 0)
                    red = 0;
                int green = (int)(res.G * light);
                if (green < 0)
                    green = 0;
                int blue = (int)(res.B * light);
                if (blue < 0)
                    blue = 0;
                res = Color.FromArgb(red, green, blue);*/
                int red = (int)(ambient.R + (res.R - ambient.R) * light);
                int green = (int)(ambient.G + (res.G - ambient.G) * light);
                int blue = (int)(ambient.B + (res.B - ambient.B) * light);
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
            double vi = double.Parse(textBoxViewVectorX.Text);
            double vj = double.Parse(textBoxViewVectorY.Text);
            double vk = double.Parse(textBoxViewVectorZ.Text);

            Polyhedron ph_copy = phd;
            List<Face> faces = new List<Face>(ph_copy.Faces);

            Point3dCompare comp = new Point3dCompare();
            Dictionary<Point3d, List<Point3d>> normals = new Dictionary<Point3d, List<Point3d>>(comp);
            for (int i = phd.Faces.Count() - 1; i >= 0; --i)
            {
                var face = phd.Faces[i];
                Point3d p1 = face.Edges[0].First;
                Point3d p2 = face.Edges[1].First;
                Point3d p3 = face.Edges[2].First;

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
                double val2 = ni * phd.Center.X + nj * phd.Center.Y + nk * phd.Center.Z + d;
                if (val1 * val2 > 0)
                {
                    ni = -ni;
                    nj = -nj;
                    nk = -nk;
                }

                Point3d norm = new Point3d(ni, nj, nk);
                foreach (var edge in face.Edges)
                {
                    if (!normals.ContainsKey(edge.First))
                        normals.Add(edge.First, new List<Point3d>());
                    normals[edge.First].Add(norm);
                }

                if (ni * vi + nj * vj + nk * vk < 0)
                    faces.RemoveAt(i);
            }
            //ph = new Polyhedron();
            //ph.faces = new Face[faces.Count()];
            //for (int j = 0; j < faces.Count(); ++j)
            //    ph.faces[j] = faces[j];
            //ph.center = ph_copy.center;
            ph = new Polyhedron(faces.ToArray(), ph_copy.Center);

            Dictionary<Point3d, Point3d> norm_verts = new Dictionary<Point3d, Point3d>(comp);
            foreach (var point in normals.Keys)
            {
                double x = normals[point].Sum(p => p.X) / normals[point].Count();
                double y = normals[point].Sum(p => p.Y) / normals[point].Count();
                double z = normals[point].Sum(p => p.Z) / normals[point].Count();
                norm_verts.Add(point, new Point3d(x, y, z));
            }

            Color color = Color.Black;
            switch (comboBoxGouraudColor.SelectedIndex)
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
            double light_x = double.Parse(textBoxGouraudSrcX.Text);
            double light_y = double.Parse(textBoxGouraudSrcY.Text);
            double light_z = double.Parse(textBoxGouraudSrcZ.Text);

            double kd = double.Parse(textBoxGouraudDiffuse.Text);
            double l0 = double.Parse(textBoxGouraudPower.Text);
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
            
            foreach (var face in ph.Faces)
            {
                //try
                {
                    Point[] corners = new Point[face.Edges.Count() + 1];
                    Point top = points_screen[face.Edges[0].First];
                    Point bottom = top;

                    for (int i = 0; i < face.Edges.Count(); ++i)
                    {
                        corners[i] = points_screen[face.Edges[i].First];
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
                        double l_0 = lightness[face.Edges[i].First];
                        double l_1 = lightness[face.Edges[0].First];
                        if (i != corners.Count() - 2)
                            l_1 = lightness[face.Edges[i + 1].First];

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

        private List<Point3d> make_grid_corners(double dist)
        {
            /*var w = pictureBox1.Width / 2;
            var h = pictureBox1.Height / 2;

            Point3d p1 = new Point3d(-w, -h, d);
            Point3d p2 = new Point3d(w, -h, d);
            Point3d p3 = new Point3d(-w, h, d);
            Point3d p4 = new Point3d(w, h, d);
            
            Cam cam = CreateCam();
            AffineMatrix m = AffineMatrix.CreateViewMatrix(cam.Pos, cam.View, cam.Hor, cam.Vert);

            p1 = m * p1;
            p2 = m * p2;
            p3 = m * p3;
            p4 = m * p4;

            List<Point3d> res = new List<Point3d>();
            res.Add(p1);
            res.Add(p2);
            res.Add(p3);
            res.Add(p4);
            return res;*/

            var w = pictureBox1.Width / 2;
            var h = pictureBox1.Height / 2;
            Cam cam = CreateCam();

            Vector view = cam.View * dist;
            Point3d m0 = new Point3d(
                cam.Pos.X + view.X,
                cam.Pos.Y + view.Y,
                cam.Pos.Z + view.Z);

            /*double a = cam.View.X;
            double b = cam.View.Y;
            double c = cam.View.Z;
            double d = -(a * m0.X + b * m0.Y + c * m0.Z);
            */

            Vector hor = cam.Hor * w;
            Vector vert = cam.Vert * h;
            Point3d p1 = new Point3d(
                m0.X - hor.X - vert.X,
                m0.Y - hor.Y - vert.Y,
                m0.Z - hor.Z - vert.Z);
            Point3d p2 = new Point3d(
                m0.X + hor.X - vert.X,
                m0.Y + hor.Y - vert.Y,
                m0.Z + hor.Z - vert.Z);
            Point3d p3 = new Point3d(
                m0.X - hor.X + vert.X,
                m0.Y - hor.Y + vert.Y,
                m0.Z - hor.Z + vert.Z);
            Point3d p4 = new Point3d(
                m0.X + hor.X + vert.X,
                m0.Y + hor.Y + vert.Y,
                m0.Z + hor.Z + vert.Z);

            List<Point3d> res = new List<Point3d>();
            res.Add(p1);
            res.Add(p2);
            res.Add(p3);
            res.Add(p4);
            return res;
        }

        List<Object> objects = new List<Object>();
        List<Sphere> spheres = new List<Sphere>();
        Dictionary<Object, Color> colors = new Dictionary<Object, Color>();
        Dictionary<Object, double> diffuse = new Dictionary<Object, double>();
        Dictionary<Object, double> reflect = new Dictionary<Object, double>();
        Dictionary<Object, double> trans = new Dictionary<Object, double>();
        Dictionary<Object, double> refract = new Dictionary<Object, double>();

        List<Point3d> lights = new List<Point3d>();
        Dictionary<Point3d, double> lights_power = new Dictionary<Point3d, double>();

        Color ambient = Color.Black;
        const double eps = 1e-6;

        private List<Sphere> find_spheres(List<Polyhedron> objects)
        {
            List<Sphere> spheres = new List<Sphere>();
            foreach (var obj in objects)
                spheres.Add(new Sphere(obj));
            return spheres;
        }

        private List<Point3d> find_rays ()
        {
            var corners = make_grid_corners(pictureBox1.Width * Math.Sqrt(3) / 2); // было 100
            List<Point3d> points = new List<Point3d>();
            var w = pictureBox1.Width;
            var h = pictureBox1.Height;

            var step_h_x = (corners[2].X - corners[0].X) / h;
            var step_h_y = (corners[2].Y - corners[0].Y) / h;
            var step_h_z = (corners[2].Z - corners[0].Z) / h;

            var step_w_x = (corners[1].X - corners[0].X) / w;
            var step_w_y = (corners[1].Y - corners[0].Y) / w;
            var step_w_z = (corners[1].Z - corners[0].Z) / w;

            for (int i = 0; i < h; ++i)
            {
                var p = new Point3d(
                    corners[0].X + step_h_x * i,
                    corners[0].Y + step_h_y * i,
                    corners[0].Z + step_h_z * i);
                for (int j = 0; j < w; ++j)
                {
                    points.Add(new Point3d(
                        p.X + step_w_x * j, 
                        p.Y + step_w_y * j, 
                        p.Z + step_w_z * j));
                }
            }

            return points;

            /*int w = pictureBox1.Width;
            int h = pictureBox1.Height;
            List<Point3d> points = new List<Point3d>();

            Point Point0 = new Point(200, 170);
            int pixelsPerUnit = 40;

            Cam cam = CreateCam();
            var m = AffineMatrix.CreateViewMatrix(cam.Pos, cam.View, cam.Hor, cam.Vert);

            for (int i = 0; i < h; ++i)
                for (int j = 0; j < w; ++j)
                {
                    // x = j, y = i
                    double yy = (double)(j - Point0.X) / pixelsPerUnit;
                    double zz = (double)(i - Point0.Y) / pixelsPerUnit;
                    //int xp = (int)Math.Round(Point0.X + pixelsPerUnit * y);
                    //int yp = (int)Math.Round(Point0.Y - pixelsPerUnit * z);
                    double xx = 0.8; // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    //points.Add(new Point3d(xx, yy, zz));

                    yy /= -1;
                    zz /= -1;

                    Vector x = new Vector(m * new Point3d(1, 0, 0));
                    Vector y = new Vector(m * new Point3d(0, 1, 0));
                    Vector z = new Vector(m * new Point3d(0, 0, 1));

                    var m_ = AffineMatrix.CreateViewMatrix(cam.Pos, x, y, z);
                    var p = m_ * new Point3d(xx, yy, zz);
                    points.Add(p);
                }

            return points;*/
        }

        private bool are_crossed(Point3d cam_pos, Point3d ray_pos, Sphere s)
        {
            Vector n = new Vector(cam_pos, ray_pos);
            Vector v = new Vector(cam_pos, s.C);
            double dist = n[v].Norm() / n.Norm();
            return dist <= s.R;
        }

        private bool find_cross(Point3d cam_pos, Point3d ray_pos, Sphere s, ref Point3d t)
        {
            Vector d = new Vector(
                ray_pos.X - cam_pos.X,
                ray_pos.Y - cam_pos.Y,
                ray_pos.Z - cam_pos.Z);
            Vector c = new Vector(
                cam_pos.X - s.C.X,
                cam_pos.Y - s.C.Y,
                cam_pos.Z - s.C.Z);

            double k1 = d * d,
                   k2 = 2 * (c * d),
                   k3 = (c * c) - s.R * s.R;
            double D = k2 * k2 - 4 * k1 * k3;
            if (D < 0)
                return false;

            double x1 = (-k2 + Math.Sqrt(D)) / (2 * k1);
            double x2 = (-k2 - Math.Sqrt(D)) / (2 * k1);
            double x = 0;
            if (x1 < eps && x2 < eps)
                return false;
            else if (x1 < eps)
                x = x2;
            else if (x2 < eps)
                x = x1;
            else
                x = x1 < x2 ? x1 : x2;

            t = new Point3d(
                cam_pos.X + d.X * x,
                cam_pos.Y + d.Y * x,
                cam_pos.Z + d.Z * x);
            return true;
        }

        private Color add_colors(Color diff, Color refl, Color trans, bool is_refl, bool is_trans)
        {
            int r = diff.R, g = diff.G, b = diff.B;
            if (is_refl)
            {
                r += refl.R;
                g += refl.G;
                b += refl.B;
            }
            if (is_trans)
            {
                r += trans.R;
                g += trans.G;
                b += trans.B;
            }
            if (r > 255)
                r = 255;
            if (g > 255)
                g = 255;
            if (b > 255)
                b = 255;
            return Color.FromArgb(r, g, b);
        }

        private Color ray_step(Point3d start, Point3d p, double intense)
        {
            if (intense < 0.01)
                return Color.Black;

            double dist = double.MaxValue;
            Object obj = null;
            Point3d cross = new Point3d();
            foreach (var o in objects)
            {
                Point3d t = new Point3d();
                //if (are_crossed(cam_pos, points[i * pictureBox1.Width + j], s))
                //if (find_cross(start, p, o, ref t))
                if (o.find_cross(start, p, ref t))
                {
                    double d1 = new Vector(start, t).Norm();
                    if (d1 < dist)
                    {
                        dist = d1;
                        obj = o;
                        cross = t;
                    }
                }
            }

            if (obj == null)
                return ambient;

            // диффузное освещение
            double ldiff = 0;
            Point3d tt = new Point3d();
            foreach (var l in lights)
            {
                bool flag = false;
                foreach (var o in objects)
                {
                    //if (find_cross(cross, l, s, ref tt))
                    if (o.find_cross(cross, l, ref tt) && new Vector(tt, cross).Norm() < new Vector(l, cross).Norm())
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                    continue;
                //ll += lights_power[l];

                double kd = diffuse[obj];
                double l0 = lights_power[l];
                Vector N = obj.normal(cross);
                Vector L = new Vector(cross, l);
                if (N * L < 0)
                    N = -N;
                double cos = (N * L) / N.Norm() / L.Norm();
                ldiff += kd * cos * l0;
            }

            Color clr_diff = light_color(colors[obj], ldiff * intense);

            // отражение
            Color clr_refl = Color.Black;
            if (reflect[obj] > 0)
            {
                Vector l = new Vector(start, p);
                l = l.Normalize();
                Vector n = obj.normal(cross);
                n = n.Normalize();
                if (n * l > 0)
                    n = -n;
                Vector r = l - 2 * n * (n * l);
                Point3d p_new = new Point3d(
                    cross.X + r.X,
                    cross.Y + r.Y,
                    cross.Z + r.Z);
                clr_refl = ray_step(cross, p_new, intense * reflect[obj]);
            }

            //преломление
            Color clr_trans = Color.Black;
            if (trans[obj] > 0)
            {
                Vector l = new Vector(start, p);
                l = l.Normalize();
                Vector n = obj.normal(cross);
                n = n.Normalize();
                if (n * l > 0)
                    n = -n;
                double coef = 1 / refract[obj];
                double cos = Math.Sqrt(1 - coef * coef * (1 - (n * l) * (n * l)));
                Vector t = coef * l - (cos + coef * (n * l)) * n;
                Point3d p_new = new Point3d(
                    cross.X + t.X,
                    cross.Y + t.Y,
                    cross.Z + t.Z);
                //if (!find_cross(cross, p_new, sph, ref tt))
                if (!obj.find_cross(cross, p_new, ref tt))
                    return clr_diff;

                l = new Vector(cross, p_new);
                l = l.Normalize();
                n = obj.normal(tt);
                n.Normalize();
                if (n * l > 0)
                    n = -n;
                coef = refract[obj];
                cos = Math.Sqrt(1 - coef * coef * (1 - (n * l) * (n * l)));
                t = coef * l - (cos + coef * (n * l)) * n;
                p_new = new Point3d(
                    tt.X + t.X,
                    tt.Y + t.Y,
                    tt.Z + t.Z);

                clr_trans = ray_step(tt, p_new, intense * trans[obj]);
            }

            return add_colors(clr_diff, clr_refl, clr_trans, reflect[obj] > 0, trans[obj] > 0);
        }

        private void Init()
        {
            objects.Clear();
            colors.Clear();
            diffuse.Clear();
            reflect.Clear();
            trans.Clear();
            refract.Clear();

            /*
            objects.Add(new Sphere(new Point3d(0, 0, 0), 1));
            colors.Add(objects.Last(), Color.White);
            diffuse.Add(objects.Last(), 0.7);
            reflect.Add(objects.Last(), 0.2);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 2.42);

            objects.Add(new Sphere(new Point3d(2, 4, 6), 2));
            colors.Add(objects.Last(), Color.Cyan);
            diffuse.Add(objects.Last(), 0.8);
            reflect.Add(objects.Last(), 0.1);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 2.42);

            objects.Add(new Sphere(new Point3d(3, 0, 0), 1));
            colors.Add(objects.Last(), Color.Fuchsia);
            diffuse.Add(objects.Last(), 0.6);
            reflect.Add(objects.Last(), 0.2);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 2.42);

            objects.Add(new Sphere(new Point3d(0, 3, 1), 2));
            colors.Add(objects.Last(), Color.Salmon);
            diffuse.Add(objects.Last(), 0.5);
            reflect.Add(objects.Last(), 0.2);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 2.42);
            */
            
            objects.Add(new Sphere(new Point3d(0, 0, 0), 1));
            colors.Add(objects.Last(), Color.White);
            diffuse.Add(objects.Last(), 0.5);
            reflect.Add(objects.Last(), 0.5);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 2.42);

            objects.Add(new Sphere(new Point3d(2, 0, 0), 1));
            colors.Add(objects.Last(), Color.Red);
            diffuse.Add(objects.Last(), 0.5);
            reflect.Add(objects.Last(), 0.5);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 2.42);

            objects.Add(new Sphere(new Point3d(0, 2, 0), 1));
            colors.Add(objects.Last(), Color.Green);
            diffuse.Add(objects.Last(), 0.5);
            reflect.Add(objects.Last(), 0.3);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 2.42);

            objects.Add(new Sphere(new Point3d(0, 0, 2), 1));
            colors.Add(objects.Last(), Color.Blue);
            diffuse.Add(objects.Last(), 0.5);
            reflect.Add(objects.Last(), 0.1);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 2.42);
            
            objects.Add(new Sphere(new Point3d(3, 3, 3), 1));
            colors.Add(objects.Last(), Color.White);
            diffuse.Add(objects.Last(), 0.1);
            reflect.Add(objects.Last(), 0);
            trans.Add(objects.Last(), 0.9);
            refract.Add(objects.Last(), 2.42);

            objects.Add(new Wall(
                new Point3d(-10, -10, -10), 
                new Point3d(-10, -10, 10), 
                new Point3d(-10, 10, 10), 
                new Point3d(-10, 10, -10)));
            colors.Add(objects.Last(), Color.White);
            diffuse.Add(objects.Last(), 0.8);
            reflect.Add(objects.Last(), 0.2);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 1);

            objects.Add(new Wall(
                new Point3d(-10, 10, -10),
                new Point3d(-10, 10, 10),
                new Point3d(10, 10, 10),
                new Point3d(10, 10, -10)));
            colors.Add(objects.Last(), Color.Red);
            diffuse.Add(objects.Last(), 0.8);
            reflect.Add(objects.Last(), 0.2);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 1);

            objects.Add(new Wall(
                new Point3d(10, 10, -10),
                new Point3d(10, 10, 10),
                new Point3d(10, -10, 10),
                new Point3d(10, -10, -10)));
            colors.Add(objects.Last(), Color.Green);
            diffuse.Add(objects.Last(), 0.8);
            reflect.Add(objects.Last(), 0.2);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 1);

            objects.Add(new Wall(
                new Point3d(10, -10, -10),
                new Point3d(10, -10, 10),
                new Point3d(-10, -10, 10),
                new Point3d(-10, -10, -10)));
            colors.Add(objects.Last(), Color.Blue);
            diffuse.Add(objects.Last(), 0.8);
            reflect.Add(objects.Last(), 0.2);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 1);

            objects.Add(new Wall(
               new Point3d(-10, -10, 10),
               new Point3d(10, -10, 10),
               new Point3d(10, 10, 10),
               new Point3d(-10, 10, 10)));
            colors.Add(objects.Last(), Color.Purple);
            diffuse.Add(objects.Last(), 0.8);
            reflect.Add(objects.Last(), 0.2);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 1);

            objects.Add(new Wall(
               new Point3d(-10, -10, -10),
               new Point3d(10, -10, -10),
               new Point3d(10, 10, -10),
               new Point3d(-10, 10, -10)));
            colors.Add(objects.Last(), Color.Purple);
            diffuse.Add(objects.Last(), 0.8);
            reflect.Add(objects.Last(), 0.2);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 1);

            /*
            objects.Add(new Sphere(new Point3d(5, 1, 8), 1));
            colors.Add(objects.Last(), Color.White);
            diffuse.Add(objects.Last(), 0);
            reflect.Add(objects.Last(), 1);
            trans.Add(objects.Last(), 0);
            refract.Add(objects.Last(), 1);
            */

            lights.Clear();
            lights_power.Clear();

            /*
            lights.Add(new Point3d(7, 7, 7));
            lights_power.Add(lights[0], 2);

            lights.Add(new Point3d(-2, -2, -3));
            lights_power.Add(lights[1], 3);
            */
            /*
            lights.Add(new Point3d(8, 0, 8));
            lights_power.Add(lights.Last(), 1);

            lights.Add(new Point3d(0, 8, 8));
            lights_power.Add(lights.Last(), 1);
            */

            lights.Add(new Point3d(5, 0, 5));
            lights_power.Add(lights.Last(), 1);

            lights.Add(new Point3d(0, 0, 9));
            lights_power.Add(lights.Last(), 1);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //add_object(sender, e);
            //var spheres = find_spheres(objects);
            Init();

            var points = find_rays();

            var cam_pos = new Point3d(
                double.Parse(textBoxCameraX.Text), 
                double.Parse(textBoxCameraY.Text), 
                double.Parse(textBoxCameraZ.Text));
            /*
            for (int i = 0; i < pictureBox1.Height; ++i)
                for (int j = 0; j < pictureBox1.Width; ++j)
                {
                    var p = find_point_screen(points[i * pictureBox1.Width + j]);
                    g.FillRectangle(new SolidBrush(Color.Red), p.X, p.Y, 1, 1);
                }
            */

            g.Clear(ambient);
            for (int i = 0; i < pictureBox1.Height; i += 1)
                for (int j = 0; j < pictureBox1.Width; j += 1)
                {
                    if (i == pictureBox1.Height / 2)
                        ;
                    var clr = ray_step(cam_pos, points[i * pictureBox1.Width + j], 1);
                    g.FillRectangle(new SolidBrush(clr), j, i, 1, 1);
                    
                }
        }
    }
}
