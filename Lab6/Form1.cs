using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
    }
}
