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
        public static Point midPoint = new Point();
        public static int ThreeDLength = 145;
        public static int ParallelogramLength = 255;
        public static int ParallelogramHeight = 175;
        public static int needXReverse = 1;
        public static int needYReverse = 1;
        public static int needZReverse = 1;
        public static int LeftandRightP = 0;
        public static int UpandDownP = 0;
        public static int PreLandRP = 0;
        public static int PreUandDP = 0;
        public static bool XandZ = false;
        public static bool XandY = false;
        public static bool YandZ = false;
        public static bool XandYandZ = false;
        public static bool G17 = false;

        //椭圆和三角形属性
        public static int triangleHeight = 85;
        public static float ellipseWidth = 50;
        public static float ellipseHeight = 40;
        public static List<Point> triPoints = new List<Point>();
        public static List<Point> eliPoints = new List<Point>();

        //坐标点属性
        public static List<Point> NowXPoint = new List<Point>();
        public static List<Point> NowYPoint = new List<Point>();
        public static List<Point> NowZPoint = new List<Point>();
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initVar();
            this.Controls.Add(panel1);

            #region 初始位置的三角形和椭圆坐标,更新
            //画椭圆
            float x = (this.panel1.Width / 2) - (ellipseWidth / 2) + LeftandRightP;
            float y = (this.panel1.Height / 2) - (ellipseHeight / 2) - (triangleHeight / 2) + UpandDownP;

            EllispepointList.Add(x);
            EllispepointList.Add(y);
            EllispepointList.Add(ellipseWidth);
            EllispepointList.Add(ellipseHeight);

            eliPoints.Add(new Point((int)x, (int)y));

            //画三角形
            //左边起点x,y+ellipseHeight/2   中间点x+ellipseWidth/2,y+ellipseHeight/2+三角形高   
            //右边点x +ellipseWidth,y+ellipseHeight/2    
            Point pointLeft = new Point((int)x + 1, (int)(y + ellipseHeight / 2));
            Point pointMid = new Point((int)(x + ellipseWidth / 2), (int)(y + ellipseHeight / 2 + triangleHeight));
            Point pointRight = new Point((int)(x + ellipseWidth - 1), (int)(y + ellipseHeight / 2));

            TrianglepointList.Add(pointLeft);
            TrianglepointList.Add(pointMid);
            TrianglepointList.Add(pointRight);

            triPoints.Add(pointLeft);
            triPoints.Add(pointMid);
            triPoints.Add(pointRight);
            #endregion

            InitXYZ();
        }

        public void InitXYZ()
        {
            NowXPoint.Clear();
            NowYPoint.Clear();
            NowZPoint.Clear();

            //初始XYZ坐标
            double xWidth = ThreeDLength * Math.Cos(Math.PI / 6);
            double yHeight = ThreeDLength * Math.Sin(Math.PI / 6);

            Point xPoint = new Point(TrianglepointList[1].X + (int)xWidth, TrianglepointList[1].Y + (int)yHeight);
            Point XPoint = new Point(TrianglepointList[1].X + (int)xWidth, TrianglepointList[1].Y + (int)yHeight - 15);
            Point ypoint = new Point(TrianglepointList[1].X + (int)xWidth, TrianglepointList[1].Y - (int)yHeight);
            Point YPoint = new Point(TrianglepointList[1].X + (int)xWidth - 15, TrianglepointList[1].Y - (int)yHeight - 15);
            Point zPoint = new Point(TrianglepointList[1].X, TrianglepointList[1].Y - ThreeDLength);
            Point ZPoint = new Point(TrianglepointList[1].X - 10, TrianglepointList[1].Y - ThreeDLength - 13);

            NowXPoint.Add(xPoint);
            NowXPoint.Add(XPoint);
            NowYPoint.Add(ypoint);
            NowYPoint.Add(YPoint);
            NowZPoint.Add(zPoint);
            NowZPoint.Add(ZPoint);
        }

        public List<Point> CalPoint(Point midPoint)
        {
            #region 计算初始时正反的XYZ坐标和+X+Y+Z坐标
            double xWidth = ThreeDLength * Math.Cos(Math.PI / 6);
            double yHeight = ThreeDLength * Math.Sin(Math.PI / 6);

            Point xPoint = new Point(midPoint.X + (int)xWidth, midPoint.Y + (int)yHeight);
            Point XPoint = new Point(midPoint.X + (int)xWidth, midPoint.Y + (int)yHeight - 15);
            Point ypoint = new Point(midPoint.X + (int)xWidth, midPoint.Y - (int)yHeight);
            Point YPoint = new Point(midPoint.X + (int)xWidth - 15, midPoint.Y - (int)yHeight - 15);
            Point zPoint = new Point(midPoint.X, midPoint.Y - ThreeDLength);
            Point ZPoint = new Point(midPoint.X - 10, midPoint.Y - ThreeDLength - 13);

            Point RxPoint = new Point(midPoint.X + needXReverse*(int)xWidth, midPoint.Y + needXReverse * (int)yHeight);
            Point RXPoint = new Point(midPoint.X + needXReverse * (int)xWidth, midPoint.Y + needXReverse * (int)yHeight - needXReverse * 15);
            Point Rypoint = new Point(midPoint.X + needYReverse * (int)xWidth, midPoint.Y - needYReverse*(int)yHeight);
            Point RYPoint = new Point(midPoint.X + needYReverse*(int)xWidth - needYReverse*15, midPoint.Y - needYReverse*(int)yHeight - needYReverse*15);
            Point RzPoint = new Point(midPoint.X, midPoint.Y - needZReverse*ThreeDLength);
            Point RZPoint = new Point(midPoint.X - 10, midPoint.Y - needZReverse * ThreeDLength - needZReverse * 13);

            List<Point> calPoint = new List<Point>();

            calPoint.Add(xPoint);
            calPoint.Add(XPoint);
            calPoint.Add(ypoint);
            calPoint.Add(YPoint);
            calPoint.Add(zPoint);
            calPoint.Add(ZPoint);

            calPoint.Add(RxPoint);
            calPoint.Add(RXPoint);
            calPoint.Add(Rypoint);
            calPoint.Add(RYPoint);
            calPoint.Add(RzPoint);
            calPoint.Add(RZPoint);

            return calPoint;
            #endregion
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
            #region 识别G代码
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

        private void BTN_Left_Click(object sender, EventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();
            LeftandRightP -= 20;

            if (XandYandZ == true)
            {
                if (G17 == false)
                {
                    rePaint(graphics);
                }
                else
                {
                    rePaint(graphics);
                    paintG17();
                }
            }
            else if(XandY == true)
            {
                rePaintXandY(graphics);
            }
            else if(YandZ == true)
            {
                rePaintYandZ(graphics);
            }
            else
            {
                rePaintXandZ(graphics);
            }
        }

        private void BTN_Right_Click(object sender, EventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();
            LeftandRightP += 20;

            if (XandYandZ == true)
            {
                if (G17 == false)
                {
                    rePaint(graphics);
                }
                else
                {
                    rePaint(graphics);
                    paintG17();
                }
            }
            else if (XandY == true)
            {
                rePaintXandY(graphics);
            }
            else if (YandZ == true)
            {
                rePaintYandZ(graphics);
            }
            else
            {
                rePaintXandZ(graphics);
            }
        }

        private void BTN_Up_Click(object sender, EventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();
            UpandDownP -= 20;

            if (XandYandZ == true)
            {
                if (G17 == false)
                {
                    rePaint(graphics);
                }
                else
                {
                    rePaint(graphics);
                    paintG17();
                }
            }
            else if (XandY == true)
            {
                rePaintXandY(graphics);
            }
            else if (YandZ == true)
            {
                rePaintYandZ(graphics);
            }
            else
            {
                rePaintXandZ(graphics);
            }
        }

        private void BTN_Down_Click(object sender, EventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();
            UpandDownP += 20;

            if (XandYandZ == true)
            {
                if (G17 == false)
                {
                    rePaint(graphics);
                }
                else
                {
                    rePaint(graphics);
                    paintG17();
                }
            }
            else if (XandY == true)
            {
                rePaintXandY(graphics);
            }
            else if (YandZ == true)
            {
                rePaintYandZ(graphics);
            }
            else
            {
                rePaintXandZ(graphics);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            paintTandE(graphics);

            midPoint = TrianglepointList[1];
            paint3D(graphics,TrianglepointList[1]);

            XandYandZ = true;
            XandY = false;
            XandZ = false;
            YandZ = false;

            Point[] points = { triPoints[0], triPoints[1], triPoints[2] };
            graphics.FillPolygon(Brushes.Red, points);

            graphics.FillEllipse(new SolidBrush(Color.Yellow), eliPoints[0].X, eliPoints[0].Y, EllispepointList[2], EllispepointList[3]);
            graphics.Dispose();
        }

        public void paintTandE(Graphics graphics)
        {
            SolidBrush brush = new SolidBrush(Color.Yellow);
            Pen pen = new Pen(Color.Red, 3);

            #region 按照当前中心点位置计算三角形坐标,更新坐标,画三角形
            Point triLeft = new Point(triPoints[0].X + LeftandRightP - PreLandRP,
                triPoints[0].Y + UpandDownP - PreUandDP);
            Point triMid = new Point(triPoints[1].X + LeftandRightP - PreLandRP,
                triPoints[1].Y + UpandDownP - PreUandDP);
            Point triRight = new Point(triPoints[2].X + LeftandRightP - PreLandRP,
                triPoints[2].Y + UpandDownP - PreUandDP);

            triPoints.Clear(); 
            triPoints.Add(triLeft);  
            triPoints.Add(triMid);
            triPoints.Add(triRight);  
                              
            graphics.DrawLine(pen, triLeft, triMid);
            graphics.DrawLine(pen, triLeft, triRight);
            graphics.DrawLine(pen, triMid, triRight);
            #endregion

            #region 填充三角形,椭圆,并更新椭圆位置
            Point[] points = { triLeft, triMid, triRight };
            graphics.FillPolygon(Brushes.Red, points);

            //填充椭圆
            float x = eliPoints[0].X + LeftandRightP - PreLandRP;
            float y = eliPoints[0].Y + UpandDownP - PreUandDP;
            graphics.FillEllipse(brush, x, y, ellipseWidth, ellipseHeight);
            eliPoints.Clear();
            eliPoints.Add(new Point((int)x, (int)y));
            #endregion
        }

        public void paintTandE1(Graphics graphics)
        {
            SolidBrush brush = new SolidBrush(Color.Yellow);
            Pen pen = new Pen(Color.Red, 3);

            Point triLeft = new Point(TrianglepointList[0].X + LeftandRightP,
                TrianglepointList[0].Y + UpandDownP);
            Point triMid = new Point(TrianglepointList[1].X + LeftandRightP,
                TrianglepointList[1].Y + UpandDownP);
            Point triRight = new Point(TrianglepointList[2].X + LeftandRightP,
               TrianglepointList[2].Y + UpandDownP);

            triPoints.Clear();
            triPoints.Add(triLeft);
            triPoints.Add(triMid);
            triPoints.Add(triRight);

            graphics.DrawLine(pen, triLeft, triMid);
            graphics.DrawLine(pen, triLeft, triRight);
            graphics.DrawLine(pen, triMid, triRight);

            //填充三角形
            Point[] points = { triLeft, triMid, triRight };
            graphics.FillPolygon(Brushes.Red, points);

            //填充椭圆
            float x = EllispepointList[0] + LeftandRightP;
            float y = EllispepointList[1] + UpandDownP;
            eliPoints.Clear();
            eliPoints.Add(new Point((int)x, (int)y));
            graphics.FillEllipse(brush, x, y, ellipseWidth, ellipseHeight);
        }

        public void paint3D(Graphics graphics,Point standardPoint)
        { 
            Point xPoint = new Point(NowXPoint[0].X + LeftandRightP - PreLandRP,
                NowXPoint[0].Y + UpandDownP - PreUandDP);
            
            Pen pen = new Pen(Color.Blue, 1);
            graphics.DrawLine(pen, xPoint, standardPoint);
            Point XPoint = new Point(NowXPoint[1].X + LeftandRightP - PreLandRP, 
                NowXPoint[1].Y + UpandDownP - PreUandDP);
            graphics.DrawString("+X", new Font("宋体", 10, FontStyle.Regular), Brushes.Blue, XPoint);

            pen = new Pen(Brushes.Green, 1);
            Point zPoint = new Point(NowZPoint[0].X + LeftandRightP - PreLandRP, 
                NowZPoint[0].Y + UpandDownP - PreUandDP);
            graphics.DrawLine(pen, standardPoint, zPoint);
            //受限于panel高度,+Z的Y高度只能+13
            Point ZPoint = new Point(NowZPoint[1].X + LeftandRightP - PreLandRP, 
                NowZPoint[1].Y + UpandDownP - PreUandDP);
            graphics.DrawString("+Z", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Green), ZPoint);

            pen = new Pen(Color.Red, 1);
            Point ypoint = new Point(NowYPoint[0].X + LeftandRightP - PreLandRP, 
                NowYPoint[0].Y + UpandDownP - PreUandDP);
            Point YPoint = new Point(NowYPoint[1].X + LeftandRightP - PreLandRP, 
                NowYPoint[1].Y + UpandDownP - PreUandDP);
            graphics.DrawLine(pen, ypoint, standardPoint); 
            graphics.DrawString("+Y", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Red), YPoint);

            PreLandRP = LeftandRightP;
            PreUandDP = UpandDownP;

            updateXPoints(xPoint, XPoint);
            updateYPoints(ypoint, YPoint);
            updateZPoints(zPoint, ZPoint);
        }                 

        public void update3D(List<Point> list)
        {
            Graphics graphics = panel1.CreateGraphics();

            Point xPoint = list[0];

            Pen pen = new Pen(Color.Blue, 1);
            graphics.DrawLine(pen, xPoint, midPoint);
            Point XPoint = list[1];
            graphics.DrawString("+X", new Font("宋体", 10, FontStyle.Regular), Brushes.Blue, XPoint);

            pen = new Pen(Brushes.Green, 1);
            Point zPoint = list[4];
            graphics.DrawLine(pen, midPoint, zPoint);
            //受限于panel高度,+Z的Y高度只能+13
            Point ZPoint =list[5];
            graphics.DrawString("+Z", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Green), ZPoint);

            pen = new Pen(Color.Red, 1);
            Point ypoint = list[2];
            Point YPoint = list[3];
            graphics.DrawLine(pen, ypoint, midPoint);
            graphics.DrawString("+Y", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Red), YPoint);

            PreLandRP = LeftandRightP;
            PreUandDP = UpandDownP;

            updateXPoints(xPoint, XPoint);
            updateYPoints(ypoint, YPoint);
            updateZPoints(zPoint, ZPoint);
        }

        public void rePaint(Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), 0, 0, panel1.Width, panel1.Height);

            paintTandE(graphics);

            float x = (this.panel1.Width / 2) - (ellipseWidth / 2) + LeftandRightP;
            float y = (this.panel1.Height / 2) - (ellipseHeight / 2) - (triangleHeight / 2) + UpandDownP;

            midPoint = new Point((int)(x + ellipseWidth / 2), (int)(y + ellipseHeight / 2 + triangleHeight));
            paint3D(graphics, midPoint);

            Point[] points = { triPoints[0], triPoints[1], triPoints[2] };
            graphics.FillPolygon(Brushes.Red, points);

            //填充椭圆
            graphics.FillEllipse(Brushes.Yellow, eliPoints[0].X, eliPoints[0].Y, ellipseWidth, ellipseHeight);
        }

        public void rePaintXandY(Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), 0, 0, panel1.Width, panel1.Height);

           // paintTandE(graphics);

            float x = (this.panel1.Width / 2) - (ellipseWidth / 2) + LeftandRightP;
            float y = (this.panel1.Height / 2) - (ellipseHeight / 2) - (triangleHeight / 2) + UpandDownP;

            midPoint = new Point((int)(x + ellipseWidth / 2), (int)(y + ellipseHeight / 2 + triangleHeight));

            Point xPoint = new Point(NowXPoint[0].X + LeftandRightP - PreLandRP,
                NowXPoint[0].Y + UpandDownP - PreUandDP);
            Point XPoint = new Point(NowXPoint[1].X + LeftandRightP - PreLandRP,
                NowXPoint[1].Y + UpandDownP - PreUandDP);

            Point yPoint = new Point(NowYPoint[0].X + LeftandRightP - PreLandRP,
                NowYPoint[0].Y + UpandDownP - PreUandDP);
            Point YPoint = new Point(NowYPoint[1].X + LeftandRightP - PreLandRP,
               NowYPoint[1].Y + UpandDownP - PreUandDP);

            PreUandDP = UpandDownP;
            PreLandRP = LeftandRightP;
            updateYPoints(yPoint, YPoint);
            updateXPoints(xPoint, XPoint);

            paintXandY(graphics, midPoint);

            paintTandE1(graphics);
        }

        public void rePaintYandZ(Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), 0, 0, panel1.Width, panel1.Height);


            float x = (this.panel1.Width / 2) - (ellipseWidth / 2) + LeftandRightP;
            float y = (this.panel1.Height / 2) - (ellipseHeight / 2) - (triangleHeight / 2) + UpandDownP;

            midPoint = new Point((int)(x + ellipseWidth / 2), (int)(y + ellipseHeight / 2 + triangleHeight));
    
            Point yPoint = new Point(NowYPoint[0].X + LeftandRightP - PreLandRP,
                NowYPoint[0].Y + UpandDownP - PreUandDP);
            Point YPoint = new Point(NowYPoint[1].X + LeftandRightP - PreLandRP,
               NowYPoint[1].Y + UpandDownP - PreUandDP);

            Point zPoint = new Point(NowZPoint[0].X + LeftandRightP - PreLandRP,
                NowZPoint[0].Y + UpandDownP - PreUandDP);
            Point ZPoint = new Point(NowZPoint[1].X + LeftandRightP - PreLandRP,
                NowZPoint[1].Y + UpandDownP - PreUandDP);

            PreUandDP = UpandDownP;
            PreLandRP = LeftandRightP;
            updateYPoints(yPoint, YPoint);
            updateZPoints(zPoint, ZPoint);

            paintYandZ(graphics, midPoint);

            paintTandE1(graphics);
        }

        public void rePaintXandZ(Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), 0, 0, panel1.Width, panel1.Height);

            float x = (this.panel1.Width / 2) - (ellipseWidth / 2) + LeftandRightP;
            float y = (this.panel1.Height / 2) - (ellipseHeight / 2) - (triangleHeight / 2) + UpandDownP;

            midPoint = new Point((int)(x + ellipseWidth / 2), (int)(y + ellipseHeight / 2 + triangleHeight));

            Point xPoint = new Point(NowXPoint[0].X + LeftandRightP - PreLandRP,
                NowXPoint[0].Y + UpandDownP - PreUandDP);
            Point XPoint = new Point(NowXPoint[1].X + LeftandRightP - PreLandRP,
                NowXPoint[1].Y + UpandDownP - PreUandDP);

            Point zPoint = new Point(NowZPoint[0].X + LeftandRightP - PreLandRP,
                NowZPoint[0].Y + UpandDownP - PreUandDP);
            Point ZPoint = new Point(NowZPoint[1].X + LeftandRightP - PreLandRP,
                NowZPoint[1].Y + UpandDownP - PreUandDP);

            PreUandDP = UpandDownP;
            PreLandRP = LeftandRightP;
            updateXPoints(xPoint, XPoint);
            updateZPoints(zPoint, ZPoint);

            paintXandZ(graphics, midPoint);

            paintTandE1(graphics);
        }

        public void paintXandY(Graphics graphics,Point standardPoint)
        {
            //画X轴和+X
            Pen pen = new Pen(Brushes.Blue, 1);
            graphics.DrawLine(pen, standardPoint, NowXPoint[0]);
            graphics.DrawString("+X", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Blue), NowXPoint[1]);

            //画Y轴和+Y
            pen = new Pen(Brushes.Red, 1);
            graphics.DrawLine(pen, standardPoint, NowYPoint[0]);
            graphics.DrawString("+Y", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Red), NowYPoint[1]);

            XandY = true;
            XandZ = false;
            YandZ = false;
            XandYandZ = false;
        }

        public void paintYandZ(Graphics graphics,Point standardPoint)
        {
            Pen pen = new Pen(Brushes.Red, 1);
            Rectangle rectangle = new Rectangle(0, 0, panel1.Width, panel1.Height);
            graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);

            //画Y轴和+Y
            graphics.DrawLine(pen, standardPoint, NowYPoint[0]);
            graphics.DrawString("+Y", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Red), NowYPoint[1]);

            //画Z轴和+Z
            pen = new Pen(Brushes.Green, 1);
            graphics.DrawLine(pen, standardPoint, NowZPoint[0]);
            graphics.DrawString("+Z", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Green),NowZPoint[1]);

            YandZ = true;
            XandY = false;
            XandZ = false;
            XandYandZ = false;
        }

        public void paintXandZ(Graphics graphics,Point standardPoint)
        {
            
            Rectangle rectangle = new Rectangle(0, 0, panel1.Width, panel1.Height);
            graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);

            //画X轴和+X
            Pen pen = new Pen(Brushes.Blue, 1);
            graphics.DrawLine(pen, standardPoint, NowXPoint[0]);
            graphics.DrawString("+X", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Blue), NowXPoint[1]);

            //画Z轴和+Z
            pen = new Pen(Brushes.Green, 1);
            graphics.DrawLine(pen, standardPoint, NowZPoint[0]);
            graphics.DrawString("+Z", new Font("宋体", 10, FontStyle.Regular),
                new SolidBrush(Color.Green),NowZPoint[1]);

            XandZ = true;
            XandY = false;
            YandZ = false;
            XandYandZ = false;
        }

        public void updateXPoints(Point xPoint, Point XPoint)
        {
            NowXPoint.Clear();
            NowXPoint.Add(xPoint);
            NowXPoint.Add(XPoint);
        }

        public void updateYPoints(Point yPoint, Point YPoint)
        {         
            NowYPoint.Clear();
            NowYPoint.Add(yPoint);
            NowYPoint.Add(YPoint);
        }

        public void updateZPoints(Point zPoint, Point ZPoint)
        {
            NowZPoint.Clear();
            NowZPoint.Add(zPoint);
            NowZPoint.Add(ZPoint);
        }

        public void paintG17()
        {
            /*
            //画虚线平行四边形
            Point x1 = new Point(triPoints[1].X - ParallelogramLength, triPoints[1].Y);
            Point x2 = new Point(triPoints[1].X + ParallelogramLength, triPoints[1].Y);
            Point y1 = new Point(triPoints[1].X, triPoints[1].Y - ParallelogramHeight);
            Point y2 = new Point(triPoints[1].X, triPoints[1].Y + ParallelogramHeight);

            Graphics graphics = panel1.CreateGraphics();
            Pen pen = new Pen(Color.CornflowerBlue, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;

            graphics.DrawLine(pen, x1, y1);
            graphics.DrawLine(pen, x1, y2);
            graphics.DrawLine(pen, x2, y1);
            graphics.DrawLine(pen, x2, y2);
            */

            //平行四边形不是重点,重点是二维坐标系和三维的转换

            #region 根据XYZ的正向与否更新三维坐标系中的XYZ坐标
            List<Point> list = CalPoint(midPoint);

            Point xPoint = list[0];
            Point XPoint = list[1];
            Point yPoint = list[2];
            Point YPoint = list[3];
            Point zPoint = list[4];
            Point ZPoint = list[5];

            if(needXReverse == -1)
            {
                xPoint = list[6];
                XPoint = list[7];
            }
            if(needYReverse == -1)
            {
                yPoint = list[8];
                YPoint = list[9];
            }
            if(needZReverse == -1)
            {
                zPoint = list[10];
                ZPoint = list[11];
            }
            list.Clear();
            list.Add(xPoint);
            list.Add(XPoint);
            list.Add(yPoint);
            list.Add(YPoint);
            list.Add(zPoint);
            list.Add(ZPoint);

            updateXPoints(xPoint, XPoint);
            updateYPoints(yPoint, YPoint);
            updateZPoints(zPoint, ZPoint);
            #endregion

            #region 将三角形坐标更新为正向
            float x = (this.panel1.Width / 2) - (ellipseWidth / 2) + LeftandRightP;
            float y = (this.panel1.Height / 2) - (ellipseHeight / 2) - (triangleHeight / 2) + UpandDownP;

            Point pointMid = new Point((int)(x + ellipseWidth / 2), (int)(y + ellipseHeight / 2 + triangleHeight));
            Point pointLeft = new Point((int)x + 1, pointMid.Y - triangleHeight);
            Point pointRight = new Point((int)(x + ellipseWidth - 1), pointMid.Y - triangleHeight);

            triPoints.Clear();
            triPoints.Add(pointLeft);
            triPoints.Add(pointMid);
            triPoints.Add(pointRight);
            #endregion

            #region 画3维坐标轴和三角形椭圆
            update3D(list);
            paintTandE(panel1.CreateGraphics());
            
            XandYandZ = true;
            G17 = true;
            XandY = false;
            XandZ = false;
            YandZ = false;
            #endregion
     
        }

        private void BTN_X_and_Y_Click(object sender, EventArgs e)
        {
            #region 清除画面
            Graphics graphics = this.panel1.CreateGraphics();
            Rectangle rectangle = new Rectangle(0, 0, panel1.Width, panel1.Height);
            graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
            #endregion

            #region 更新XY坐标绘制XY坐标轴
            Point xPoint = new Point(midPoint.X + ThreeDLength, midPoint.Y);
            Point XPoint = new Point(midPoint.X + ThreeDLength, midPoint.Y + 10);
            Point yPoint = new Point(midPoint.X, midPoint.Y - ThreeDLength);
            Point YPoint = new Point(midPoint.X - 10, midPoint.Y - ThreeDLength - 10);

            if(needXReverse == -1)
            {
                xPoint = new Point(midPoint.X + needXReverse*ThreeDLength, midPoint.Y); 
                XPoint = new Point(midPoint.X + needXReverse*ThreeDLength, midPoint.Y + needXReverse*10);
            }
            if(needYReverse == -1)
            {
                yPoint = new Point(midPoint.X, midPoint.Y - needYReverse*ThreeDLength);
                YPoint = new Point(midPoint.X - needYReverse*10, midPoint.Y - needYReverse*ThreeDLength - needYReverse*10);
            }
            updateXPoints(xPoint, XPoint);
            updateYPoints(yPoint, YPoint);

            paintXandY(graphics, midPoint);
            #endregion

            #region 重新填充三角形和椭圆
            Point[] points = { triPoints[0], triPoints[1], triPoints[2] };
            graphics.FillPolygon(Brushes.Red, points);

            graphics.FillEllipse(new SolidBrush(Color.Yellow), eliPoints[0].X, 
                eliPoints[0].Y, EllispepointList[2], EllispepointList[3]);
            graphics.Dispose();

            XandYandZ = false;
            XandY = true;
            XandZ = false;
            YandZ = false;
            #endregion
        }

        private void BTN_X_and_Z_Click(object sender, EventArgs e)
        {
            Graphics graphics = this.panel1.CreateGraphics();

            #region 清除三角形
            Point[] points = { new Point(TrianglepointList[0].X+LeftandRightP, TrianglepointList[0].Y+UpandDownP),
                    midPoint, new Point(TrianglepointList[2].X+LeftandRightP,TrianglepointList[2].Y+UpandDownP) };

            graphics.FillPolygon(new SolidBrush(Panel.DefaultBackColor), points);
            graphics.DrawLine(new Pen(new SolidBrush(Panel.DefaultBackColor), 3),
                new Point(TrianglepointList[0].X + LeftandRightP, TrianglepointList[0].Y + UpandDownP),
                midPoint);
            graphics.DrawLine(new Pen(new SolidBrush(Panel.DefaultBackColor), 3), midPoint,
                new Point(TrianglepointList[2].X + LeftandRightP, TrianglepointList[2].Y + UpandDownP));
            graphics.DrawLine(new Pen(new SolidBrush(Panel.DefaultBackColor), 3),
                new Point(TrianglepointList[0].X + LeftandRightP, TrianglepointList[0].Y + UpandDownP),
                new Point(TrianglepointList[2].X + LeftandRightP, TrianglepointList[2].Y + UpandDownP));

            int triangleHeight = 85;
            float ellipseWidth = 50;
            float ellipseHeight = 40;

            float x = (this.panel1.Width / 2) - (ellipseWidth / 2) + LeftandRightP;
            float y = (this.panel1.Height / 2) - (ellipseHeight / 2) - (triangleHeight / 2) + UpandDownP;

            Point pointMid = new Point((int)(x + ellipseWidth / 2), (int)(y + ellipseHeight / 2 + triangleHeight));
            Point pointLeft = new Point((int)x + 1, pointMid.Y + triangleHeight);
            Point pointRight = new Point((int)(x + ellipseWidth - 1), pointMid.Y + triangleHeight);
            Point[] ppp = { pointLeft, pointMid, pointRight };
            graphics.FillPolygon(new SolidBrush(Panel.DefaultBackColor), ppp);
            graphics.DrawLine(new Pen(new SolidBrush(Panel.DefaultBackColor), 3), pointLeft, pointMid);
            graphics.DrawLine(new Pen(new SolidBrush(Panel.DefaultBackColor), 3), pointMid, pointRight);
            graphics.DrawLine(new Pen(new SolidBrush(Panel.DefaultBackColor), 3), pointLeft, pointRight);
            #endregion

            #region 更新XZ坐标画XZ坐标轴
            Point xPoint = new Point(midPoint.X + ThreeDLength, midPoint.Y);
            Point XPoint = new Point(midPoint.X + ThreeDLength, midPoint.Y + 10);
            Point zPoint = new Point(midPoint.X, midPoint.Y - ThreeDLength);
            Point ZPoint = new Point(midPoint.X - 10, midPoint.Y - ThreeDLength - 10);

            if (needXReverse == -1)
            {
                xPoint = new Point(midPoint.X + needXReverse*ThreeDLength, midPoint.Y);
                XPoint = new Point(midPoint.X + needXReverse*ThreeDLength, midPoint.Y + needXReverse*10);
            }
            if (needZReverse == -1)
            {
                zPoint = new Point(midPoint.X, midPoint.Y - needZReverse*ThreeDLength);
                ZPoint = new Point(midPoint.X - needZReverse*10, midPoint.Y - needZReverse*ThreeDLength - needZReverse*10);
            }

            updateXPoints(xPoint, XPoint);
            updateZPoints(zPoint, ZPoint);
           
            paintXandZ(graphics, midPoint);
            #endregion

            #region 重新填充三角形和椭圆
            Point[] tri = { points[0], points[1], points[2] };
            graphics.FillPolygon(Brushes.Red, points);

            graphics.FillEllipse(new SolidBrush(Color.Yellow), EllispepointList[0]+LeftandRightP,
                EllispepointList[1]+UpandDownP, EllispepointList[2], EllispepointList[3]);
            graphics.Dispose();

            XandYandZ = false;
            XandY = false;
            XandZ = true;
            YandZ = false;
            #endregion

            #region 更新三角形和椭圆坐标
            triPoints.Clear();
            triPoints.Add(points[0]);
            triPoints.Add(points[1]);
            triPoints.Add(points[2]);
            eliPoints.Clear();
            eliPoints.Add(new Point((int)EllispepointList[0] + LeftandRightP, (int)EllispepointList[1] + UpandDownP));
            graphics.Dispose();
            #endregion
        }

        private void BTN_Y_and_Z_Click(object sender, EventArgs e)
        {
            #region 清除画面
            Graphics graphics = this.panel1.CreateGraphics();

            Pen pen = new Pen(Brushes.Red, 1);
            Rectangle rectangle = new Rectangle(0, 0, panel1.Width, panel1.Height);
            graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
            #endregion

            #region 更新YZ坐标画YZ坐标轴
            Point yPoint = new Point(midPoint.X + ThreeDLength, midPoint.Y);
            Point YPoint = new Point(midPoint.X + ThreeDLength, midPoint.Y + 10);
            Point zPoint = new Point(midPoint.X, midPoint.Y - ThreeDLength);
            Point ZPoint = new Point(midPoint.X - 10, midPoint.Y - ThreeDLength - 10);

            if (needYReverse == -1)
            {
                yPoint = new Point(midPoint.X + needYReverse*ThreeDLength, midPoint.Y);
                YPoint = new Point(midPoint.X + needYReverse*ThreeDLength, midPoint.Y + needYReverse*10);
            }
            if (needZReverse == -1)
            {
                zPoint = new Point(midPoint.X, midPoint.Y - needZReverse * ThreeDLength);
                ZPoint = new Point(midPoint.X - needZReverse * 10, midPoint.Y - needZReverse * ThreeDLength - needZReverse * 10);
            }

            updateYPoints(yPoint, YPoint);
            updateZPoints(zPoint, ZPoint);

            paintYandZ(graphics, midPoint);
            #endregion

            #region  重新填充三角形和椭圆
            Point[] points = { triPoints[0], triPoints[1], triPoints[2] };
            graphics.FillPolygon(Brushes.Red, points);

            graphics.FillEllipse(new SolidBrush(Color.Yellow), eliPoints[0].X,
                eliPoints[0].Y, EllispepointList[2], EllispepointList[3]);
            graphics.Dispose();

            XandYandZ = false;
            XandY = false;
            XandZ = false;
            YandZ = true;
            #endregion
        }

        private void BTN_X_reverse_Click(object sender, EventArgs e)
        {
            Graphics graphics = this.panel1.CreateGraphics();

            if (XandZ == true || XandY == true)
            {
                #region 清除之前的X轴和字体
                needXReverse = -1 * needXReverse;
                Pen pen = new Pen(Brushes.Blue, 1);
                
                Rectangle rectangle = new Rectangle(midPoint.X, midPoint.Y, ThreeDLength + 25, 1);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                rectangle = new Rectangle(midPoint.X - ThreeDLength, midPoint.Y, ThreeDLength, 1);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                //清除之前的字体
                rectangle = new Rectangle(midPoint.X + ThreeDLength, midPoint.Y, 25, 25);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                rectangle = new Rectangle(midPoint.X - ThreeDLength, midPoint.Y, 25, 25);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                #endregion

                #region 画X轴和+X,并更新坐标
                Point xPoint = new Point(midPoint.X + (needXReverse * ThreeDLength), midPoint.Y);
                Point XPoint = new Point(midPoint.X + (needXReverse * ThreeDLength), midPoint.Y + 10);

                updateXPoints(xPoint, XPoint);

                graphics.DrawLine(pen, midPoint, xPoint);
                graphics.DrawString("+X", new Font("宋体", 10, FontStyle.Regular),
                    new SolidBrush(Color.Blue), XPoint);
                #endregion

                #region 重新填充三角形和椭圆
                Point[] points = { new Point(TrianglepointList[0].X+LeftandRightP, TrianglepointList[0].Y+UpandDownP),
                    midPoint, new Point(TrianglepointList[2].X+LeftandRightP,TrianglepointList[2].Y+UpandDownP) };
                graphics.FillPolygon(Brushes.Red, points);

                graphics.FillEllipse(new SolidBrush(Color.Yellow), 
                    EllispepointList[0] +LeftandRightP
                    , EllispepointList[1]+UpandDownP, EllispepointList[2], EllispepointList[3]);
                #endregion

            }
            else if (XandYandZ == true)
            {
                #region 消除全部X轴和字体
                needXReverse = -1 * needXReverse;

                Pen pen = new Pen(new SolidBrush(Panel.DefaultBackColor), 1);
                double xWidth = ThreeDLength * Math.Cos(Math.PI / 6);
                double yHeight = ThreeDLength * Math.Sin(Math.PI / 6);

                //Point xPoint = new Point(midPoint.X + (int)xWidth, midPoint.Y + (int)yHeight);
                //Point xReversePoint = new Point(midPoint.X - (int)xWidth, midPoint.Y - (int)yHeight);
                Point xPoint = NowXPoint[0];
                Point xReversePoint = NowXPoint[1];
                graphics.DrawLine(pen, xPoint, midPoint);
                graphics.DrawLine(pen, xReversePoint, midPoint);

                Point XPoint = new Point(midPoint.X + (int)xWidth, midPoint.Y + (int)yHeight - 15);
                Point XReversePoint = new Point(midPoint.X - (int)xWidth, midPoint.Y - (int)yHeight + 15);
                graphics.DrawString("+X", new Font("宋体", 10, FontStyle.Regular), new SolidBrush(Panel.DefaultBackColor), XPoint);
                graphics.DrawString("+X", new Font("宋体", 10, FontStyle.Regular), new SolidBrush(Panel.DefaultBackColor), XReversePoint);
                #endregion

                #region 重新画X轴,并更新坐标
                pen = new Pen(Color.Blue, 1);
                Point newxPoint = new Point(midPoint.X + (needXReverse * (int)xWidth), midPoint.Y + (needXReverse * (int)yHeight));
                graphics.DrawLine(pen, newxPoint, midPoint);
                Point newXPoint = new Point(midPoint.X + (needXReverse * (int)xWidth), midPoint.Y + (needXReverse * (int)yHeight - (needXReverse * 15)));
                graphics.DrawString("+X", new Font("宋体", 10, FontStyle.Regular), Brushes.Blue, newXPoint);

                updateXPoints(newxPoint, newXPoint);
                #endregion
            }
            else
            {
                //do nothing
            }
            graphics.Dispose();
        }

        private void BTN_Y_reverse_Click(object sender, EventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();

            if (XandY == true || YandZ == true)
            {
                #region 清除之前的Y轴,字体
                Rectangle rectangle = new Rectangle(midPoint.X, midPoint.Y, 1, ThreeDLength + 25);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                rectangle = new Rectangle(midPoint.X, midPoint.Y - ThreeDLength, 1, ThreeDLength);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);

                //清除之前的字体(减去的25是字体高度)
                rectangle = new Rectangle(midPoint.X - 25, midPoint.Y - ThreeDLength - 25, 35, 35);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                rectangle = new Rectangle(midPoint.X - 25, midPoint.Y + ThreeDLength, 35, 35);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                #endregion

                #region 画反向的Y轴和+Y,并更新坐标
                needYReverse = -1 * needYReverse;
                Pen pen = new Pen(Brushes.Red, 1);

                Point yPoint = new Point(midPoint.X, midPoint.Y - (needYReverse * ThreeDLength));
                Point YPoint = new Point(midPoint.X - 10, midPoint.Y - (needYReverse * ThreeDLength) - (needYReverse * 10));

                updateYPoints(yPoint, YPoint);
                graphics.DrawLine(pen, midPoint, yPoint);
                graphics.DrawString("+Y", new Font("宋体", 10, FontStyle.Regular),
                    new SolidBrush(Color.Red), YPoint);
                #endregion

                #region 重新填充三角形和椭圆
                Point[] points = { new Point(TrianglepointList[0].X+LeftandRightP, TrianglepointList[0].Y+UpandDownP),
                    midPoint, new Point(TrianglepointList[2].X+LeftandRightP,TrianglepointList[2].Y+UpandDownP) };
                graphics.FillPolygon(Brushes.Red, points);

                graphics.FillEllipse(new SolidBrush(Color.Yellow),
                    EllispepointList[0] + LeftandRightP
                    , EllispepointList[1] + UpandDownP, EllispepointList[2], EllispepointList[3]);
                #endregion              
            }
            else if (YandZ == true)
            {
                #region  清除之前的横轴,字体
                needZReverse = -1 * needZReverse;

                Rectangle rectangle = new Rectangle(midPoint.X, midPoint.Y, ThreeDLength + 25, 1);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                rectangle = new Rectangle(midPoint.X - ThreeDLength, midPoint.Y, ThreeDLength, 1);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                //清除之前的字体
                rectangle = new Rectangle(midPoint.X + ThreeDLength, midPoint.Y, 25, 25);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                rectangle = new Rectangle(midPoint.X - ThreeDLength, midPoint.Y, 25, 25);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                #endregion

                #region 重新画Y轴和+Y,并更新坐标
                Pen pen = new Pen(Color.Red, 1);
                Point yPoint = new Point(midPoint.X + (needZReverse * ThreeDLength), midPoint.Y);
                Point YPoint = new Point(midPoint.X + (needZReverse * ThreeDLength), midPoint.Y + 10);
                graphics.DrawLine(pen, midPoint, yPoint);
                graphics.DrawString("+Y", new Font("宋体", 10, FontStyle.Regular),
                    new SolidBrush(Color.Red), YPoint);

                updateYPoints(yPoint, YPoint);
                #endregion
            }
            else if (XandYandZ == true)
            {
                #region 消除全部Y轴和字体
                needYReverse = -1 * needYReverse;

                double xWidth = ThreeDLength * Math.Cos(Math.PI / 6);
                double yHeight = ThreeDLength * Math.Sin(Math.PI / 6);
                Pen pen = new Pen(new SolidBrush(Panel.DefaultBackColor), 1);
                //Point ypoint = new Point(midPoint.X + (int)xWidth, midPoint.Y - (int)yHeight);
                Point ypoint = NowYPoint[0];
                Point yReversePoint = NowYPoint[1];
                //Point yReversePoint = new Point(midPoint.X - (int)xWidth, midPoint.Y + (int)yHeight);
                graphics.DrawLine(pen, ypoint, midPoint);
                graphics.DrawLine(pen, yReversePoint, midPoint);

                graphics.DrawString("+Y", new Font("宋体", 10, FontStyle.Regular),
                    new SolidBrush(Panel.DefaultBackColor),
                    new Point(midPoint.X + (int)xWidth - 15, midPoint.Y - (int)yHeight - 15));
                graphics.DrawString("+Y", new Font("宋体", 10, FontStyle.Regular),
                    new SolidBrush(Panel.DefaultBackColor),
                    new Point(midPoint.X - (int)xWidth + 15, midPoint.Y + (int)yHeight + 15));
                #endregion

                #region 重新画Y轴和+Y,并更新坐标
                pen = new Pen(Color.Red, 1);
                Point newyPoint = new Point(midPoint.X + (needYReverse * (int)xWidth), midPoint.Y - (needYReverse * (int)yHeight));
                graphics.DrawLine(pen, newyPoint, midPoint);
                Point newYPoint = new Point(midPoint.X + (needYReverse * (int)xWidth) - (needYReverse * 15), midPoint.Y - (needYReverse * (int)yHeight) - (needYReverse * 15));
                graphics.DrawString("+Y", new Font("宋体", 10, FontStyle.Regular), Brushes.Red, newYPoint);

                updateYPoints(newyPoint, newyPoint);
                #endregion
            }
            else
            {
                //do nothing
            }
            graphics.Dispose();
        }

        private void BTN_Z_reverse_Click(object sender, EventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();

            if (XandZ == true)
            {
                #region 清除之前的竖轴,字体
                needZReverse = -1 * needZReverse;
                Rectangle rectangle = new Rectangle(midPoint.X, midPoint.Y, 1, ThreeDLength + 25);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                rectangle = new Rectangle(midPoint.X, midPoint.Y - ThreeDLength, 1, ThreeDLength);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);

                rectangle = new Rectangle(midPoint.X - 25, midPoint.Y - ThreeDLength - 25, 35, 35);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                rectangle = new Rectangle(midPoint.X - 25, midPoint.Y + ThreeDLength - 25, 35, 35);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                #endregion

                #region 重新画Z轴和+Z,更新Z坐标
                Pen pen = new Pen(Brushes.Green, 1);
                Point zPoint = new Point(midPoint.X, midPoint.Y - (needZReverse * ThreeDLength));
                Point ZPoint = new Point(midPoint.X - 10, midPoint.Y - (needZReverse * ThreeDLength) - 10);
                graphics.DrawLine(pen, midPoint, zPoint);
                graphics.DrawString("+Z", new Font("宋体", 10, FontStyle.Regular),
                    new SolidBrush(Color.Green), ZPoint);

                updateZPoints(zPoint, ZPoint);
                #endregion

                #region 重新填充三角形和椭圆
                Point[] points = { new Point(TrianglepointList[0].X+LeftandRightP, TrianglepointList[0].Y+UpandDownP),
                    midPoint, new Point(TrianglepointList[2].X+LeftandRightP,TrianglepointList[2].Y+UpandDownP) };
                graphics.FillPolygon(Brushes.Red, points);

                graphics.FillEllipse(new SolidBrush(Color.Yellow),
                    EllispepointList[0] + LeftandRightP
                    , EllispepointList[1] + UpandDownP, EllispepointList[2], EllispepointList[3]);
                #endregion
            }
            else if (YandZ == true)
            {
                #region 清除数轴和字体
                needZReverse = -1 * needZReverse;

                Rectangle rectangle = new Rectangle(midPoint.X, midPoint.Y, 1, ThreeDLength + 25);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                rectangle = new Rectangle(midPoint.X, midPoint.Y - ThreeDLength, 1, ThreeDLength);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);

                rectangle = new Rectangle(midPoint.X - 25, midPoint.Y - ThreeDLength - 25, 35, 35);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                rectangle = new Rectangle(midPoint.X - 25, midPoint.Y + ThreeDLength - 25, 35, 35);
                graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), rectangle);
                #endregion

                #region 重新画Z轴和+Z,更新Z坐标
                Pen pen = new Pen(Brushes.Green, 1);
                Point zPoint = new Point(midPoint.X, midPoint.Y - (needZReverse * ThreeDLength));
                Point ZPoint = new Point(midPoint.X - 10, midPoint.Y - (needZReverse * ThreeDLength) - 10);
                graphics.DrawLine(pen, midPoint, zPoint);
                graphics.DrawString("+Z", new Font("宋体", 10, FontStyle.Regular),
                    new SolidBrush(Color.Green), ZPoint);

                updateZPoints(zPoint, ZPoint);
                #endregion

                #region 重新填充三角形和椭圆
                Point[] points = { new Point(TrianglepointList[0].X+LeftandRightP, TrianglepointList[0].Y+UpandDownP),
                    midPoint, new Point(TrianglepointList[2].X+LeftandRightP,TrianglepointList[2].Y+UpandDownP) };
                graphics.FillPolygon(Brushes.Red, points);

                graphics.FillEllipse(new SolidBrush(Color.Yellow),
                    EllispepointList[0] + LeftandRightP
                    , EllispepointList[1] + UpandDownP, EllispepointList[2], EllispepointList[3]);
                #endregion
            }
            else if (XandYandZ == true )
            {
                #region  清除三角形
                needZReverse = -1 * needZReverse;

                Point[] points = { new Point(TrianglepointList[0].X+LeftandRightP, TrianglepointList[0].Y+UpandDownP),
                    midPoint, new Point(TrianglepointList[2].X+LeftandRightP,TrianglepointList[2].Y+UpandDownP) };

                graphics.FillPolygon(new SolidBrush(Panel.DefaultBackColor), points);
                graphics.DrawLine(new Pen(new SolidBrush(Panel.DefaultBackColor), 3), 
                    new Point(TrianglepointList[0].X + LeftandRightP, TrianglepointList[0].Y + UpandDownP), 
                    midPoint);
                graphics.DrawLine(new Pen(new SolidBrush(Panel.DefaultBackColor), 3), midPoint,
                    new Point(TrianglepointList[2].X + LeftandRightP, TrianglepointList[2].Y + UpandDownP));
                graphics.DrawLine(new Pen(new SolidBrush(Panel.DefaultBackColor), 3),
                    new Point(TrianglepointList[0].X + LeftandRightP, TrianglepointList[0].Y + UpandDownP),
                    new Point(TrianglepointList[2].X + LeftandRightP, TrianglepointList[2].Y + UpandDownP));

                int triangleHeight = 85;
                float ellipseWidth = 50;
                float ellipseHeight = 40;

                float x = (this.panel1.Width / 2) - (ellipseWidth / 2) + LeftandRightP;
                float y = (this.panel1.Height / 2) - (ellipseHeight / 2) - (triangleHeight / 2) + UpandDownP;

                Point pointMid = new Point((int)(x + ellipseWidth / 2), (int)(y + ellipseHeight / 2 + triangleHeight));
                Point pointLeft = new Point((int)x + 1, pointMid.Y + triangleHeight);
                Point pointRight = new Point((int)(x + ellipseWidth - 1), pointMid.Y + triangleHeight);
                Point[] ppp = { pointLeft, pointMid, pointRight };
                graphics.FillPolygon(new SolidBrush(Panel.DefaultBackColor), ppp);
                graphics.DrawLine(new Pen(new SolidBrush(Panel.DefaultBackColor), 3), pointLeft, pointMid);
                graphics.DrawLine(new Pen(new SolidBrush(Panel.DefaultBackColor), 3), pointMid, pointRight);
                graphics.DrawLine(new Pen(new SolidBrush(Panel.DefaultBackColor), 3), pointLeft, pointRight);
                #endregion

                #region 清除椭圆
                graphics.FillEllipse(new SolidBrush(Panel.DefaultBackColor),
                    EllispepointList[0] + LeftandRightP, EllispepointList[1] + UpandDownP, 
                    EllispepointList[2], EllispepointList[3]);
                graphics.FillEllipse(new SolidBrush(Panel.DefaultBackColor), 
                    219+LeftandRightP, 319+UpandDownP, EllispepointList[2], EllispepointList[3]);
                #endregion

                #region 消除Z轴和+Z
                Pen pen = new Pen(Panel.DefaultBackColor, 1);
                //graphics.DrawLine(pen, midPoint, new Point(midPoint.X, midPoint.Y - ThreeDLength));
                //graphics.DrawLine(pen, midPoint, new Point(midPoint.X, midPoint.Y + ThreeDLength));

                graphics.DrawLine(pen, midPoint, NowZPoint[0]);
                graphics.DrawLine(pen, midPoint, NowZPoint[1]);
                //受限于panel高度,+Z的Y高度只能+13
                graphics.DrawString("+Z", new Font("宋体", 10, FontStyle.Regular),
                    new SolidBrush(Panel.DefaultBackColor), new Point(midPoint.X - 10, midPoint.Y - ThreeDLength - 13));
                graphics.DrawString("+Z", new Font("宋体", 10, FontStyle.Regular),
                    new SolidBrush(Panel.DefaultBackColor), new Point(midPoint.X - 10, midPoint.Y + ThreeDLength + 13));
                #endregion

                if (G17 == false)
                {
                    #region 反向三角形
                    needZReverse = -1 * needZReverse;
                    pen = new Pen(Color.Red, 3);
                    pointMid = new Point((int)(x + ellipseWidth / 2), (int)(y + ellipseHeight / 2 + triangleHeight));
                    pointLeft = new Point((int)x + 1, pointMid.Y + (needZReverse * triangleHeight));
                    pointRight = new Point((int)(x + ellipseWidth - 1), pointMid.Y + (needZReverse * triangleHeight));

                    triPoints.Clear();
                    triPoints.Add(pointLeft);
                    triPoints.Add(pointMid);
                    triPoints.Add(pointRight);

                    graphics.DrawLine(pen, pointLeft, pointMid);
                    graphics.DrawLine(pen, pointMid, pointRight);
                    graphics.DrawLine(pen, pointLeft, pointRight);
                    needZReverse = -1 * needZReverse;
                    #endregion
                }
                else
                {
                    #region 正向三角形
                    pen = new Pen(Color.Red, 3);
                    pointMid = new Point((int)(x + ellipseWidth / 2), (int)(y + ellipseHeight / 2 + triangleHeight));
                    pointLeft = new Point((int)x + 1, pointMid.Y -   triangleHeight);
                    pointRight = new Point((int)(x + ellipseWidth - 1), pointMid.Y -  triangleHeight);

                    triPoints.Clear();
                    triPoints.Add(pointLeft);
                    triPoints.Add(pointMid);
                    triPoints.Add(pointRight);

                    graphics.DrawLine(pen, pointLeft, pointMid);
                    graphics.DrawLine(pen, pointMid, pointRight);
                    graphics.DrawLine(pen, pointLeft, pointRight);
                    #endregion
                }

                #region  重新画Z轴和+Z,更新Z坐标
                pen = new Pen(Brushes.Green, 1);
                Point newzPoint = new Point(midPoint.X, midPoint.Y - (needZReverse * ThreeDLength));
                Point newZPoint = new Point(midPoint.X - 10, midPoint.Y - (needZReverse * ThreeDLength) - (needZReverse * 13));
                graphics.DrawLine(pen, midPoint, newzPoint);
                graphics.DrawString("+Z", new Font("宋体", 10, FontStyle.Regular),
                    new SolidBrush(Color.Green), newZPoint);

                updateZPoints(newzPoint, newZPoint);
                #endregion

                #region 重新画椭圆(219,mid.Y-椭圆高/2---319),填充三角形和椭圆
                y = pointLeft.Y - ellipseHeight / 2;

                Point[] newPoint = { pointLeft, pointMid, pointRight };
                graphics.FillPolygon(Brushes.Red, newPoint);
                graphics.FillEllipse(new SolidBrush(Color.Yellow), x, y, ellipseWidth, ellipseHeight);

                eliPoints.Clear();
                eliPoints.Add(new Point((int)x, (int)y));
                #endregion
            }
            else
            {
                //do nothing
            }

            graphics.Dispose();
        }

        private void BTN_G17_Click(object sender, EventArgs e)
        {
            Graphics graphics = panel1.CreateGraphics();
            graphics.FillRectangle(new SolidBrush(Panel.DefaultBackColor), 0, 0, panel1.Width, panel1.Height);

            paintG17();
            graphics.Dispose();
        }

        private void BTN_G18_Click(object sender, EventArgs e)
        {

        }
    }
}
