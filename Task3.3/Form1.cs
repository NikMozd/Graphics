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

        Tuple<int, int, int> find_next(Tuple<int, int, int> dot, int dir)
        {
            int x = dot.Item1;
            int y = dot.Item2;
            if (1 <= dir && dir <= 3)
                ++x;
            if (3 <= dir && dir <= 5)
                --y;
            if (5 <= dir && dir <= 7)
                --x;
            if (dir == 7 || dir == 0 || dir == 1)
                ++y;
            return new Tuple<int, int, int>(x, y, dir);
        }

        List<Tuple<int, int, int>> border = new List<Tuple<int, int, int>>();
        SortedDictionary<int, List<int>> pairs = new SortedDictionary<int, List<int>>();
        Tuple<int, int, int> first, last;
        Color color = Color.FromArgb(0, 0, 0); // цвет границы

        private void find_border(int x, int y, int stride)
        {
            // Находим настоящий старт
            while (color != Color.FromArgb(rgbValues[stride * y + 3 * x + 2],
                rgbValues[stride * y + 3 * x + 1],
                rgbValues[stride * y + 3 * x])) 
            {
                ++x;
            } 

            /*color = Color.FromArgb(rgbValues[stride * y + 3 * x + 2],
                rgbValues[stride * y + 3 * x + 1],
                rgbValues[stride * y + 3 * x]);*/

            // Начинаем выделять границу
            border.Clear();
            first = new Tuple<int, int, int>(x, y, 4);
            last = first;
            border.Add(first);
            //var lastlast = last;

            // Вторая точка границы
            // 0 - вниз, далее - против часовой стрелки
            int direction = 6;
            for (; ; direction += 1)
            {
                //if (direction == 8)
                //  return;
                direction = direction % 8;
                var next = find_next(last, direction);
                int xx = next.Item1;
                int yy = next.Item2;
                Color clr = Color.FromArgb(rgbValues[stride * yy + 3 * xx + 2],
                    rgbValues[stride * yy + 3 * xx + 1],
                    rgbValues[stride * yy + 3 * xx]);
                if (clr == color)
                {
                    border.Add(next);
                    last = next;
                    break;
                }
            }

            // Все остальные точки
            while (first.Item1 != last.Item1 || first.Item2 != last.Item2)
            //for (int k = 0; k < 1000; ++k)
            {
                int nd = (direction + 6) % 8;
                for (; ; nd += 1)
                {
                    //if (nd - 8 == (direction + 6) % 8)
                    //    return;
                    var new_dir = nd % 8;
                    if (nd == (direction + 4) % 8)
                        continue;
                    var next = find_next(last, new_dir);
                    int xx = next.Item1;
                    int yy = next.Item2;
                    Color clr = Color.FromArgb(rgbValues[stride * yy + 3 * xx + 2],
                        rgbValues[stride * yy + 3 * xx + 1],
                        rgbValues[stride * yy + 3 * xx]);
                    if (clr == color)
                    {
                        /*
                        // Точки по горизонтали
                        if (lastlast.Item1 == xx - 2 && last.Item1 == xx - 1 &&
                            lastlast.Item2 == yy && last.Item2 == yy)
                            pairs[yy].Add(last.Item1);
                        if (lastlast.Item1 == xx + 2 && last.Item1 == xx + 1 &&
                            lastlast.Item2 == yy && last.Item2 == yy)
                            pairs[yy].Add(last.Item1);*/

                        /*
                        // Точки - вертикальные пики
                        if (lastlast.Item2 == yy && last.Item2 == yy + 1)
                            pairs[yy+1].Add(last.Item1);
                        if (lastlast.Item2 == yy && last.Item2 == yy - 1)
                            pairs[yy-1].Add(last.Item1);*/

                        border.Add(next);
                        //lastlast = last;
                        last = next;
                        direction = new_dir;
                        break;
                    }
                }
            }
            border[0] = last;
        }

        private void modify_border()
        {
            if (!pairs.ContainsKey(first.Item2))
                pairs.Add(first.Item2, new List<int>());
            pairs[first.Item2].Add(first.Item1);

            for (int pos = 1; pos < border.Count() - 1; ++pos)
            {
                var curr = border[pos];
                var prev = border[pos - 1];
                var next = border[pos + 1];

                // Вертикальные пики
                if (prev.Item2 == curr.Item2 - 1 && next.Item2 == curr.Item2 - 1)
                {
                    if (!pairs.ContainsKey(curr.Item2))
                        pairs.Add(curr.Item2, new List<int>());
                    pairs[curr.Item2].Add(curr.Item1);
                    pairs[curr.Item2].Add(curr.Item1);
                }
                else if (prev.Item2 == curr.Item2 + 1 && next.Item2 == curr.Item2 + 1)
                {
                    if (!pairs.ContainsKey(curr.Item2))
                        pairs.Add(curr.Item2, new List<int>());
                    pairs[curr.Item2].Add(curr.Item1);
                    pairs[curr.Item2].Add(curr.Item1);
                }

                // Горизонтальные прямые
                else if (prev.Item1 + 1 == curr.Item1 && prev.Item2 == curr.Item2)
                {
                    ++pos;
                    while (border[pos].Item2 == curr.Item2)
                        ++pos;
                    --pos;
                    curr = border[pos];
                    next = border[pos + 1];

                    // слева направо, пришли наверх, уходим вниз (обе не нужны)
                    if (//curr.Item3 == 6 &&
                        (3 <= prev.Item3 && prev.Item3 <= 5) &&
                        (next.Item3 == 7 || next.Item3 == 0 || next.Item3 == 1))
                        pairs[prev.Item2].Remove(prev.Item1);
                    // слева направо, пришли наверх, уходим наверх (нужна только правая)
                    if (//curr.Item3 == 6 &&
                        (3 <= prev.Item3 && prev.Item3 <= 5) &&
                        (3 <= next.Item3 && next.Item3 <= 5))
                    {
                        pairs[prev.Item2].Remove(prev.Item1);
                        pairs[curr.Item2].Add(curr.Item1);
                    }
                    // слева направо, пришли вниз, уходим вниз (нужна только левая)
                    if (//curr.Item3 == 6 &&
                        (prev.Item3 == 7 || prev.Item3 == 0 || prev.Item3 == 1) &&
                        (next.Item3 == 7 || next.Item3 == 0 || next.Item3 == 1))
                        continue;
                    // слева направо, пришли вниз, уходим наверх (обе нужны)
                    if (//curr.Item3 == 6 &&
                        (prev.Item3 == 7 || prev.Item3 == 0 || prev.Item3 == 1) &&
                        (3 <= next.Item3 && next.Item3 <= 5))
                    {
                        if (!pairs.ContainsKey(curr.Item2))
                            pairs.Add(curr.Item2, new List<int>());
                        pairs[curr.Item2].Add(curr.Item1);
                    }

                }
                else if (prev.Item1 - 1 == curr.Item1 && prev.Item2 == curr.Item2)
                {
                    ++pos;
                    while (border[pos].Item2 == curr.Item2)
                        ++pos;
                    --pos;
                    curr = border[pos];
                    next = border[pos + 1];

                    // справа налево, пришли наверх, уходим вниз (обе нужны)
                    if (//curr.Item3 == 2 &&
                        (3 <= prev.Item3 && prev.Item3 <= 5) &&
                        (next.Item3 == 7 || next.Item3 == 0 || next.Item3 == 1))
                    {
                        if (!pairs.ContainsKey(curr.Item2))
                            pairs.Add(curr.Item2, new List<int>());
                        pairs[curr.Item2].Add(curr.Item1);
                    }
                    // справа налево, пришли наверх, уходим наверх (нужна только правая)
                    if (//curr.Item3 == 2 &&
                        (3 <= prev.Item3 && prev.Item3 <= 5) &&
                        (3 <= next.Item3 && next.Item3 <= 5))
                        continue;
                    // справа налево, пришли вниз, уходим вниз (нужна только левая)
                    if (//curr.Item3 == 6 &&
                        (prev.Item3 == 7 || prev.Item3 == 0 || prev.Item3 == 1) &&
                        (next.Item3 == 7 || next.Item3 == 0 || next.Item3 == 1))
                    {
                        pairs[prev.Item2].Remove(prev.Item1);
                        pairs[curr.Item2].Add(curr.Item1);
                    }
                    // справа налево, пришли вниз, ушли наверх (обе не нужны)
                    if (//curr.Item3 == 6 &&
                        (prev.Item3 == 7 || prev.Item3 == 0 || prev.Item3 == 1) &&
                        (3 <= next.Item3 && next.Item3 <= 5))
                        pairs[prev.Item2].Remove(prev.Item1);
                }

                // Всё остальное
                else
                {
                    if (!pairs.ContainsKey(curr.Item2))
                        pairs.Add(curr.Item2, new List<int>());
                    pairs[curr.Item2].Add(curr.Item1);
                }
            }

            for (int k = pairs.Keys.Min(); k <= pairs.Keys.Max(); ++k)
                pairs[k].Sort();
        }

        private void fill(int stride)
        {
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
            
            for (int k = pairs.Keys.Min(); k <= pairs.Keys.Max(); ++k)
            {
                for (int pos = 0; pos < pairs[k].Count() - 1; pos += 2)
                {
                    int a = pairs[k][pos];
                    int b = pairs[k][pos + 1];
                    bool restart = false;

                    for (int i = a + 1; i < b; ++i)
                        if (color != Color.FromArgb(rgbValues[stride * k + 3 * i + 2],
                        rgbValues[stride * k + 3 * i + 1], rgbValues[stride * k + 3 * i]))
                        {
                            rgbValues[stride * k + 3 * i + 2] = red;
                            rgbValues[stride * k + 3 * i + 1] = green;
                            rgbValues[stride * k + 3 * i] = blue;
                        }
                        else
                        {
                            find_border(i - 1, k + 1, stride);
                            modify_border();
                            restart = true;
                            break;
                        }
                    if (restart)
                        pos -= 2;
                }
            }
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

            pairs.Clear();
            find_border(x, y, bmpData.Stride);
            modify_border();
            fill(bmpData.Stride);

            /*for (int counter = 0; counter < rgbValues.Length - 2; counter += 3)
            {
                rgbValues[counter] = rgbValues[counter+1] = rgbValues[counter+2] = 255;                                
            }

            //for (int pos = 0; pos < border.Count(); ++pos)
            for (int k = pairs.Keys.Min(); k <= pairs.Keys.Max(); ++k)
                for (int l = 0; l < pairs[k].Count(); ++l)
            {
                rgbValues[bmpData.Stride * k + 3 * pairs[k][l] + 2] = 0;
                rgbValues[bmpData.Stride * k + 3 * pairs[k][l] + 1] = 0;
            }*/

            /*
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
            for (int k = pairs.Keys.Min(); k <= pairs.Keys.Max(); ++k)
                for (int l = 0; l < pairs[k].Count(); ++l)
                {
                    rgbValues[bmpData.Stride * k + 3 * pairs[k][l] + 2] = red;
                    rgbValues[bmpData.Stride * k + 3 * pairs[k][l] + 1] = green;
                    rgbValues[bmpData.Stride * k + 3 * pairs[k][l] + 0] = blue;
                }
            */

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
                bmp.SetPixel(xx * pictureBox1.Width / bmpData.Width, yy * pictureBox1.Height / bmpData.Height, 
                    Color.FromArgb(red, green, blue));
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

            /*int i = 0;
            for (int counter = 0; counter < bmp.Width * bmp.Height * 3; counter += 3)
            {
                bmp.SetPixel(i % bmp.Width, i / bmp.Width,
                    Color.FromArgb(rgbValues[counter + 2], rgbValues[counter + 1], rgbValues[counter]));
                i++;
            }
            pictureBox1.Refresh();*/
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