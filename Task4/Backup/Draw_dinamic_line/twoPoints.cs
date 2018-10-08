using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Draw_dinamic_line
{
    class twoPoints
    {
        public Point p1, p2;
        public twoPoints(Point p1, Point p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}
