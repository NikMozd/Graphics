using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task3._2
{
    public partial class Form1 : Form
    {
        byte[] rgbValues;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] colors = { "Красный", "Чёрный", "Синий", "Зелёный", "Белый" };
            listBox1.Items.AddRange(colors);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        Tuple<int, int> find_next(Tuple<int, int> dot, int dir)
        {
            int x = dot.Item1;
            int y = dot.Item2;
            if (1 <= dir && dir <= 3)
                ++y;
            if (3 <= dir && dir <= 5)
                --x;
            if (5 <= dir && dir <= 7)
                --y;
            if (dir == 7 || dir == 0 || dir == 1)
                ++x;
            return new Tuple<int, int>(x, y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Bitmap.FromFile(label1.Text);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            Bitmap bmp = pictureBox1.Image as Bitmap;

            // Lock the bitmap's bits.  
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Находим настоящий старт
            var x = int.Parse(textBox1.Text) * bmpData.Width / pictureBox1.Width;
            var y = int.Parse(textBox2.Text) * bmpData.Height / pictureBox1.Height;
            var color = Color.FromArgb(rgbValues[(bmpData.Stride * y + 3 * x) + 2],
                rgbValues[bmpData.Stride * y + 3 * x + 1],
                rgbValues[bmpData.Stride * y + 3 * x]);
            do
            {
                ++x;
            } while (color == Color.FromArgb(rgbValues[bmpData.Stride * y + 3 * x + 2],
                rgbValues[bmpData.Stride * y + 3 * x + 1],
                rgbValues[bmpData.Stride * y + 3 * x]));

            // Начинаем выделять границу
            LinkedList<Tuple<int, int>> border = new LinkedList<Tuple<int, int>>();
            var p = new Tuple<int, int>(x, y);
            var first = border.AddLast(p);
            var last = first;

            // Вторая точка границы
            // 0 - вниз, далее - против часовой стрелки
            int direction = 0;
            for (; ; ++direction)
            {
                if (direction == 8)
                    return;
                var next = find_next(last.Value, direction);
                int xx = next.Item1;
                int yy = next.Item2;
                Color clr = Color.FromArgb(rgbValues[bmpData.Stride * yy + 3 * xx + 2],
                    rgbValues[bmpData.Stride * yy + 3 * xx + 1],
                    rgbValues[bmpData.Stride * yy + 3 * xx]);
                if (clr == color)
                {
                    p = new Tuple<int, int>(xx, yy);
                    last = border.AddLast(p);
                    break;
                }
            }

            // Все остальные точки
            //while (first.Value != last.Value)
            for (int k = 0; k < 10000; ++k)
            {
                int nd = (direction + 6) % 8;
                for (; ; ++nd)
                {
                    if (nd - 8 == (direction + 6) % 8)
                        return;
                    var new_dir = nd % 8;
                    if (nd == (direction + 4) % 8)
                        continue;
                    var next = find_next(last.Value, new_dir);
                    int xx = next.Item1;
                    int yy = next.Item2;
                    Color clr = Color.FromArgb(rgbValues[bmpData.Stride * yy + 3 * xx + 2],
                        rgbValues[bmpData.Stride * yy + 3 * xx + 1],
                        rgbValues[bmpData.Stride * yy + 3 * xx]);
                    if (clr == color)
                    {
                        p = new Tuple<int, int>(xx, yy);
                        last = border.AddLast(p);
                        direction = new_dir;
                        break;
                    }
                }
            }

            // Окрасить границу
            byte red = 0, green = 0, blue = 0;
            switch (listBox1.SelectedIndex)
            {
                case 0:
                    red = 255;
                    green = blue = 0;
                    break;
                case 1:
                    red = green = blue = 0;
                    break;
                case 2:
                    blue = 255;
                    red = green = 0;
                    break;
                case 3:
                    green = 255;
                    red = blue = 0;
                    break;
                case 4:
                    red = green = blue = 255;
                    break;
            }
            while (first != last)
            {
                var xx = first.Value.Item1;
                var yy = first.Value.Item2;
                //label4.Text += '(' + xx.ToString() + ',' + yy.ToString() + ") ";
                rgbValues[bmpData.Stride * yy + 3 * xx + 2] = red;
                rgbValues[bmpData.Stride * yy + 3 * xx + 1] = green;
                rgbValues[bmpData.Stride * yy + 3 * xx] = blue;
                first = first.Next;
            }

            /*bmp.UnlockBits(bmpData);
            byte red = 0, green = 0, blue = 0;
            switch (listBox1.SelectedIndex)
            {
                case 0:
                    red = 255;
                    green = blue = 0;
                    break;
                case 1:
                    red = green = blue = 0;
                    break;
                case 2:
                    blue = 255;
                    red = green = 0;
                    break;
                case 3:
                    green = 255;
                    red = blue = 0;
                    break;
                case 4:
                    red = green = blue = 255;
                    break;
            }
            while (first != last)
            {
                var xx = first.Value.Item1;
                var yy = first.Value.Item2;
                label4.Text += '(' + xx.ToString() + ',' + yy.ToString() + ") ";
                bmp.SetPixel(xx, yy, Color.FromArgb(red, green, blue));
                first = first.Next;
            }
            pictureBox1.Refresh();*/

            // Set every third value to 255. A 24bpp bitmap will look red.  
            for (int counter = 0; counter < rgbValues.Length; counter += 3)
            {
                //rgbValues[counter+1] = 255;                                
            }
            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            int i = 0;
            for (int counter = 0; counter < rgbValues.Length; counter += 3)
            {
                bmp.SetPixel(i % bmp.Width, i / bmp.Width,
                    Color.FromArgb(rgbValues[counter + 2], rgbValues[counter + 1], rgbValues[counter]));
                i++;

            }
            pictureBox1.Refresh();
    }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            label1.Text = openFileDialog1.FileName;
            pictureBox1.Image = Bitmap.FromFile(label1.Text);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = e.X.ToString();
            textBox2.Text = e.Y.ToString();
        }
    }
}
