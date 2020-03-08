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
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace testRotate
{
    public struct POINT3D
    {
        public double x;
        public double y;
        public double z;
        public double a;
        public double b;
        public double c;
        public int GCode;
    };

    public partial class Form1 : Form
    {
        public enum ShowMode { Mode3D = 0, ModeXY, ModeXZ, ModeYZ, ModeG17, ModeG18, ModeG19 };
        public int ROLL_X = 0;
        public int ROLL_Y = 1;
        public int ROLL_Z = 2;
       
        public bool m_flagSave;
        public int m_clearPoint;
        public bool m_flagClear;
        public bool m_flagRedrawDone;
        POINT3D m_originPoint3D = new POINT3D();
        public bool m_flagInitDone;
        public Point m_lastBackPoint;
        public double m_coneBackHeight;
        public double m_coneBackWidth;
        public Bitmap m_coneBackBitmap;
        public Bitmap m_coneBackDC;
        public int m_axisXInver;
        public int m_axisYInver;
        public int m_axisZInver;
        public bool m_flagXInver;
        public bool m_flagYInver;
        public bool m_flagZInver;
        public int m_curShowMode;
        public bool m_flagModel;
        public Color m_drawColor;
        public int m_flagState;
        public int m_drawRate;
        public ShowMode m_showMode;
        public bool m_flagRedraw;
        public int m_curPoint;
        public int m_NCDataNum;
        unsafe POINT3D[] m_pNCRouteData;
        POINT3D m_NCLastData = new POINT3D();
        public string m_filePath;
        public double m_rad;
        public float m_xyangle;
        public Point m_originPoint;
        public double m_nextX;
        public double m_nextY;
        public double m_lastX;
        public double m_lastY;
        public double m_3dxyangle;
        public double m_xycosangle;
        public double m_xysinangle;
        public double m_unit;
        public double[] m_arrChangeTranpose = new double[9];
        public double[] m_arrChange = new double[9];
        public double[] m_arrVector = new double[9];
        public double[] m_arrRollZ = new double[9];
        public string m_xVal;
        public string m_yVal;
        public string m_zVal;
        public string m_aVal;
        public string m_bVal;
        public string m_cVal;
        //CMutex m_mutex;
        Thread m_pThread;
        public Point moveto = new Point();

        public Form1()
        {
            InitializeComponent();
    }

        public void initVar()
        {
            m_originPoint.X = 300;
            m_originPoint.Y = 300;
            m_xyangle = 150;
            m_rad = 0.017453;
            m_3dxyangle = m_xyangle * m_rad;
            m_xycosangle = Math.Cos(m_3dxyangle);
            m_xysinangle = Math.Sin(m_3dxyangle);
            m_unit = 2;             //空间坐标的单位

            for (int i = 0; i < 9; i++)
            {
                m_arrChangeTranpose[i] = 0;
                m_arrChange[i] = 0;
                m_arrVector[i] = 0;
                m_arrRollZ[i] = 0;
            }
            m_arrVector[0] = 1.0;
            m_arrVector[4] = 1.0;
            m_arrVector[8] = 1.0;

            m_pNCRouteData = null;
            m_NCDataNum = 0;

            m_NCLastData.x = 0;
            m_NCLastData.y = 0;
            m_NCLastData.z = 0;
            m_NCLastData.a = 0;
            m_NCLastData.b = 0;
            m_NCLastData.c = 0;
            m_NCLastData.GCode = 0;

            m_nextX = m_originPoint.X;
            m_nextY = m_originPoint.Y;
            m_lastX = m_originPoint.X;
            m_lastY = m_originPoint.Y;

            m_originPoint3D.x = 0.0;
            m_originPoint3D.y = 0.0;
            m_originPoint3D.z = 0.0;
            m_originPoint3D.a = 0.0;
            m_originPoint3D.b = 0.0;
            m_originPoint3D.c = 0.0;

            m_xVal = "0.000000";
            m_yVal = "0.000000";
            m_zVal = "0.000000";
            m_aVal = "0.000000";
            m_bVal = "0.000000";
            m_cVal = "0.000000";

            m_curPoint = -1;
            m_pThread = null;
            m_flagRedraw = false;
            m_showMode = ShowMode.Mode3D;
            m_drawRate = 20;
            m_flagState = -1;   //状态标志位 -1为未开始 0为移动 1为停止 2为完成
            m_flagModel = false;
            m_flagXInver = false;
            m_flagYInver = false;
            m_flagZInver = false;
            m_axisXInver = 1;       //观察坐标系的轴方向
            m_axisYInver = 1;
            m_axisZInver = 1;

            m_coneBackWidth = 0;
            m_coneBackHeight = 0;

            m_flagInitDone = false;
            m_flagClear = false;

            m_clearPoint = 0;

            m_flagSave = true;

            Bitmap bitmap = new Bitmap(panel1.Width, panel1.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(panel1.BackColor);

            m_coneBackDC = bitmap;
            //m_coneBackBitmap.CreateCompatibleBitmap(GetDC(), 300, 300);
            //m_coneBackDC.SelectObject(&m_coneBackBitmap);

            //m_pThread = AfxBeginThread(DrawThread, (LPVOID)this);
            m_pThread = new Thread(DrawThread);
            m_pThread.Start();
        }

        public void DrawAxis()
        {
            double x = 60 * m_unit, y = 0, z = 0;   //绝对坐标系
            double xTox = 0, yTox = 0, zTox = 0;
            double xx = 0, yy = 0;
            switch (m_showMode)
            {
                case ShowMode.ModeG17:
                    {
                        DrawBoundary();
                        Draw3D();
                        break;
                    }
                case ShowMode.ModeG18:
                    {
                        DrawBoundary();
                        Draw3D();
                        break;
                    }
                case ShowMode.ModeG19:
                    {
                        DrawBoundary();
                        Draw3D();
                        break;
                    }
                case ShowMode.Mode3D: //MODE_3D
                    {
                        Draw3D();
                        break;
                    }
                case ShowMode.ModeXY: //MODE_XY
                    {
                        DrawAxis2D(60, 60);
                        break;
                    }
                case ShowMode.ModeXZ: //MODE_XZ
                    {
                        DrawAxis2D(60, 60);
                        break;
                    }
                case ShowMode.ModeYZ: //MODE_YZ
                    {
                        DrawAxis2D(60, 60);
                        break;
                    }
            }
        }

        public void DrawBoundary()
        {
            double x1 = 0, y1 = 0, z1 = 0, x2 = 0, y2 = 0, z2 = 0;
            double xx1 = 0, yy1 = 0, xx2 = 0, yy2 = 0;
            double xTox1 = 0, yTox1 = 0, zTox1 = 0;
            double xTox2 = 0, yTox2 = 0, zTox2 = 0;
            double width = 80 * m_unit, height = 80 * m_unit, zLen = 80 * m_unit;

            Graphics pDC = panel1.CreateGraphics();
            Pen newPen = new Pen(Color.Orange,1);
            newPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            if (ShowMode.ModeG17 == m_showMode)  //XOY
            {
                x1 = width;
                y1 = height;
                x2 = -1 * x1;
                y2 = height;
            }
            else if (ShowMode.ModeG18 == m_showMode)     //XOZ
            {
                x1 = width;
                z1 = height;
                x2 = -1 * x1;
                z2 = height;
            }
            else if (ShowMode.ModeG19 == m_showMode)     //YOZ
            {
                y1 = width;
                z1 = height;
                y2 = -1 * y1;
                z2 = height;
            }

            xTox1 = (m_arrVector[0] * x1 + m_arrVector[1] * y1 + m_arrVector[2] * z1);
            yTox1 = (m_arrVector[3] * x1 + m_arrVector[4] * y1 + m_arrVector[5] * z1);
            zTox1 = (m_arrVector[6] * x1 + m_arrVector[7] * y1 + m_arrVector[8] * z1);
            xTox2 = (m_arrVector[0] * x2 + m_arrVector[1] * y2 + m_arrVector[2] * z2);
            yTox2 = (m_arrVector[3] * x2 + m_arrVector[4] * y2 + m_arrVector[5] * z2);
            zTox2 = (m_arrVector[6] * x2 + m_arrVector[7] * y2 + m_arrVector[8] * z2);

            xx1 = -1 * xTox1 * m_xycosangle - yTox1 * m_xycosangle;
            yy1 = xTox1 * m_xysinangle - yTox1 * m_xysinangle - zTox1;
            xx2 = -1 * xTox2 * m_xycosangle - yTox2 * m_xycosangle;
            yy2 = xTox2 * m_xysinangle - yTox2 * m_xysinangle - zTox2;

            Point p1 = new Point((int)(m_originPoint.X + xx1), (int)(m_originPoint.Y + yy1));
            Point p2 = new Point((int)(m_originPoint.X + xx2), (int)(m_originPoint.Y + yy2));

            pDC.DrawLine(newPen, p1, p2);

            if (ShowMode.ModeG17 == m_showMode)
                y2 = -1 * y2;
            else if (ShowMode.ModeG18 == m_showMode || ShowMode.ModeG19 == m_showMode)
                z2 = -1 * z2;
            xTox2 = (m_arrVector[0] * x2 + m_arrVector[1] * y2 + m_arrVector[2] * z2);
            xTox2 = (m_arrVector[0] * x2 + m_arrVector[1] * y2 + m_arrVector[2] * z2);
            yTox2 = (m_arrVector[3] * x2 + m_arrVector[4] * y2 + m_arrVector[5] * z2);
            zTox2 = (m_arrVector[6] * x2 + m_arrVector[7] * y2 + m_arrVector[8] * z2);
            xx2 = -1 * xTox2 * m_xycosangle - yTox2 * m_xycosangle;
            yy2 = xTox2 * m_xysinangle - yTox2 * m_xysinangle - zTox2;

            Point p3 = new Point((int)(m_originPoint.X + xx2), (int)(m_originPoint.Y + yy2));
            pDC.DrawLine(newPen, p2, p3);

            if (ShowMode.ModeG17 == m_showMode || ShowMode.ModeG18 == m_showMode)
                x2 = -1 * x2;
            else if (ShowMode.ModeG19 == m_showMode)
                y2 = -1 * y2;
            xTox2 = (m_arrVector[0] * x2 + m_arrVector[2] * y2 + m_arrVector[2] * z2);
            yTox2 = (m_arrVector[3] * x2 + m_arrVector[4] * y2 + m_arrVector[5] * z2);
            zTox2 = (m_arrVector[6] * x2 + m_arrVector[7] * y2 + m_arrVector[8] * z2);
            xx2 = -1 * xTox2 * m_xycosangle - yTox2 * m_xycosangle;
            yy2 = xTox2 * m_xysinangle - yTox2 * m_xysinangle - zTox2;
            Point p4 = new Point((int)(m_originPoint.X + xx2), (int)(m_originPoint.Y + yy2));
            pDC.DrawLine(newPen, p3, p4);

            if (ShowMode.ModeG18 == m_showMode || ShowMode.ModeG19 == m_showMode)
                z2 = -1 * z2;
            else if (ShowMode.ModeG17 == m_showMode)
                y2 = -1 * y2;
            xTox2 = (m_arrVector[0] * x2 + m_arrVector[1] * y2 + m_arrVector[2] * z2);
            yTox2 = (m_arrVector[3] * x2 + m_arrVector[4] * y2 + m_arrVector[5] * z2);
            zTox2 = (m_arrVector[6] * x2 + m_arrVector[7] * y2 + m_arrVector[8] * z2);
            xx2 = -1 * xTox2 * m_xycosangle - yTox2 * m_xycosangle;
            yy2 = xTox2 * m_xysinangle - yTox2 * m_xysinangle - zTox2;
            p1 = new Point((int)(m_originPoint.X + xx2), (int)(m_originPoint.Y + yy2));
            pDC.DrawLine(newPen, p4, p1);

            pDC.Dispose();
        }

        public void Draw3D()
        {
            POINT3D point = new POINT3D();
            Color oldColor = m_drawColor;
            point.x = 60;
            point.y = 0;
            point.z = 0;
            m_drawColor = Color.Blue;
            DrawAxis3D(point);
            m_drawColor = oldColor;

            oldColor = m_drawColor;
            point.x = 0;
            point.y = 60;
            point.z = 0;
            m_drawColor = Color.Red;
            DrawAxis3D(point);
            m_drawColor = oldColor;

            oldColor = m_drawColor;
            point.x = 0;
            point.y = 0;
            point.z = 60;
            m_drawColor = Color.Green;
            DrawAxis3D(point);
            m_drawColor = oldColor;
        }

        public void DrawAxis2D(long ix,long iy)
        {
            double x = 0, y = 0;
            Color oldColor;
            oldColor = m_drawColor;
            if (ShowMode.ModeXY == m_showMode || ShowMode.ModeXZ == m_showMode)
            {
                x = ix * m_unit * m_axisXInver;
                m_drawColor = Color.FromArgb(0, 0, 255);

                if (ShowMode.ModeXY == m_showMode)
                    y = iy * m_unit * m_axisYInver;
                else
                    y = iy * m_unit * m_axisZInver;
            }
            else if (ShowMode.ModeYZ == m_showMode)
            {
                m_drawColor = Color.FromArgb(255, 0, 0);
                x = ix * m_unit * m_axisYInver;
                y = iy * m_unit * m_axisZInver;
            }

            Graphics pDC = panel1.CreateGraphics();

            Pen oldPen = new Pen(m_drawColor, 1);

            pDC.Clear(this.BackColor);

            pDC.DrawLine(oldPen, m_originPoint, new Point((int)(m_originPoint.X + x), m_originPoint.Y));
            Font font = new Font("宋体", 10, FontStyle.Regular);
            Brush brush = new SolidBrush(m_drawColor);

            if (ShowMode.ModeXY == m_showMode || ShowMode.ModeXZ == m_showMode)
            {
                brush = new SolidBrush(Color.Blue);
                pDC.DrawString("+X", font, brush, new Point((int)(m_originPoint.X + x + 20), (int)(m_originPoint.Y)));
            }
            else if (ShowMode.ModeYZ == m_showMode)
            {
                brush = new SolidBrush(Color.Red);
                pDC.DrawString("+Y", font, brush, new Point((int)(m_originPoint.X + x + 20), (int)(m_originPoint.Y)));
            }
            
            if (ShowMode.ModeXY == m_showMode)
                m_drawColor = Color.FromArgb(255, 0, 0);
            else if (ShowMode.ModeXZ == m_showMode || ShowMode.ModeYZ == m_showMode)
                m_drawColor = Color.FromArgb(0, 255, 0);

            Pen newPeny = new Pen(m_drawColor,1);
            pDC.DrawLine(newPeny, m_originPoint, new Point((int)m_originPoint.X, (int)(m_originPoint.Y - y)));

            if (ShowMode.ModeXY == m_showMode)
            {
                brush = new SolidBrush(Color.Red);
                pDC.DrawString("+Y", font, brush, new Point((int)(m_originPoint.X), (int)(m_originPoint.Y - y - 20)));
            }
            else if (ShowMode.ModeXZ == m_showMode || ShowMode.ModeYZ == m_showMode)
            {
                brush = new SolidBrush(Color.Green);
                pDC.DrawString("+Z", font, brush, new Point((int)(m_originPoint.X), (int)(m_originPoint.Y - y - 20)));
            }

            m_drawColor = oldColor;
        }

        public void DrawAxis3D(POINT3D point)
        {
            int flagAxis = -1;//轴标志 0为x轴 1为y轴 2为z轴
            double x = point.x * m_unit, y = point.y * m_unit, z = point.z * m_unit;    //绝对坐标系
            double xTox = 0, yTox = 0, zTox = 0;
            double xx = 0, yy = 0;
            if (0.0 != point.x)
                flagAxis = 0;
            else if (0.0 != point.y)
                flagAxis = 1;
            else if (0.0 != point.z)
                flagAxis = 2;

            xTox = (m_arrVector[0] * x + m_arrVector[1] * y + m_arrVector[2] * z) * m_axisXInver;
            yTox = (m_arrVector[3] * x + m_arrVector[4] * y + m_arrVector[5] * z) * m_axisYInver;
            zTox = (m_arrVector[6] * x + m_arrVector[7] * y + m_arrVector[8] * z) * m_axisZInver;

            xx = -1 * xTox * m_xycosangle - yTox * m_xycosangle;
            yy = xTox * m_xysinangle - yTox * m_xysinangle - zTox;

            Graphics pDC = panel1.CreateGraphics();

            Pen newPen = new Pen(m_drawColor, 1);

            pDC.DrawLine(newPen, m_originPoint, new Point((int)(m_originPoint.X + xx), (int)(m_originPoint.Y + yy)));

            if (0 == flagAxis)
                x += 20;
            else if (1 == flagAxis)
                y += 20;
            else if (2 == flagAxis)
                z += 20;
            xTox = (m_arrVector[0] * x + m_arrVector[1] * y + m_arrVector[2] * z) * m_axisXInver;
            yTox = (m_arrVector[3] * x + m_arrVector[4] * y + m_arrVector[5] * z) * m_axisYInver;
            zTox = (m_arrVector[6] * x + m_arrVector[7] * y + m_arrVector[8] * z) * m_axisZInver;
            xx = -1 * xTox * m_xycosangle - yTox * m_xycosangle;
            yy = xTox * m_xysinangle - yTox * m_xysinangle - zTox;

            Font font = new Font("宋体", 10, FontStyle.Regular);
            if (0 == flagAxis)
            {
                Brush brush = new SolidBrush(Color.Blue);
                pDC.DrawString("+X", font, brush, new Point((int)(m_originPoint.X + xx), (int)(m_originPoint.Y + yy)));
            }
            else if (1 == flagAxis)
            {
                Brush brush = new SolidBrush(Color.Red);
                pDC.DrawString("+Y", font, brush, new Point((int)(m_originPoint.X + xx), (int)(m_originPoint.Y + yy)));
            }
            else if (2 == flagAxis)
            {
                Brush brush = new SolidBrush(Color.Green);
                pDC.DrawString("+Z", font, brush, new Point((int)(m_originPoint.X + xx), (int)(m_originPoint.Y + yy)));
            }

            pDC.Dispose();
        }

        public void RedrawRoute()
        {
            if (m_NCDataNum == 0)
            {
                return;
            }
            if (m_flagModel)
            {
                RedrawModel();
            }
            for (int i = m_clearPoint; i <= m_curPoint; i++)
            {
                CalNextPoint(i);
                if (i == m_clearPoint)
                {
                    m_lastX = m_nextX;
                    m_lastY = m_nextY;
                }
                DrawRoute(i);
                m_lastX = m_nextX;
                m_lastY = m_nextY;
            }
        }

        public void RedrawModel()
        {
            Color oldColor = m_drawColor;
            m_drawColor = Color.Blue;
            for (int j = 0; j < m_NCDataNum; j++)
            {
                //计算m_nextX、m_nextY
                CalNextPoint(j);
                DrawRoute(j);
                m_lastX = m_nextX;
                m_lastY = m_nextY;
            }
            m_drawColor = oldColor;
        }

        public void CalNextPoint(int nextPoint)
        {
            double x = 0, y = 0, z = 0; //绝对坐标系
            double xTox = 0, yTox = 0, zTox = 0;
            double xLen = m_pNCRouteData[nextPoint].x * m_unit, yLen = m_pNCRouteData[nextPoint].y * m_unit, zLen = m_pNCRouteData[nextPoint].z * m_unit;

            if (ShowMode.Mode3D == m_showMode)
            {
                x = m_pNCRouteData[nextPoint].x;
                y = m_pNCRouteData[nextPoint].y;
                z = m_pNCRouteData[nextPoint].z;
                xTox = (m_arrVector[0] * x + m_arrVector[1] * y + m_arrVector[2] * z) * m_axisXInver;   //Add direction
                yTox = (m_arrVector[3] * x + m_arrVector[4] * y + m_arrVector[5] * z) * m_axisYInver;
                zTox = (m_arrVector[6] * x + m_arrVector[7] * y + m_arrVector[8] * z) * m_axisZInver;
                m_nextX = m_originPoint.X + (-1 * xTox * m_xycosangle * m_unit - yTox * m_xycosangle * m_unit);
                m_nextY = m_originPoint.Y + (xTox * m_xysinangle * m_unit - yTox * m_xysinangle * m_unit - zTox * m_unit);
            }
            else if (ShowMode.ModeXY == m_showMode)
            {
                m_nextX = m_originPoint.X + m_pNCRouteData[nextPoint].x * m_unit * m_axisXInver;
                m_nextY = m_originPoint.Y + m_pNCRouteData[nextPoint].y * m_unit * m_axisYInver;
            }
            else if (ShowMode.ModeXZ == m_showMode)
            {
                m_nextX = m_originPoint.X + m_pNCRouteData[nextPoint].x * m_unit * m_axisXInver;
                m_nextY = m_originPoint.Y + m_pNCRouteData[nextPoint].z * m_unit * m_axisZInver;
            }
            else if (ShowMode.ModeYZ == m_showMode)
            {
                m_nextX = m_originPoint.X + m_pNCRouteData[nextPoint].y * m_unit * m_axisYInver;
                m_nextY = m_originPoint.Y + (-1 * m_pNCRouteData[nextPoint].z) * m_unit * m_axisZInver;
            }
            else if (ShowMode.ModeG17 == m_showMode) //XOY
            {
                x = xLen; y = yLen; z = 0;
                xTox = (m_arrVector[0] * x + m_arrVector[1] * y + m_arrVector[2] * z) * m_axisXInver;
                yTox = (m_arrVector[3] * x + m_arrVector[4] * y + m_arrVector[5] * z) * m_axisYInver;
                zTox = (m_arrVector[6] * x + m_arrVector[7] * y + m_arrVector[8] * z) * m_axisZInver;
                m_nextX = m_originPoint.X + (-1 * xTox * m_xycosangle - yTox * m_xycosangle);
                m_nextY = m_originPoint.Y + (xTox * m_xysinangle - yTox * m_xysinangle - zTox);
            }
            else if (ShowMode.ModeG18 == m_showMode)     //XOZ
            {
                x = xLen; y = 0; z = zLen;
                xTox = (m_arrVector[0] * x + m_arrVector[1] * y + m_arrVector[2] * z) * m_axisXInver;
                yTox = (m_arrVector[3] * x + m_arrVector[4] * y + m_arrVector[5] * z) * m_axisYInver;
                zTox = (m_arrVector[6] * x + m_arrVector[7] * y + m_arrVector[8] * z) * m_axisZInver;
                m_nextX = m_originPoint.X + (-1 * xTox * m_xycosangle - yTox * m_xycosangle);
                m_nextY = m_originPoint.Y + (xTox * m_xysinangle - yTox * m_xysinangle - zTox);
            }
            else if (ShowMode.ModeG19 == m_showMode)     //YOZ
            {
                x = 0; y = yLen; z = zLen;
                xTox = (m_arrVector[0] * x + m_arrVector[1] * y + m_arrVector[2] * z) * m_axisXInver;
                yTox = (m_arrVector[3] * x + m_arrVector[4] * y + m_arrVector[5] * z) * m_axisYInver;
                zTox = (m_arrVector[6] * x + m_arrVector[7] * y + m_arrVector[8] * z) * m_axisZInver;
                m_nextX = m_originPoint.X + (-1 * xTox * m_xycosangle - yTox * m_xycosangle);
                m_nextY = m_originPoint.Y + (xTox * m_xysinangle - yTox * m_xysinangle - zTox);
            }
        }

        public void DrawRoute(int point)
        {
            Graphics pDC = panel1.CreateGraphics();

            Pen newPen = new Pen(m_drawColor, 1);

            if (point != 0)
            {
                pDC.DrawLine(newPen, new Point((int)m_lastX, (int)m_lastY), new Point((int)m_nextX, (int)m_nextY));
            }
            else
            {
                moveto = new Point((int)m_nextX, (int)m_nextY);
            }
            pDC.Dispose();
        }

        public void DrawCone()
        {
            Pen cirPen = new Pen(Color.FromArgb(192, 192, 192), 2);
            Pen conePen = new Pen(Color.FromArgb(224, 224, 224), 1);

            Graphics pDC = panel1.CreateGraphics();
            Pen oldPen = conePen;
            float a = 0;
            float b = 0;
            float c = 0;
            if (m_NCDataNum != 0)
            {
                a = Convert.ToSingle(m_rad * m_pNCRouteData[m_curPoint].a);
                b = Convert.ToSingle(m_rad * m_pNCRouteData[m_curPoint].b);
                c = Convert.ToSingle(m_rad * m_pNCRouteData[m_curPoint].c);
            }
            float siny = (float)Math.Sin(b);
            float cosy = (float)Math.Cos(b);
            float sinx = (float)Math.Sin(a);
            float cosx = (float)Math.Cos(a);
            float sinz = (float)Math.Sin(c);
            float cosz = (float)Math.Cos(c);
            float xx1, yy1, zz1, xx2, yy2, zz2;

            int r = Convert.ToInt32(8 * m_unit);
            int h = Convert.ToInt32(24 * m_unit);

            int cx = 0;
            int cy = r;
            int cz = h;
            int aa = 2 * (1 - r);
            int vx = 0, vy = 0;
            int vx1 = 0, vy1 = 0;
            int vx2 = 0, vy2 = 0;
            int vx3 = 0, vy3 = 0;

            //Draw Cone
            while (cy >= 0)
            {
                if (m_showMode == ShowMode.Mode3D)//沿XYZ旋转 (x,-y,z)
                {
                    xx1 = Convert.ToSingle(cx * cosy * cosz + -cy * (sinx * siny * cosz - cosx * sinz) + cz * (cosx * siny * cosz + sinx * sinz));
                    yy1 = Convert.ToSingle(cx * cosy * sinz + -cy * (sinx * siny * sinz + cosx * cosz) + cz * (cosx * siny * sinz - sinx * cosz));
                    zz1 = Convert.ToSingle(cx * -siny + -cy * sinx * cosy + cz * cosx * cosy);

                    xx2 = Convert.ToSingle(m_arrVector[0] * xx1 + m_arrVector[1] * yy1 + m_arrVector[2] * zz1) * m_axisXInver;
                    yy2 = Convert.ToSingle(m_arrVector[3] * xx1 + m_arrVector[4] * yy1 + m_arrVector[5] * zz1) * m_axisYInver;
                    zz2 = Convert.ToSingle(m_arrVector[6] * xx1 + m_arrVector[7] * yy1 + m_arrVector[8] * zz1) * m_axisZInver;
                }
                else
                {
                    xx2 = Convert.ToSingle(cx); yy2 = (float)-cy; zz2 = Convert.ToSingle(cz);
                }

                vx = Convert.ToInt32((-xx2 * m_xycosangle) + (-yy2 * m_xycosangle) + m_nextX);
                vy = Convert.ToInt32(-((-xx2 * m_xysinangle) + (yy2 * m_xysinangle + zz2)) + m_nextY);

                if (m_showMode == ShowMode.Mode3D)//沿XYZ旋转 (x,y,z)
                {
                    xx1 = Convert.ToSingle(cx * cosy * cosz + cy * (sinx * siny * cosz - cosx * sinz) + cz * (cosx * siny * cosz + sinx * sinz));
                    yy1 = Convert.ToSingle(cx * cosy * sinz + cy * (sinx * siny * sinz + cosx * cosz) + cz * (cosx * siny * sinz - sinx * cosz));
                    zz1 = Convert.ToSingle(cx * -siny + cy * sinx * cosy + cz * cosx * cosy);
                    xx2 = Convert.ToSingle(m_arrVector[0] * xx1 + m_arrVector[1] * yy1 + m_arrVector[2] * zz1) * m_axisXInver;
                    yy2 = Convert.ToSingle(m_arrVector[3] * xx1 + m_arrVector[4] * yy1 + m_arrVector[5] * zz1) * m_axisYInver;
                    zz2 = Convert.ToSingle(m_arrVector[6] * xx1 + m_arrVector[7] * yy1 + m_arrVector[8] * zz1) * m_axisZInver;
                }
                else
                {
                    xx2 = Convert.ToSingle(cx); yy2 = Convert.ToSingle(cy); zz2 = Convert.ToSingle(cz);
                }

                vx1 = Convert.ToInt32((-xx2 * m_xycosangle) + (-yy2 * m_xycosangle) + m_nextX);
                vy1 = Convert.ToInt32(-((-xx2 * m_xysinangle) + (yy2 * m_xysinangle + zz2)) + m_nextY);

                if (m_showMode == ShowMode.Mode3D)//沿XYZ旋转 (-x,y,z)
                {
                    xx1 = Convert.ToSingle(-cx * cosy * cosz + -cy * (sinx * siny * cosz - cosx * sinz) + cz * (cosx * siny * cosz + sinx * sinz));
                    yy1 = Convert.ToSingle(-cx * cosy * sinz + -cy * (sinx * siny * sinz + cosx * cosz) + cz * (cosx * siny * sinz - sinx * cosz));
                    zz1 = Convert.ToSingle(-cx * -siny + -cy * sinx * cosy + cz * cosx * cosy);
                    xx2 = Convert.ToSingle(m_arrVector[0] * xx1 + m_arrVector[1] * yy1 + m_arrVector[2] * zz1) * m_axisXInver;
                    yy2 = Convert.ToSingle(m_arrVector[3] * xx1 + m_arrVector[4] * yy1 + m_arrVector[5] * zz1) * m_axisYInver;
                    zz2 = Convert.ToSingle(m_arrVector[6] * xx1 + m_arrVector[7] * yy1 + m_arrVector[8] * zz1) * m_axisZInver;
                }
                else
                {
                    xx2 = Convert.ToSingle(-cx); yy2 = Convert.ToSingle(-cy); zz2 = Convert.ToSingle(cz);
                }
                vx2 = Convert.ToInt32((-xx2 * m_xycosangle) + (-yy2 * m_xycosangle) + m_nextX);
                vy2 = Convert.ToInt32(-((-xx2 * m_xysinangle) + (yy2 * m_xysinangle + zz2)) + m_nextY);

                if (m_showMode == ShowMode.Mode3D)//沿XYZ旋转 (-x,-y,z)
                {
                    xx1 = Convert.ToSingle(-cx * cosy * cosz + cy * (sinx * siny * cosz - cosx * sinz) + cz * (cosx * siny * cosz + sinx * sinz));
                    yy1 = Convert.ToSingle(-cx * cosy * sinz + cy * (sinx * siny * sinz + cosx * cosz) + cz * (cosx * siny * sinz - sinx * cosz));
                    zz1 = Convert.ToSingle(-cx * -siny + cy * sinx * cosy + cz * cosx * cosy);
                    xx2 = Convert.ToSingle(m_arrVector[0] * xx1 + m_arrVector[1] * yy1 + m_arrVector[2] * zz1) * m_axisXInver;
                    yy2 = Convert.ToSingle(m_arrVector[3] * xx1 + m_arrVector[4] * yy1 + m_arrVector[5] * zz1) * m_axisYInver;
                    zz2 = Convert.ToSingle(m_arrVector[6] * xx1 + m_arrVector[7] * yy1 + m_arrVector[8] * zz1) * m_axisZInver;
                }
                else
                {
                    xx2 = Convert.ToSingle(-cx); yy2 = Convert.ToSingle(cy); zz2 = Convert.ToSingle(cz);
                }

                vx3 = Convert.ToInt32((-xx2 * m_xycosangle) + (-yy2 * m_xycosangle) + m_nextX);
                vy3 = Convert.ToInt32(-((-xx2 * m_xysinangle) + (yy2 * m_xysinangle + zz2)) + m_nextY);

                Point p = new Point(Convert.ToInt32(m_nextX), Convert.ToInt32(m_nextY));
                Point p1 = new Point(vx, vy);
                pDC.DrawLine(oldPen, p, p1);

                p = new Point(Convert.ToInt32(m_nextX), Convert.ToInt32(m_nextY));
                p1 = new Point(vx1, vy1);
                pDC.DrawLine(oldPen, p, p1);

                p = new Point(Convert.ToInt32(m_nextX), Convert.ToInt32(m_nextY));
                p1 = new Point(vx2, vy2);
                pDC.DrawLine(oldPen, p, p1);

                p = new Point(Convert.ToInt32(m_nextX), Convert.ToInt32(m_nextY));
                p1 = new Point(vx3, vy3);
                pDC.DrawLine(oldPen, p, p1);

                //以下方法为改进的Bresenham画圆算法(经典画圆(圆弧)算法)

                if (aa < 0)//aa= 2*(1-r); aa 为右下角的点到圆心的距离与r之差(Δd) 该点在圆内
                {
                    int bb = 2 * aa + 2 * cy - 1; //bb为正右方的点到圆心的距离与r之差(Δh)与右下角的点到圆心的距离与r之差(Δd)的差
                    if (bb <= 0)        //正右方的点到圆心的距离与r之差比右下角的点到圆心的距离与r之差小，更靠近r 
                    {
                        cx = cx + 1; aa = aa + 2 * cx + 1; //选择正右方的点
                    }
                    else
                    {
                        cx = cx + 1; cy = cy - 1; aa = aa + 2 * cx - 2 * cy + 2;//选择右下角的点
                    }
                }
                else if (aa > 0)        //aa 为右下角的点到圆心的距离与r之差 该点在圆外		
                {
                    int bb = 2 * aa - 2 * cx - 1;  //bb为右下角的点到圆心的距离与r之差与正下方的点到圆心的距离与r之差(Δv)的差
                    if (bb <= 0)
                    {
                        cx = cx + 1; cy = cy - 1; aa = aa + 2 * cx - 2 * cy + 2;    //选择右下角的点			
                    }
                    else
                    {
                        cy = cy - 1; aa = aa - 2 * cy + 1;      //选择正下方的点
                    }
                }
                else                    //aa==0
                {
                    cx = cx + 1; cy = cy - 1; aa = aa + 2 * cx - 2 * cy + 2;
                }
            }

            //Draw circle
            cx = 0; cy = r;
            vx = 0; vy = 0;
            aa = 2 * (1 - r);
            vx1 = 0; vy1 = 0;
            vx2 = 0; vy2 = 0;
            vx3 = 0; vy3 = 0;

            while (cy >= 0)
            {
                if (m_showMode == ShowMode.Mode3D)//沿XYZ旋转 (x,-y,z)
                {
                    xx1 = Convert.ToSingle(cx * cosy * cosz + -cy * (sinx * siny * cosz - cosx * sinz) + cz * (cosx * siny * cosz + sinx * sinz));
                    yy1 = Convert.ToSingle(cx * cosy * sinz + -cy * (sinx * siny * sinz + cosx * cosz) + cz * (cosx * siny * sinz - sinx * cosz));
                    zz1 = Convert.ToSingle(cx * -siny + -cy * sinx * cosy + cz * cosx * cosy);

                    xx2 = Convert.ToSingle(m_arrVector[0] * xx1 + m_arrVector[1] * yy1 + m_arrVector[2] * zz1) * m_axisXInver;
                    yy2 = Convert.ToSingle(m_arrVector[3] * xx1 + m_arrVector[4] * yy1 + m_arrVector[5] * zz1) * m_axisYInver;
                    zz2 = Convert.ToSingle(m_arrVector[6] * xx1 + m_arrVector[7] * yy1 + m_arrVector[8] * zz1) * m_axisZInver;
                }
                else
                {
                    xx2 = Convert.ToSingle(cx); yy2 = Convert.ToSingle(-cy); zz2 = Convert.ToSingle(cz);
                }

                vx = Convert.ToInt32((-xx2 * m_xycosangle) + (-yy2 * m_xycosangle) + m_nextX);
                vy = Convert.ToInt32(-((-xx2 * m_xysinangle) + (yy2 * m_xysinangle + zz2)) + m_nextY);

                if (m_showMode == ShowMode.Mode3D)//沿XYZ旋转 (x,y,z)
                {
                    xx1 = Convert.ToSingle(cx * cosy * cosz + cy * (sinx * siny * cosz - cosx * sinz) + cz * (cosx * siny * cosz + sinx * sinz));
                    yy1 = Convert.ToSingle(cx * cosy * sinz + cy * (sinx * siny * sinz + cosx * cosz) + cz * (cosx * siny * sinz - sinx * cosz));
                    zz1 = Convert.ToSingle(cx * -siny + cy * sinx * cosy + cz * cosx * cosy);
                    xx2 = Convert.ToSingle(m_arrVector[0] * xx1 + m_arrVector[1] * yy1 + m_arrVector[2] * zz1) * m_axisXInver;
                    yy2 = Convert.ToSingle(m_arrVector[3] * xx1 + m_arrVector[4] * yy1 + m_arrVector[5] * zz1) * m_axisYInver;
                    zz2 = Convert.ToSingle(m_arrVector[6] * xx1 + m_arrVector[7] * yy1 + m_arrVector[8] * zz1) * m_axisZInver;
                }
                else
                {
                    xx2 = Convert.ToSingle(cx); yy2 = Convert.ToSingle(cy); zz2 = Convert.ToSingle(cz);
                }

                vx1 = Convert.ToInt32((-xx2 * m_xycosangle) + (-yy2 * m_xycosangle) + m_nextX);
                vy1 = Convert.ToInt32(-((-xx2 * m_xysinangle) + (yy2 * m_xysinangle + zz2)) + m_nextY);
                if (m_showMode == ShowMode.Mode3D)//沿XYZ旋转 (-x,y,z)
                {
                    xx1 = Convert.ToSingle(-cx * cosy * cosz + -cy * (sinx * siny * cosz - cosx * sinz) + cz * (cosx * siny * cosz + sinx * sinz));
                    yy1 = Convert.ToSingle(-cx * cosy * sinz + -cy * (sinx * siny * sinz + cosx * cosz) + cz * (cosx * siny * sinz - sinx * cosz));
                    zz1 = Convert.ToSingle(-cx * -siny + -cy * sinx * cosy + cz * cosx * cosy);
                    xx2 = Convert.ToSingle(m_arrVector[0] * xx1 + m_arrVector[1] * yy1 + m_arrVector[2] * zz1) * m_axisXInver;
                    yy2 = Convert.ToSingle(m_arrVector[3] * xx1 + m_arrVector[4] * yy1 + m_arrVector[5] * zz1) * m_axisYInver;
                    zz2 = Convert.ToSingle(m_arrVector[6] * xx1 + m_arrVector[7] * yy1 + m_arrVector[8] * zz1) * m_axisZInver;
                }
                else
                {
                    xx2 = Convert.ToSingle(-cx); yy2 = Convert.ToSingle(-cy); zz2 = Convert.ToSingle(cz);
                }
                vx2 = Convert.ToInt32((-xx2 * m_xycosangle) + (-yy2 * m_xycosangle) + m_nextX);
                vy2 = Convert.ToInt32(-((-xx2 * m_xysinangle) + (yy2 * m_xysinangle + zz2)) + m_nextY);

                if (m_showMode == ShowMode.Mode3D)//沿XYZ旋转 (-x,-y,z)
                {
                    xx1 = Convert.ToSingle(-cx * cosy * cosz + cy * (sinx * siny * cosz - cosx * sinz) + cz * (cosx * siny * cosz + sinx * sinz));
                    yy1 = Convert.ToSingle(-cx * cosy * sinz + cy * (sinx * siny * sinz + cosx * cosz) + cz * (cosx * siny * sinz - sinx * cosz));
                    zz1 = Convert.ToSingle(-cx * -siny + cy * sinx * cosy + cz * cosx * cosy);
                    xx2 = Convert.ToSingle(m_arrVector[0] * xx1 + m_arrVector[1] * yy1 + m_arrVector[2] * zz1) * m_axisXInver;
                    yy2 = Convert.ToSingle(m_arrVector[3] * xx1 + m_arrVector[4] * yy1 + m_arrVector[5] * zz1) * m_axisYInver;
                    zz2 = Convert.ToSingle(m_arrVector[6] * xx1 + m_arrVector[7] * yy1 + m_arrVector[8] * zz1) * m_axisZInver;
                }
                else
                {
                    xx2 = Convert.ToSingle(-cx); yy2 = Convert.ToSingle(cy); zz2 = Convert.ToSingle(cz);
                }

                vx3 = Convert.ToInt32((-xx2 * m_xycosangle) + (-yy2 * m_xycosangle) + m_nextX);
                vy3 = Convert.ToInt32(-((-xx2 * m_xysinangle) + (yy2 * m_xysinangle + zz2)) + m_nextY);

                Point p = new Point(vx, vy);
                Point p1 = new Point(vx3, vy3);
                pDC.DrawLine(cirPen, p, p1);

                p = new Point(vx1, vy1);
                p1 = new Point(vx2, vy2);
                pDC.DrawLine(cirPen, p, p1);
                /*
                        int flag = 0;//cx+1
                        int R2 = r*r;
                        int r2 = (cx+1)*(cx+1)+cy*cy;
                        int tmp = cx*cx+(cy-1)*(cy-1);
                        if(abs(r2-R2)<abs(tmp-R2))
                        {
                        }else
                        {
                            r2 = tmp;
                            flag = 1;//cy-1
                        }
                        tmp = (cx+1)*(cx+1)+(cy-1)*(cy-1);
                        if(abs(r2-R2)<abs(tmp-R2))
                        {	
                        }
                        else
                        {
                            r2 = tmp;
                            flag = 2;//cx+1 cy-1
                        }
                        if(flag == 0)
                        {
                            cx += 1;
                        }else if(flag == 1)
                        {
                            cy -= 1;
                            flag = 0;
                        }else if(flag == 2)
                        {
                            cx += 1;
                            cy -= 1;
                            flag = 0;
                        }
                */
                //以下方法为改进的Bresenham画圆算法(经典画圆(圆弧)算法)

                if (aa < 0)//aa= 2*(1-r); aa 为右下角的点到圆心的距离与r之差(Δd) 该点在圆内
                {
                    int bb = 2 * aa + 2 * cy - 1; //bb为正右方的点到圆心的距离与r之差(Δh)与右下角的点到圆心的距离与r之差(Δd)的差
                    if (bb <= 0)        //正右方的点到圆心的距离与r之差比右下角的点到圆心的距离与r之差小，更靠近r 
                    {
                        cx = cx + 1; aa = aa + 2 * cx + 1; //选择正右方的点
                    }
                    else
                    {
                        cx = cx + 1; cy = cy - 1; aa = aa + 2 * cx - 2 * cy + 2;//选择右下角的点
                    }
                }
                else if (aa > 0)        //aa 为右下角的点到圆心的距离与r之差 该点在圆外		
                {
                    int bb = 2 * aa - 2 * cx - 1;  //bb为右下角的点到圆心的距离与r之差与正下方的点到圆心的距离与r之差(Δv)的差
                    if (bb <= 0)
                    {
                        cx = cx + 1; cy = cy - 1; aa = aa + 2 * cx - 2 * cy + 2;    //选择右下角的点			
                    }
                    else
                    {
                        cy = cy - 1; aa = aa - 2 * cy + 1;      //选择正下方的点
                    }
                }
                else                    //aa==0
                {
                    cx = cx + 1; cy = cy - 1; aa = aa + 2 * cx - 2 * cy + 2;
                }

            }

            pDC.Dispose();
        }

        public void DrawCurPoint(int i)
        {
            Graphics pDC = panel1.CreateGraphics();

            //m_prgText.SetSelected(0, true);

            if (0 == m_pNCRouteData[m_curPoint].GCode || 1 == m_pNCRouteData[m_curPoint].GCode)
            {
                //计算m_nextX、m_nextY
                CalNextPoint(m_curPoint);
                //还原
                RestoreBackground();
                //画图
                DrawRoute(m_curPoint);
                //保存
                SaveBackground();
                //画圆锥
                DrawCone();
                m_lastX = m_nextX;
                m_lastY = m_nextY;

                //sprintf(m_xVal.GetBuffer(50), "%lf", m_pNCRouteData[m_curPoint].x);
                //sprintf(m_yVal.GetBuffer(50), "%lf", m_pNCRouteData[m_curPoint].y);
                //sprintf(m_zVal.GetBuffer(50), "%lf", m_pNCRouteData[m_curPoint].z);
                //sprintf(m_aVal.GetBuffer(50), "%lf", m_pNCRouteData[m_curPoint].a);
                //sprintf(m_bVal.GetBuffer(50), "%lf", m_pNCRouteData[m_curPoint].b);
                //sprintf(m_cVal.GetBuffer(50), "%lf", m_pNCRouteData[m_curPoint].c);
            }
            pDC.Dispose();

            //SetDlgItemText(IDC_STATIC_XVAL, m_xVal);
            //SetDlgItemText(IDC_STATIC_YVAL, m_yVal);
            //SetDlgItemText(IDC_STATIC_ZVAL, m_zVal);
            //SetDlgItemText(IDC_STATIC_AVAL, m_aVal);
            //SetDlgItemText(IDC_STATIC_BVAL, m_bVal);
            //SetDlgItemText(IDC_STATIC_CVAL, m_cVal);
        }

        [DllImport(@"gdi32.dll")]
        public static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        public const int ROP_SrcCopy = 0xCC0020;

        #region 保存和还原背景
        public void SaveBackground()
        {
            m_coneBackWidth = 66 * m_unit;
            m_coneBackHeight = 66 * m_unit;
            m_lastBackPoint.X = Convert.ToInt32(m_nextX - 33 * m_unit);
            m_lastBackPoint.Y = Convert.ToInt32(m_nextY - 33 * m_unit);

            Graphics graphics = panel1.CreateGraphics();
            IntPtr hDsrc = graphics.GetHdc();
            Graphics graphics1 = Graphics.FromImage(m_coneBackDC);
            IntPtr hDdst = graphics1.GetHdc();
            BitBlt(hDdst, 0, 0, (int)m_coneBackWidth, (int)m_coneBackHeight, hDsrc,
                Convert.ToInt32(m_nextX - 33 * m_unit), Convert.ToInt32(m_nextY - 33 * m_unit), ROP_SrcCopy);

            pictureBox1.Image = m_coneBackDC;
            graphics.Dispose();
            graphics1.Dispose();

            m_flagSave = false;
        }

        public void RestoreBackground()
        {
            Graphics pDC = panel1.CreateGraphics();
            IntPtr hDdst = pDC.GetHdc();
            //Graphics graphics = Graphics.FromImage(m_coneBackDC);
            Graphics graphics = Graphics.FromImage(pictureBox1.Image);
            IntPtr hDsrc = graphics.GetHdc();
           
            BitBlt(hDdst,m_lastBackPoint.X, m_lastBackPoint.Y,
                (int)m_coneBackWidth, (int)m_coneBackHeight, hDsrc, 0, 0, ROP_SrcCopy);

            pictureBox1.Image = m_coneBackDC;

            pDC.Dispose();
            graphics.Dispose();
        }
        #endregion

        public void DrawThread()
        {
            Form1 p = (Form1)this;
            int curIndex = 0;
            Color oldColor =p.m_drawColor;
            p.m_drawColor = Color.FromArgb(255, 140, 0);

            int i = 0;

            while (true)
            {
                if (0 == p.m_flagState)            //移动
                {
                    for (i = p.m_curPoint; i < p.m_NCDataNum; i++)
                    {
                        if (1 == p.m_flagState)        //停止
                        {
                            break;
                        }
                        if (p.m_flagRedraw)
                        {
                            //redraw
                            if (p.m_flagClear)
                            {
                                p.m_clearPoint = i;
                                p.m_flagClear = false;
                            }
                            p.m_flagRedrawDone = false;
                            p.m_flagSave = true;
                            //p.Invalidate(true);
                            p.paint();
                            p.m_flagRedraw = false;
                            while (!p.m_flagRedrawDone) ;
                        }
                        //draw current point
                        p.DrawCurPoint(i);
                        p.m_curPoint++;
                        Thread.Sleep(p.m_drawRate);
                    }
                    if (p.m_curPoint == p.m_NCDataNum)    //完成
                            p.m_flagState = 2;

                }
                else            //其他 
                {
                    if (p.m_flagRedraw)    //重画
                    {
                        if (p.m_flagClear)
                        {
                            p.m_clearPoint = i;
                            p.m_flagClear = false;
                        }
                        p.m_flagRedrawDone = false;
                        p.m_flagSave = true;
                        //p.Invalidate(true);
                        p.paint();
                        p.m_flagRedraw = false;
                        while (!p.m_flagRedrawDone) ;
                    }
                }
            }
            p.m_drawColor = oldColor;
        }

        #region 控制图形移动
        private void BTN_Left_Click(object sender, EventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();
            graphics.Clear(panel1.BackColor);

            m_originPoint.X -= 20;
            if (m_flagState == -1)
            {
                m_nextX = m_originPoint.X;
                m_lastX = m_originPoint.X;
            }
            m_flagRedraw = true;
        }

        private void BTN_Right_Click(object sender, EventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();
            graphics.Clear(panel1.BackColor);

            m_originPoint.X += 20;
            if (m_flagState == -1)
            {
                m_nextX = m_originPoint.X;
                m_lastX = m_originPoint.X;
            }
            m_flagRedraw = true;
        }

        private void BTN_Up_Click(object sender, EventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();
            graphics.Clear(panel1.BackColor);

            m_originPoint.Y -= 20;
            if (m_flagState == -1)
            {
                m_nextY = m_originPoint.Y;
                m_lastY = m_originPoint.Y;
            }
            m_flagRedraw = true;
        }

        private void BTN_Down_Click(object sender, EventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();
            graphics.Clear(panel1.BackColor);

            m_originPoint.Y += 20;
            if (m_flagState == -1)
            {
                m_nextY = m_originPoint.Y;
                m_lastY = m_originPoint.Y;
            }
            m_flagRedraw = true;
        }
        #endregion

        #region 控制坐标轴反向
        private void BTN_X_Reverse_Click(object sender, EventArgs e)
        {
            m_flagXInver = true;
            AxisInversion();
            m_flagRedraw = true;
        }

        private void BTN_Y_Reverse_Click(object sender, EventArgs e)
        {
            m_flagYInver = true;
            AxisInversion();
            m_flagRedraw = true;
        }

        private void BTN_Z_Reverse_Click(object sender, EventArgs e)
        {
            m_flagZInver = true;
            AxisInversion();
            m_flagRedraw = true;
        }

        public void AxisInversion()
        {
            if (m_flagXInver)
            {
                m_flagXInver = false;
                m_axisXInver = -1 * m_axisXInver;
            }
            if (m_flagYInver)
            {
                m_flagYInver = false;
                m_axisYInver = -1 * m_axisYInver;
            }
            if (m_flagZInver)
            {
                m_flagZInver = false;
                m_axisZInver = -1 * m_axisZInver;
            }
        }
        #endregion

        #region 控制坐标轴旋转
        public void SetArrChangeAndTranspose(int roll_axis)
        {
            double x = 0, y = 0, z = 0;
            double u = 0, w = 0;
            switch (roll_axis)
            {
                case 0:
                    x = m_arrVector[0];
                    y = m_arrVector[3];
                    z = m_arrVector[6];
                    break;
                case 1:
                    x = m_arrVector[1];
                    y = m_arrVector[4];
                    z = m_arrVector[7];
                    break;
                case 2:
                    x = m_arrVector[2];
                    y = m_arrVector[5];
                    z = m_arrVector[8];
                    break;
                default:
                    break;
            }
            if (0 == (x * x + y * y))
            {
                m_arrChange[0] = 1.0;
                m_arrChange[1] = 0.0;
                m_arrChange[2] = 0.0;
            }
            else
            {
                u = Math.Sqrt(x * x + y * y);
                m_arrChange[0] = -1 * y / u;
                m_arrChange[1] = x / u;
                m_arrChange[2] = 0.0;
            }
            w = Math.Sqrt(x * x + y * y + z * z);
            m_arrChange[6] = x / w; //6
            m_arrChange[7] = y / w; //7
            m_arrChange[8] = z / w; //8

            m_arrChange[3] = m_arrChange[2] * m_arrChange[7] - m_arrChange[1] * m_arrChange[8];
            m_arrChange[4] = m_arrChange[0] * m_arrChange[8] - m_arrChange[2] * m_arrChange[6];
            m_arrChange[5] = m_arrChange[1] * m_arrChange[6] - m_arrChange[0] * m_arrChange[7];

            m_arrChangeTranpose[0] = m_arrChange[0];
            m_arrChangeTranpose[1] = m_arrChange[3];
            m_arrChangeTranpose[2] = m_arrChange[6];
            m_arrChangeTranpose[3] = m_arrChange[1];
            m_arrChangeTranpose[4] = m_arrChange[4];
            m_arrChangeTranpose[5] = m_arrChange[7];
            m_arrChangeTranpose[6] = m_arrChange[2];
            m_arrChangeTranpose[7] = m_arrChange[5];
            m_arrChangeTranpose[8] = m_arrChange[8];
        }

        public void SetArrVector()
        {
            double[] temp1 = new double[9];
            double[] temp2 = new double[9];

            for (int n = 0; n < 9; n++)
            {
                temp1[n] = 0.0;
                temp2[n] = 0.0;
            }

            MatrixMul(temp1, m_arrChangeTranpose, m_arrRollZ);
            MatrixMul(temp2, temp1, m_arrChange);
            for (int i = 0; i < 9; i++)
            {
                temp1[i] = m_arrVector[i];
            }
            MatrixMul(m_arrVector, temp2, temp1);
        }

        public void SetArrRollZ(double angle)
        {
            m_arrRollZ[0] = Math.Cos(m_rad * angle);
            m_arrRollZ[1] = -1 * Math.Sin(m_rad * angle);
            m_arrRollZ[2] = 0.0;
            m_arrRollZ[3] = Math.Sin(m_rad * angle);
            m_arrRollZ[4] = Math.Cos(m_rad * angle);
            m_arrRollZ[5] = 0.0;
            m_arrRollZ[6] = 0.0;
            m_arrRollZ[7] = 0.0;
            m_arrRollZ[8] = 1.0;
        }

        public void MatrixMul(double[] MatrixRes, double[] Matrix1, double[] Matrix2)
        {
            for(int i=0;i<3;i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        sum += Matrix1[3 * i + k] * Matrix2[3 * k + j];
                    }
                    MatrixRes[3 * i + j] = sum;
                }
            }
        }

        private void BTN_X_Rotate1_Click(object sender, EventArgs e)
        {
            SetArrRollZ(20.0);
            SetArrChangeAndTranspose(ROLL_X);
            SetArrVector();
            m_flagRedraw = true;
        }

        private void BTN_X_Rotate2_Click(object sender, EventArgs e)
        {
            SetArrRollZ(-20.0);
            SetArrChangeAndTranspose(ROLL_X);
            SetArrVector();
            m_flagRedraw = true;
        }

        private void BTN_Y_Rotate1_Click(object sender, EventArgs e)
        {
            SetArrRollZ(20.0);
            SetArrChangeAndTranspose(ROLL_Y);
            SetArrVector();
            m_flagRedraw = true;
        }

        private void BTN_Y_Rotate2_Click(object sender, EventArgs e)
        {
            SetArrRollZ(-20.0);
            SetArrChangeAndTranspose(ROLL_Y);
            SetArrVector();
            m_flagRedraw = true;
        }

        private void BTN_Z_Rotate1_Click(object sender, EventArgs e)
        {
            SetArrRollZ(20.0);
            SetArrChangeAndTranspose(ROLL_Z);
            SetArrVector();
            m_flagRedraw = true;
        }

        private void BTN_Z_Rotate2_Click(object sender, EventArgs e)
        {
            SetArrRollZ(-20.0);
            SetArrChangeAndTranspose(ROLL_Z);
            SetArrVector();
            m_flagRedraw = true;
        }
        #endregion


        public void paint()
        {
            Graphics graphics = panel1.CreateGraphics();
            graphics.Clear(panel1.BackColor);

            DrawAxis();
            RedrawRoute();
            if (m_flagSave)
                SaveBackground();
            DrawCone();
            m_flagRedrawDone = true;

            graphics.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initVar();
            panel1.Paint += Panel1_Paint;
            panel1.Refresh();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            paint();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void BTN_X_and_Y_Click(object sender, EventArgs e)
        {
            m_showMode = ShowMode.ModeXY;
            m_flagRedraw = true;
        }

        private void BTN_X_and_Z_Click(object sender, EventArgs e)
        {
            m_showMode = ShowMode.ModeXZ;
            m_flagRedraw = true;
        }

        private void BTN_Y_and_Z_Click(object sender, EventArgs e)
        {
            m_showMode = ShowMode.ModeYZ;
            m_flagRedraw = true;
        }

        private void BTN_G17_Click(object sender, EventArgs e)
        {
            m_showMode = ShowMode.ModeG17;
            m_flagRedraw = true;
        }

        private void BTN_Start_Click(object sender, EventArgs e)
        {
            if(m_NCDataNum == 0)
            {
                return;
            }
            m_flagState = 0;
        }

        private void BTN_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.txt)|*.txt|(*.*)|*.*";


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DrawAxis();
                DrawCone();

                m_prgText.Items.Clear();
                EDI_FilePath.Text = openFileDialog.FileName.Trim();

                //对读取的文件进行操作
                int indexCount = 0;
                try
                {
                    string[] lines = File.ReadAllLines(EDI_FilePath.Text, Encoding.UTF8);
                    m_NCDataNum = 0;
                    indexCount = lines.Count();

                    for (int i = 0; i < lines.Count(); ++i)
                    {
                        m_prgText.Items.Add(lines[i].ToUpper());
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("打开文件失败...");
                }

                if (m_pNCRouteData == null)
                {
                    //C#有垃圾回收机制,不需要释放空间
                }

                m_pNCRouteData = new POINT3D[indexCount];
                for (int i = 0; i < indexCount; ++i)
                {
                    m_pNCRouteData[i].x = 0.0;
                    m_pNCRouteData[i].y = 0.0;
                    m_pNCRouteData[i].z = 0.0;
                    m_pNCRouteData[i].a = 0.0;
                    m_pNCRouteData[i].b = 0.0;
                    m_pNCRouteData[i].c = 0.0;
                    m_pNCRouteData[i].GCode = -1;
                }

                for (int i = 0; i < indexCount; i++)
                {
                    DecodeAndSave(i);
                    m_NCDataNum++;
                }

                m_NCDataNum = indexCount;
                m_curPoint = 0;
                m_lastX = m_originPoint.X;
                m_lastY = m_originPoint.Y;
            }
        }

        unsafe public bool DecodeAndSave(int curentIndex)
        {
            #region 识别G代码
            string str;
            int strLen = 0;
            str = m_prgText.Items[curentIndex].ToString();

            strLen = str.Length;
            if (-1 == str.IndexOf("X") && -1 == str.IndexOf("Y") && -1 == str.IndexOf("Z") &&
                -1 == str.IndexOf("A") && -1 == str.IndexOf("B") && -1 == str.IndexOf("C") && -1 == str.IndexOf("G"))
            {
                return false;
            }
            else
            {
                int offsetLen = 0;
                string strX = "", strY = "", strZ = "", strA = "", strB = "", strC = "", strG = "";
                bool flagX = false, flagY = false, flagZ = false, flagA = false, flagB = false, flagC = false, flagG = false;
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == ';')
                    {
                        break;
                    }
                    if (str[i] == 'X')
                    {
                        if (i == strLen - 1)
                            break;
                        int j = 1;
                        int k = i + 1;          //x后一个字符位置
                        offsetLen = 0;
                        while (true)
                        {
                            if ((str[i + j] >= '0' && str[i + j] <= '9') || str[i + j] == '-' || str[i + j] == '.')
                            {
                                if ((i + j) >= strLen - 1)//末尾
                                {
                                    strX = str.Substring(k, i + j - k + 1);
                                    flagX = true;
                                    break;
                                }
                                else            //没到末尾
                                {
                                    j++;
                                    offsetLen++;
                                }
                            }
                            else                        //str[i+j]为其他字符 
                            {
                                strX = str.Substring(k, i + j - k);
                                flagX = true;
                                break;
                            }
                        }
                        i = i + offsetLen;
                    }
                    if (str[i] == 'Y')
                    {
                        if (i == strLen - 1)
                            break;
                        int j = 1;
                        int k = i + 1;          //y后一个字符位置
                        offsetLen = 0;
                        while (true)
                        {
                            if ((str[i + j] >= '0' && str[i + j] <= '9') || str[i + j] == '-' || str[i + j] == '.')
                            {
                                if ((i + j) >= strLen - 1)//末尾
                                {
                                    strY = str.Substring(k, i + j - k + 1);
                                    flagY = true;
                                    break;
                                }
                                else            //没到末尾
                                {
                                    j++;
                                    offsetLen++;
                                }
                            }
                            else                        //str[i+j]为其他字符 
                            {
                                strY = str.Substring(k, i + j - k);
                                flagY = true;
                                break;
                            }
                        }
                        i = i + offsetLen;
                    }
                    if (str[i] == 'Z')
                    {
                        if (i == strLen - 1)
                            break;
                        int j = 1;
                        int k = i + 1;          //z后一个字符位置
                        offsetLen = 0;
                        while (true)
                        {
                            if ((str[i + j] >= '0' && str[i + j] <= '9') || str[i + j] == '-' || str[i + j] == '.')
                            {
                                if ((i + j) >= strLen - 1)//末尾
                                {
                                    strZ = str.Substring(k, i + j - k + 1);
                                    flagZ = true;
                                    break;
                                }
                                else            //没到末尾
                                {
                                    j++;
                                    offsetLen++;
                                }
                            }
                            else                        //str[i+j]为其他字符 
                            {
                                strZ = str.Substring(k, i + j - k);
                                flagZ = true;
                                break;
                            }
                        }
                        i = i + offsetLen;
                    }
                    if (str[i] == 'A')
                    {
                        if (i == strLen - 1)
                            break;
                        int j = 1;
                        int k = i + 1;          //a后一个字符位置
                        offsetLen = 0;
                        while (true)
                        {
                            if ((str[i + j] >= '0' && str[i + j] <= '9') || str[i + j] == '-' || str[i + j] == '.')
                            {
                                if ((i + j) >= strLen - 1)//末尾
                                {
                                    strA = str.Substring(k, i + j - k + 1);
                                    flagA = true;
                                    break;
                                }
                                else            //没到末尾
                                {
                                    j++;
                                    offsetLen++;
                                }
                            }
                            else                        //str[i+j]为其他字符 
                            {
                                strA = str.Substring(k, i + j - k);
                                flagA = true;
                                break;
                            }
                        }
                        i = i + offsetLen;
                    }
                    if (str[i] == 'B')
                    {
                        if (i == strLen - 1)
                            break;
                        int j = 1;
                        int k = i + 1;          //b后一个字符位置
                        offsetLen = 0;
                        while (true)
                        {
                            if ((str[i + j] >= '0' && str[i + j] <= '9') || str[i + j] == '-' || str[i + j] == '.')
                            {
                                if ((i + j) >= strLen - 1)//末尾
                                {
                                    strB = str.Substring(k, i + j - k + 1);
                                    flagB = true;
                                    break;
                                }
                                else            //没到末尾
                                {
                                    j++;
                                    offsetLen++;
                                }
                            }
                            else                        //str[i+j]为其他字符 
                            {
                                strB = str.Substring(k, i + j - k);
                                flagB = true;
                                break;
                            }
                        }
                        i = i + offsetLen;
                    }
                    if (str[i] == 'C')
                    {
                        if (i == strLen - 1)
                            break;
                        int j = 1;
                        int k = i + 1;          //c后一个字符位置
                        offsetLen = 0;
                        while (true)
                        {
                            if ((str[i + j] >= '0' && str[i + j] <= '9') || str[i + j] == '-' || str[i + j] == '.')
                            {
                                if ((i + j) >= strLen - 1)//末尾
                                {
                                    strC = str.Substring(k, i + j - k + 1);
                                    flagC = true;
                                    break;
                                }
                                else            //没到末尾
                                {
                                    j++;
                                    offsetLen++;
                                }
                            }
                            else                        //str[i+j]为其他字符 
                            {
                                strC = str.Substring(k, i + j - k);
                                flagC = true;
                                break;
                            }
                        }
                        i = i + offsetLen;
                    }
                    if (str[i] == 'G')
                    {
                        if (i == strLen - 1)
                            break;
                        int j = 1;
                        int k = i + 1;          //g后一个字符位置
                        offsetLen = 0;
                        while (true)
                        {
                            if (str[i + j] >= '0' && str[i + j] <= '9')
                            {
                                if ((i + j) >= strLen - 1)//末尾
                                {
                                    strG = str.Substring(k, i + j - k + 1);
                                    flagG = true;
                                    break;
                                }
                                else            //没到末尾
                                {
                                    j++;
                                    offsetLen++;
                                }
                            }
                            else                        //str[i+j]为其他字符 
                            {
                                strG = str.Substring(k, i + j - k);
                                flagG = true;
                                break;
                            }
                        }
                        i = i + offsetLen;
                    }
                }           //end of for 
                #endregion

                #region 数据操作
                try
                {
                    if (flagX && (strX != ""))
                    {
                        flagX = false;
                        m_pNCRouteData[m_NCDataNum].x = Convert.ToDouble(strX);
                    }
                    else
                    {
                        if (m_NCDataNum != 0)
                            m_pNCRouteData[m_NCDataNum].x = m_pNCRouteData[m_NCDataNum - 1].x;
                        else
                            m_pNCRouteData[m_NCDataNum].x = m_NCLastData.x;
                    }

                    if (flagY && (strY != ""))
                    {
                        flagY = false;
                        m_pNCRouteData[m_NCDataNum].y = Convert.ToDouble(strY);
                    }
                    else
                    {
                        if (m_NCDataNum != 0)
                            m_pNCRouteData[m_NCDataNum].y = m_pNCRouteData[m_NCDataNum - 1].y;
                        else
                            m_pNCRouteData[m_NCDataNum].y = m_NCLastData.y;
                    }

                    if (flagZ && (strZ != ""))
                    {
                        flagZ = false;
                        m_pNCRouteData[m_NCDataNum].z = Convert.ToDouble(strZ);
                    }
                    else
                    {
                        if (m_NCDataNum != 0)
                            m_pNCRouteData[m_NCDataNum].z = m_pNCRouteData[m_NCDataNum - 1].z;
                        else
                            m_pNCRouteData[m_NCDataNum].z = m_NCLastData.z;
                    }

                    if (flagA && (strA != ""))
                    {
                        flagA = false;
                        m_pNCRouteData[m_NCDataNum].a = Convert.ToDouble(strA);
                    }
                    else
                    {
                        if (m_NCDataNum != 0)
                            m_pNCRouteData[m_NCDataNum].a = m_pNCRouteData[m_NCDataNum - 1].a;
                        else
                            m_pNCRouteData[m_NCDataNum].a = m_NCLastData.a;
                    }

                    if (flagB && (strB != ""))
                    {
                        flagB = false;
                        m_pNCRouteData[m_NCDataNum].b = Convert.ToDouble(strB);
                    }
                    else
                    {
                        if (m_NCDataNum != 0)
                            m_pNCRouteData[m_NCDataNum].b = m_pNCRouteData[m_NCDataNum - 1].b;
                        else
                            m_pNCRouteData[m_NCDataNum].b = m_NCLastData.b;
                    }

                    if (flagC && (strC != ""))
                    {
                        flagC = false;
                        m_pNCRouteData[m_NCDataNum].c = Convert.ToDouble(strC);
                    }
                    else
                    {
                        if (m_NCDataNum != 0)
                            m_pNCRouteData[m_NCDataNum].c = m_pNCRouteData[m_NCDataNum - 1].c;
                        else
                            m_pNCRouteData[m_NCDataNum].c = m_NCLastData.c;
                    }

                    if (flagG && (strG != ""))
                    {
                        flagG = false;
                        m_pNCRouteData[m_NCDataNum].GCode = (int)Convert.ToInt32(strG);
                    }
                    else
                    {
                        if (m_NCDataNum != 0)
                            m_pNCRouteData[m_NCDataNum].GCode = m_pNCRouteData[m_NCDataNum - 1].GCode;
                        else
                            m_pNCRouteData[m_NCDataNum].GCode = m_NCLastData.GCode;
                    }
                    return true;
                }
                catch (FormatException error)
                {
                    MessageBox.Show("字符串格式错误...");
                    return false;
                }
                #endregion
            }
        }
    }
}
