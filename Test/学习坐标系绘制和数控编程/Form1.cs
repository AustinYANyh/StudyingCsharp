using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Csharp_Plot
{
    struct POINT3D
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
        /// <summary>
        /// 变量
        /// </summary>
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
        //public CDC m_coneBackDC;
        public int m_axisXInver;
        public int m_axisYInver;
        public int m_axisZInver;
        public bool m_flagXInver;
        public bool m_flagYInver;
        public bool m_flagZInver;
        public int m_curShowMode;
        public bool m_flagModel;
        //public COLORREF m_drawColor;
        public int m_flagState;
        public int m_drawRate;
        //public ShowMode m_showMode;
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
        //CWinThread* m_pThread;


        public static List<Point> TrianglepointList = new List<Point>();
        public static List<float> EllispepointList = new List<float>();
        public static int threeDLength = 145;
        public static int needReverse = 1;
        public static bool XandZ = false;
        public static bool XandY = false;
        public static bool YandZ = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initVar();
            this.Controls.Add(panel1);
        }

        unsafe private void BTN_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.txt)|*.txt|(*.*)|*.*";


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                listBox1.Items.Clear();
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
                        listBox1.Items.Add(lines[i].ToUpper());
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("打开文件失败...");
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

        unsafe public void initVar()
        {
            m_originPoint.X = 500;
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
            //m_pThread = NULL;
            m_flagRedraw = false;
            //m_showMode = Mode3D;
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

            //m_coneBackDC.CreateCompatibleDC(GetDC());
            //m_coneBackBitmap.CreateCompatibleBitmap(GetDC(), 300, 300);
            //m_coneBackDC.SelectObject(&m_coneBackBitmap);

            //m_pThread = AfxBeginThread(DrawThread, (LPVOID)this);
        }

        unsafe public bool DecodeAndSave(int curentIndex)
        {
            string str;
            int strLen = 0;
            str = listBox1.Items[curentIndex].ToString();

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
            }
        }

        private void BTN_Left_Click(object sender, EventArgs e)
        {
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
            m_originPoint.Y += 20;
            if (m_flagState == -1)
            {
                m_nextY = m_originPoint.Y;
                m_lastY = m_originPoint.Y;
            }
            m_flagRedraw = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            SolidBrush brush = new SolidBrush(Color.Yellow);
            Panel panel = (Panel)sender;

            //画椭圆
            int triangleHeight = 85;
            float ellipseWidth = 50;
            float ellipseHeight = 40;
            float x = (panel.Width / 2) - (ellipseWidth / 2);
            float y = (panel.Height / 2) - (ellipseHeight / 2) - (triangleHeight / 2);

            EllispepointList.Add(x);
            EllispepointList.Add(y);
            EllispepointList.Add(ellipseWidth);
            EllispepointList.Add(ellipseHeight);

            //画三角形
            Pen pen = new Pen(Color.Red, 3);
            //左边起点x,y+ellipseHeight/2   中间点x+ellipseWidth/2,y+ellipseHeight/2+三角形高   
            //右边点x +ellipseWidth,y+ellipseHeight/2    
            Point pointLeft = new Point((int)x + 1, (int)(y + ellipseHeight / 2));
            Point pointMid = new Point((int)(x + ellipseWidth / 2), (int)(y + ellipseHeight / 2 + triangleHeight));
            Point pointRight = new Point((int)(x + ellipseWidth - 1), (int)(y + ellipseHeight / 2));

            TrianglepointList.Add(pointLeft);
            TrianglepointList.Add(pointMid);
            TrianglepointList.Add(pointRight);

            graphics.DrawLine(pen, pointLeft, pointMid);
            graphics.DrawLine(pen, pointMid, pointRight);
            graphics.DrawLine(pen, pointLeft, pointRight);

            //填充三角形
            Point[] points = { pointLeft, pointMid, pointRight };
            graphics.FillPolygon(Brushes.Red, points);

            //填充椭圆
            graphics.FillEllipse(brush, x, y, ellipseWidth, ellipseHeight);
        }

        private void BTN_X_and_Y_Click(object sender, EventArgs e)
        {
            needReverse = -1 * needReverse;
            Pen pen = new Pen(Brushes.Blue, 1);
            Graphics graphics = this.panel1.CreateGraphics();

            Rectangle rectangle = new Rectangle(0, 0, panel1.Width, panel1.Height);
            graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);

            //画X轴和+X
            graphics.DrawLine(pen, TrianglepointList[1], new Point(TrianglepointList[1].X + threeDLength, TrianglepointList[1].Y));
            graphics.DrawString("+X", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Blue), new Point(TrianglepointList[1].X + threeDLength, TrianglepointList[1].Y + 10));

            //画Y轴和+Y
            pen = new Pen(Brushes.Red, 1);
            graphics.DrawLine(pen, TrianglepointList[1], new Point(TrianglepointList[1].X, TrianglepointList[1].Y - threeDLength));
            graphics.DrawString("+Y", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Red), new Point(TrianglepointList[1].X - 10, TrianglepointList[1].Y - threeDLength - 10));

            Point[] points = { TrianglepointList[0], TrianglepointList[1], TrianglepointList[2] };
            graphics.FillPolygon(Brushes.Red, points);

            graphics.FillEllipse(new SolidBrush(Color.Yellow), EllispepointList[0], EllispepointList[1], EllispepointList[2], EllispepointList[3]);
            graphics.Dispose();

            XandY = true;
            XandZ = false;
            YandZ = false;
        }

        private void BTN_X_and_Z_Click(object sender, EventArgs e)
        {
            Pen pen = new Pen(Brushes.Blue, 1);
            Graphics graphics = this.panel1.CreateGraphics();

            Rectangle rectangle = new Rectangle(0, 0, panel1.Width, panel1.Height);
            graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);

            //画X轴和+X
            graphics.DrawLine(pen, TrianglepointList[1], new Point(TrianglepointList[1].X + threeDLength, TrianglepointList[1].Y));
            graphics.DrawString("+X", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Blue), new Point(TrianglepointList[1].X + threeDLength, TrianglepointList[1].Y + 10));

            //画Z轴和+Z
            pen = new Pen(Brushes.Green, 1);
            graphics.DrawLine(pen, TrianglepointList[1], new Point(TrianglepointList[1].X, TrianglepointList[1].Y - threeDLength));
            graphics.DrawString("+Z", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Green), new Point(TrianglepointList[1].X - 10, TrianglepointList[1].Y - threeDLength - 10));

            Point[] points = { TrianglepointList[0], TrianglepointList[1], TrianglepointList[2] };
            graphics.FillPolygon(Brushes.Red, points);

            graphics.FillEllipse(new SolidBrush(Color.Yellow), EllispepointList[0], EllispepointList[1], EllispepointList[2], EllispepointList[3]);
            graphics.Dispose();

            XandZ = true;
            XandY = false;
            YandZ = false;
        }

        private void BTN_Y_and_Z_Click(object sender, EventArgs e)
        {
            Pen pen = new Pen(Brushes.Red, 1);
            Graphics graphics = this.panel1.CreateGraphics();

            Rectangle rectangle = new Rectangle(0, 0, panel1.Width, panel1.Height);
            graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);

            //画Y轴和+Y
            graphics.DrawLine(pen, TrianglepointList[1], new Point(TrianglepointList[1].X + threeDLength, TrianglepointList[1].Y));
            graphics.DrawString("+Y", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Red), new Point(TrianglepointList[1].X + threeDLength, TrianglepointList[1].Y + 10));

            //画Z轴和+Z
            pen = new Pen(Brushes.Green, 1);
            graphics.DrawLine(pen, TrianglepointList[1], new Point(TrianglepointList[1].X, TrianglepointList[1].Y - threeDLength));
            graphics.DrawString("+Z", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Green), new Point(TrianglepointList[1].X - 10, TrianglepointList[1].Y - threeDLength - 10));

            Point[] points = { TrianglepointList[0], TrianglepointList[1], TrianglepointList[2] };
            graphics.FillPolygon(Brushes.Red, points);

            graphics.FillEllipse(new SolidBrush(Color.Yellow), EllispepointList[0], EllispepointList[1], EllispepointList[2], EllispepointList[3]);
            graphics.Dispose();

            YandZ = true;
            XandY = false;
            XandZ = false;
        }

        private void BTN_X_reverse_Click(object sender, EventArgs e)
        {
            if (XandZ == true || XandY == true)
            {
                needReverse = -1 * needReverse;
                Pen pen = new Pen(Brushes.Blue, 1);
                Graphics graphics = this.panel1.CreateGraphics();

                //先清除之前的X轴和字体
                Rectangle rectangle = new Rectangle(TrianglepointList[1].X, TrianglepointList[1].Y, threeDLength + 25, 25);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                rectangle = new Rectangle(TrianglepointList[1].X - threeDLength, TrianglepointList[1].Y, threeDLength, 25);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);


                //画X轴和+X
                graphics.DrawLine(pen, TrianglepointList[1], new Point(TrianglepointList[1].X + (needReverse * threeDLength), TrianglepointList[1].Y));
                graphics.DrawString("+X", new Font("宋体", 10, FontStyle.Regular),
                    new SolidBrush(Color.Blue), new Point(TrianglepointList[1].X + (needReverse * threeDLength), TrianglepointList[1].Y + 10));

                Point[] points = { TrianglepointList[0], TrianglepointList[1], TrianglepointList[2] };
                graphics.FillPolygon(Brushes.Red, points);

                graphics.FillEllipse(new SolidBrush(Color.Yellow), EllispepointList[0], EllispepointList[1], EllispepointList[2], EllispepointList[3]);
                graphics.Dispose();
            }
            else
            {
                //do nothing
            }
        }

        private void BTN_Y_reverse_Click(object sender, EventArgs e)
        {
            
            Graphics graphics = panel1.CreateGraphics();

            if (XandY == true)
            {
                //只消除Y轴和+Y,其他不变,重新填充
                Rectangle rectangle = new Rectangle(TrianglepointList[1].X, TrianglepointList[1].Y - 85 -40 -50, 1, threeDLength + 25);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                rectangle = new Rectangle(TrianglepointList[1].X-25, TrianglepointList[1].Y - 85 - 40 - 55, 75, 75);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
            }
            else if(YandZ == true)
            {
                //消除横向的Y轴,其他不变
            }
            else
            {
                //do nothing
            }
        }
    }
}
