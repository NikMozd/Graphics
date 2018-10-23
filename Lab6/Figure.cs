using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    interface IFigure
    {
        IFigure Clone();
        
        void Draw(Graphics g, Color c);
    }

    class Point3d : IFigure
    {
        public Point3d()
        {
            /* EMPTY */
        }
        
        public Point3d(double a, double b, double c)
        {
            x = a;
            y = b;
            z = c;
        }

        public Point3d(Point3d a)
        {
            x = a.x;
            y = a.y;
            z = a.z;
        }

        public void Copy(Point3d a)
        {
            x = a.x;
            y = a.y;
            z = a.z;
        }

        public Point To2d()
        {
            int xp = (int)Math.Round(Point0.X + pixelsPerUnit * y);
            int yp = (int)Math.Round(Point0.Y - pixelsPerUnit * z);

            return new Point(xp, yp);
        }
        
        protected double x, y, z;

        public double X
        {
            get
            {
                return x;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
        }

        public double Z
        {
            get
            {
                return z;
            }
        }

        public IFigure Clone()
        {
            return new Point3d(this);
        }

        public void Draw(Graphics g, Color c) { /* EMPTY */ }
        
        static Point Point0 = new Point(200, 170);

        public static void DrawPoint0(Graphics g, Color c)
        {
            SolidBrush b = new SolidBrush(c);
            g.FillEllipse(b, Point0.X - 2, Point0.Y - 2, 5, 5);
        }

        int pixelsPerUnit = 40;
    }

    class Edge : IFigure
    {
        public Edge ()
        {
            /*empty*/
        }
        
        public Edge(Point3d a, Point3d b)
        {
            this.a = new Point3d(a); 
            this.b = new Point3d(b);
        }

        public Point3d First
        {
            get
            {
                return a;
            }
        }

        public Point3d Second
        {
            get
            {
                return b;
            }
        }

        public IFigure Clone()
        {
            return new Edge(a.Clone() as Point3d, b.Clone() as Point3d);
        }

        public void Draw(Graphics g, Color c)
        {
            Pen p = new Pen(c);
            g.DrawLine(p, a.To2d(), b.To2d());
            p.Dispose();
        }

        private Point3d a, b;
    }

    class Face : IFigure
    {
        protected Face()
        {
            /*EMPTY*/
        }
        
        protected Edge[] edges;

        public Edge[] Edges
        {
            get
            {
                return edges;
            }
        }

        public static Face CreateTriangle(Point3d a, Point3d b, Point3d c)
        {
            Face Triangle = new Face();
            Triangle.edges = new Edge[3];
            
            Triangle.edges[0] = new Edge(a, b);
            Triangle.edges[1] = new Edge(b, c);
            Triangle.edges[2] = new Edge(c, a);

            return Triangle;
        }

        public static Face CreateSquare(Point3d a, Point3d b, Point3d c, Point3d d)
        {
            Face Square = new Face();
            Square.edges = new Edge[4];
            
            Square.edges[0] = new Edge(a, b);
            Square.edges[1] = new Edge(b, c);
            Square.edges[2] = new Edge(c, d);
            Square.edges[3] = new Edge(d, a);

            return Square;
        }
        
        public static Face CreatePentagon(Point3d a, Point3d b, Point3d c, Point3d d, Point3d e)
        {
            Face Pentagon = new Face();
            Pentagon.edges = new Edge[5];

            Pentagon.edges[0] = new Edge(a, b);
            Pentagon.edges[1] = new Edge(b, c);
            Pentagon.edges[2] = new Edge(c, d);
            Pentagon.edges[3] = new Edge(d, e);
            Pentagon.edges[4] = new Edge(e, a);

            return Pentagon;
        }

        public IFigure Clone()
        {
            Face res = new Face();
            res.edges = new Edge[edges.Length];
            for (int i = 0; i < edges.Length; ++i)
            {
                res.edges[i] = edges[i].Clone() as Edge;
            }
            return res;
        }

        public void Draw(Graphics g, Color c)
        {
            foreach (var x in Edges)
            {
                x.Draw(g, c);
            }
        }
    }

    class Polyhedron : IFigure
    {
        protected Polyhedron()
        {
            /* EMPTY */
        }
        
        protected Face[] faces;

        protected Point3d center;

        public Face[] Faces
        {
            get 
            {
                return faces;
            }    
        }

        public Point3d Center
        {
            get
            {
                return center;
            }
        }

        public static Polyhedron CreateTetrahedron(Point3d a, Point3d b, Point3d q)
        {
            Point3d c = GetThirdVertexForTriangle(a, b, q);
            Vector m = new Vector(a, b);
            Vector n = new Vector(a, c);
            double sideLen = m.Norm();
            double hLen = sideLen * Math.Sqrt(6) / 3.0;
            Vector h = m[n].Normalize() * hLen;
            Vector r = h + ((m + n) * (1.0 / 3.0));
            Point3d d = new Point3d(a.X + r.X, a.Y + r.Y, a.Z + r.Z);

            Polyhedron Tetrahedron = new Polyhedron();
            Tetrahedron.faces = new Face[4];
            Tetrahedron.faces[0] = Face.CreateTriangle(a, b, c);
            Tetrahedron.faces[1] = Face.CreateTriangle(a, b, d);
            Tetrahedron.faces[2] = Face.CreateTriangle(a, c, d);
            Tetrahedron.faces[3] = Face.CreateTriangle(b, c, d);

            double cx = (a.X + b.X + c.X + d.X) / 4;
            double cy = (a.Y + b.Y + c.Y + d.Y) / 4;
            double cz = (a.Z + b.Z + c.Z + d.Z) / 4;
            Tetrahedron.center = new Point3d(cx, cy, cz);

            return Tetrahedron;
        }

        public static Polyhedron CreateHexahedron(Point3d a, Point3d b, Point3d q)
        {
            Point3d[] cd = GetVertecesForSquare(a, b, q);
            Point3d c = cd[0];
            Point3d d = cd[1];
            Vector m = new Vector(a, b);
            Vector n = new Vector(a, c);
            double sideLen = m.Norm();            
            Vector h = m[n].Normalize() * sideLen;
            Point3d a1 = new Point3d(a.X + h.X, a.Y + h.Y, a.Z + h.Z);
            Point3d b1 = new Point3d(b.X + h.X, b.Y + h.Y, b.Z + h.Z);
            Point3d c1 = new Point3d(c.X + h.X, c.Y + h.Y, c.Z + h.Z);
            Point3d d1 = new Point3d(d.X + h.X, d.Y + h.Y, d.Z + h.Z);

            Polyhedron Hexahedron = new Polyhedron();
            Hexahedron.faces = new Face[6];
            Hexahedron.faces[0] = Face.CreateSquare(a, b, c, d);
            Hexahedron.faces[1] = Face.CreateSquare(a, b, b1, a1);
            Hexahedron.faces[2] = Face.CreateSquare(b, c, c1, b1);
            Hexahedron.faces[3] = Face.CreateSquare(c, d, d1, c1);
            Hexahedron.faces[4] = Face.CreateSquare(a, d, d1, a1);
            Hexahedron.faces[5] = Face.CreateSquare(a1, b1, c1, d1);

            double cx = (a.X + c1.X) / 2;
            double cy = (a.Y + c1.Y) / 2;
            double cz = (a.Z + c1.Z) / 2;
            Hexahedron.center = new Point3d(cx, cy, cz);

            return Hexahedron;
        }

        public static Polyhedron CreateOktahedron(Point3d a, Point3d b, Point3d q)
        {
            Point3d[] cd = GetVertecesForSquare(a, b, q);
            Point3d c = cd[0];
            Point3d d = cd[1];
            Vector m = new Vector(a, b);
            Vector n = new Vector(a, d);
            double sideLen = m.Norm();
            double hLen = sideLen / Math.Sqrt(2);
            Vector h = m[n].Normalize() * hLen;
            Vector r = h + ((m + n) * 0.5);
            Point3d top = new Point3d(a.X + r.X, a.Y + r.Y, a.Z + r.Z);
            Point3d bottom = new Point3d(c.X - r.X, c.Y - r.Y, c.Z - r.Z);
            
            Polyhedron Oktahedron = new Polyhedron();
            Oktahedron.faces = new Face[8];
            Oktahedron.faces[0] = Face.CreateTriangle(a, b, top);
            Oktahedron.faces[1] = Face.CreateTriangle(b, c, top);
            Oktahedron.faces[2] = Face.CreateTriangle(c, d, top);
            Oktahedron.faces[3] = Face.CreateTriangle(d, a, top);
            Oktahedron.faces[4] = Face.CreateTriangle(a, b, bottom);
            Oktahedron.faces[5] = Face.CreateTriangle(b, c, bottom);
            Oktahedron.faces[6] = Face.CreateTriangle(c, d, bottom);
            Oktahedron.faces[7] = Face.CreateTriangle(d, a, bottom);

            double cx = (a.X + c.X) / 2;
            double cy = (a.Y + c.Y) / 2;
            double cz = (a.Z + c.Z) / 2;
            Oktahedron.center = new Point3d(cx, cy, cz);

            return Oktahedron;
        }

        public static Polyhedron CreateIcosahedron()
        {
            Polyhedron Icosahedron = new Polyhedron();
            Point3d[] verts = new Point3d[12];

            // верх и низ
            verts[0] = new Point3d(0, 0, 1);
            verts[1] = new Point3d(0, 0, -1);

            double h = Math.Sqrt(5) / 5;
            double R = h / Math.Sin(Math.PI / 5);

            // верхний круг
            double angle = 0;
            for (int i = 2; i < 7; ++i)
            {
                verts[i] = new Point3d(R * Math.Sin(angle), R * Math.Cos(angle), h);
                angle += 72 * Math.PI / 180;
            }

            // нижний круг
            angle = 36 * Math.PI / 180;
            for (int i = 7; i < 12; ++i)
            {
                verts[i] = new Point3d(R * Math.Sin(angle), R * Math.Cos(angle), -h);
                angle += 72 * Math.PI / 180;
            }

            Icosahedron.faces = new Face[20];
            for (int i = 0; i < 4; ++i)
            {
                Icosahedron.faces[2 * i] = Face.CreateTriangle(verts[0], verts[2 + i], verts[3 + i]);
                Icosahedron.faces[2 * i + 1] = Face.CreateTriangle(verts[1], verts[7 + i], verts[8 + i]);
            }
            Icosahedron.faces[8] = Face.CreateTriangle(verts[0], verts[2], verts[6]);
            Icosahedron.faces[9] = Face.CreateTriangle(verts[1], verts[7], verts[11]);

            for (int i = 0; i < 4; ++i)
            {
                Icosahedron.faces[10 + 2 * i] = Face.CreateTriangle(verts[2 + i], verts[3 + i], verts[7 + i]);
                Icosahedron.faces[11 + 2 * i] = Face.CreateTriangle(verts[7 + i], verts[8 + i], verts[3 + i]);
            }
            Icosahedron.faces[18] = Face.CreateTriangle(verts[6], verts[2], verts[11]);
            Icosahedron.faces[19] = Face.CreateTriangle(verts[11], verts[7], verts[2]);

            Icosahedron.center = new Point3d(0, 0, 0);
            return Icosahedron;
        }

        public static Polyhedron CreateDodecahedron()
        {
            Polyhedron Icosahedron = CreateIcosahedron();
            Point3d[] verts = new Point3d[20];
            for (int i = 0; i < 20; ++i)
            {
                double x = (Icosahedron.faces[i].Edges[0].First.X + Icosahedron.faces[i].Edges[1].First.X + Icosahedron.faces[i].Edges[2].First.X) / 3;
                double y = (Icosahedron.faces[i].Edges[0].First.Y + Icosahedron.faces[i].Edges[1].First.Y + Icosahedron.faces[i].Edges[2].First.Y) / 3;
                double z = (Icosahedron.faces[i].Edges[0].First.Z + Icosahedron.faces[i].Edges[1].First.Z + Icosahedron.faces[i].Edges[2].First.Z) / 3;
                verts[i] = new Point3d(x, y, z);
            }

            Polyhedron Dodecahedron = new Polyhedron();
            Dodecahedron.faces = new Face[12];
            Dodecahedron.faces[0] = Face.CreatePentagon(verts[0], verts[2], verts[4], verts[6], verts[8]);
            Dodecahedron.faces[1] = Face.CreatePentagon(verts[1], verts[3], verts[5], verts[7], verts[9]);

            for (int i = 0; i < 4; ++i)
            {
                Dodecahedron.faces[2 + 2 * i] = Face.CreatePentagon(verts[2 * i + 2], verts[2 * i], verts[10 + 2 * i], verts[11 + 2 * i], verts[12 + 2 * i]);
                Dodecahedron.faces[3 + 2 * i] = Face.CreatePentagon(verts[2 * i + 3], verts[2 * i + 1], verts[11 + 2 * i], verts[12 + 2 * i], verts[13 + 2 * i]);
            }

            Dodecahedron.faces[10] = Face.CreatePentagon(verts[0], verts[8], verts[18], verts[19], verts[10]);
            Dodecahedron.faces[11] = Face.CreatePentagon(verts[9], verts[1], verts[11], verts[10], verts[19]);

            Dodecahedron.center = new Point3d(0, 0, 0);

            return Dodecahedron;
        }
        
        private static Point3d GetThirdVertexForTriangle(Point3d a, Point3d b, Point3d q)
        {
            Vector m = new Vector(a, b);
            Vector n = new Vector(a, q);
            double sideLen = m.Norm();
            double hLen = sideLen * Math.Sqrt(3) / 2.0;

            double coeff = (m * n) / (m * m);
            Vector h = (n - (coeff * m)).Normalize() * hLen;
            Vector r = (0.5 * m) + h;
            return new Point3d(a.X + r.X, a.Y + r.Y, a.Z + r.Z);
        }

        private static Point3d[] GetVertecesForSquare(Point3d a, Point3d b, Point3d q)
        {
            Vector m = new Vector(a, b);
            Vector n = new Vector(a, q);
            double sideLen = m.Norm();

            double coeff = (m * n) / (m * m);
            Vector h = (n - (coeff * m)).Normalize() * sideLen;
            Vector r = m + h;
            Point3d c = new Point3d(a.X + r.X, a.Y + r.Y, a.Z + r.Z);
            Point3d d = new Point3d(a.X + h.X, a.Y + h.Y, a.Z + h.Z);
            return new Point3d[2] { c, d };
        }

        public IFigure Clone()
        {
            Polyhedron res = new Polyhedron();
            res.faces = new Face[faces.Length];
            for (int i = 0; i < faces.Length; ++i)
            {
                res.faces[i] = faces[i].Clone() as Face;
            }
            res.center = center.Clone() as Point3d;
            return res;
        }

        public void Draw(Graphics g, Color c)
        {
            foreach (var x in Faces)
            {
                x.Draw(g, c);
            }
        }
    }

    class Vector
    {
        public Vector()
        {
            /*empty*/
        }

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public Vector (Point3d a)
        {
            this.x = a.X;
            this.y = a.Y;
            this.z = a.Z;
        }

        public Vector(Point3d a, Point3d b)
        {
            this.x = b.X - a.X;
            this.y = b.Y - a.Y;
            this.z = b.Z - a.Z;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector operator -(Vector a)
        {
            return new Vector(-a.x, -a.y, -a.z);
        }

        public static Vector operator *(double a, Vector b)
        {
            return new Vector(a * b.x, a * b.y, a * b.z);
        }

        public static Vector operator *(Vector a, double b)
        {
            return new Vector(a.x * b, a.y * b, a.z * b);
        }

        public static double operator *(Vector a, Vector b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public double Norm()
        {
            return Math.Sqrt(this * this);
        }
        
        public Vector Normalize()
        {
            return this * (1.0 / this.Norm());
        }

        public Vector this[Vector a]
        {
            get
            {
                return new Vector(this.y * a.z - a.y * this.z, this.z * a.x - a.z * this.x, this.x * a.y - a.x * this.y);
            }
        }

        private double x, y, z;

        public double X
        {
            get
            {
                return x;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
        }

        public double Z
        {
            get
            {
                return z;
            }
        }
    }
}
