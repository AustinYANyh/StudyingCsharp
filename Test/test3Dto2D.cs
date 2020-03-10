using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test3Dto2D
{
    public partial class Form1 : Form
    {
        public struct POINT3D
        {
            public double x;
            public double y;
            public double z;
        };

        public struct POINT2D
        {
            public double x;
            public double y;
        };

        public int needXReverxe = 1;
        public int needYReverxe = 1;
        public int needZReverxe = 1;

        public double[] mixtrl = new double[9];
        POINT3D[] pointX = new POINT3D[1];
        POINT3D[] pointY = new POINT3D[1];
        POINT3D[] pointZ = new POINT3D[1];
        public Point orginPoint = new Point(400, 200);

        public Form1()
        {
            InitializeComponent();
        }

        public POINT2D[] Transform3DTo2D(POINT3D[] pt3d)
        {
            POINT2D[] result = new POINT2D[1];

            double x = pt3d[0].x;
            double y = pt3d[0].y;
            double z = pt3d[0].z;

            double xTOx = (mixtrl[0] * x + mixtrl[1] * y + mixtrl[2] * z) * needXReverxe;
            double yTox = (mixtrl[3] * x + mixtrl[4] * y + mixtrl[5] * z) * needYReverxe;
            double zTox = (mixtrl[6] * x + mixtrl[7] * y + mixtrl[8] * z) * needZReverxe;

            double xx = -1 * Math.Cos(150 * 0.017453) * xTOx - Math.Cos(150 * 0.017453) * yTox;
            double yy = Math.Sin(150 * 0.017453) * xTOx - Math.Sin(150 * 0.017453) * yTox - zTox;

            result[0].x = orginPoint.X + xx;
            result[0].y = orginPoint.Y + yy;

            return result;
        }

        public POINT3D[] TransRoll(POINT3D[] pt3d,int RollFlag)
        {
            POINT3D[] result = new POINT3D[1];

            double x = pt3d[0].x;
            double y = pt3d[0].y;
            double z = pt3d[0].z;

            double cos = Math.Cos(20 * (Math.PI / 180));
            double sin = Math.Sin(20 * (Math.PI / 180));

            //绕x轴
            if (RollFlag == 0)
            {
                double x1 = x;
                double y1 = y * cos - z * sin;
                double z1 = z * cos + y * sin;

                result[0].x = x1;
                result[0].y = y1;
                result[0].z = z1;
            }
            else if(RollFlag == 1)
            {
                double x1 = x * cos - z * sin;
                double y1 = y;
                double z1 = z * cos + x * sin;

                result[0].x = x1;
                result[0].y = y1;
                result[0].z = z1;
            }
            else
            {
                double x1 = x * cos - y * sin;
                double y1 = y * cos + x * sin;
                double z1 = z;

                result[0].x = x1;
                result[0].y = y1;
                result[0].z = z1;
            }
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            needYReverxe = -1 * needYReverxe;
            paintXYZ(pointX, pointY, pointZ);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            needXReverxe = -1 * needXReverxe;
            paintXYZ(pointX, pointY, pointZ);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pointX = TransRoll(pointX, 2);
            pointY = TransRoll(pointY, 2);

            paintXYZ(pointX, pointY, pointZ);
        }

        public void paintXYZ(POINT3D[] pointX, POINT3D[] pointY, POINT3D[] pointZ)
        {
            Graphics graphics = this.CreateGraphics();
            graphics.Clear(this.BackColor);
            Font font = new Font("宋体", 10, FontStyle.Regular);

            POINT2D[] pointx = new POINT2D[1];
            POINT2D[] pointy = new POINT2D[1];
            POINT2D[] pointz = new POINT2D[1];

            pointx = Transform3DTo2D(pointX);

            graphics.DrawLine(new Pen(Color.Blue, 1), orginPoint,
                new Point(Convert.ToInt32(pointx[0].x), Convert.ToInt32(pointx[0].y)));

            graphics.DrawString("+X", font, new SolidBrush(Color.Blue),
                new Point(Convert.ToInt32(pointx[0].x) - 20, Convert.ToInt32(pointx[0].y)));


            pointy = Transform3DTo2D(pointY);

            graphics.DrawLine(new Pen(Color.Red, 1), orginPoint,
                new Point(Convert.ToInt32(pointy[0].x), Convert.ToInt32(pointy[0].y)));

            graphics.DrawString("+Y", font, new SolidBrush(Color.Red),
                new Point(Convert.ToInt32(pointy[0].x) - 20, Convert.ToInt32(pointy[0].y) - 20));

            pointz = Transform3DTo2D(pointZ);

            graphics.DrawLine(new Pen(Color.Green, 1), orginPoint,
                new Point(Convert.ToInt32(pointz[0].x), Convert.ToInt32(pointz[0].y)));

            graphics.DrawString("+Z", font, new SolidBrush(Color.Green),
                new Point(Convert.ToInt32(pointz[0].x), Convert.ToInt32(pointz[0].y) - 20));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mixtrl[0] = 1;
            mixtrl[1] = 0;
            mixtrl[2] = 0;
            mixtrl[3] = 0;
            mixtrl[4] = 1;
            mixtrl[5] = 0;
            mixtrl[6] = 0;
            mixtrl[7] = 0;
            mixtrl[8] = 1;

            initVar();

            paintXYZ(pointX, pointY, pointZ);
        }

        void initVar()
        {
            pointX[0].x = 120;
            pointX[0].y = 0;
            pointX[0].z = 0;

            pointY[0].x = 0;
            pointY[0].y = 120;
            pointY[0].z = 0;

            pointZ[0].x = 0;
            pointZ[0].y = 0;
            pointZ[0].z = 120;
        }
    }
}
