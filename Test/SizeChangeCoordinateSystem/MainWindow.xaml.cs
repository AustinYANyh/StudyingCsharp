using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //设置小球坐标时的偏移量
        private double XOffset = 7.5;//小球的长度的1/2

        private double YOffset = 7.5;//小球的宽度的1/2

        //X轴刻度Max值
        private double MaxCoordinateAxisX = 500;

        //Y轴刻度Max值
        private double MaxCoordinateAxisY = 500;

        //小球是否开始拖动的标志
        private bool Ismove = false;

        private bool IsDelete = false;

        //存放占空比曲线小球对象的列表
        private List<Ellipse> DutyCycleDotList = new List<Ellipse>();

        //存放占空比曲线的线段的列表
        private List<Line> DutyCycleLineList = new List<Line>();

        /// <summary>
        /// 转换Canvas坐标
        /// </summary>
        /// <param value="坐标轴的刻度"></param>
        /// <returns></returns>
        private double TransFromX(double value)
        {
            return (double)(((decimal)value / 10) * (decimal)(CanvasInPath.Width) / 10 - ((decimal)15 / 2));
        }
        private double TransFromY(double value)
        {
            return (double)(((decimal)value / 10) * (decimal)(CanvasInPath.Height) / 10 - ((decimal)15 / 2));
        }

        //小球的拖动事件
        private void Uic_MouseMove(object sender, MouseEventArgs e)
        {
            if (Ismove == true)
            {
                double xpos = (double)e.GetPosition(CanvasInPath).X;
                double ypos = (double)e.GetPosition(CanvasInPath).Y;
                double currentPointx = Convert.ToDouble((sender as Ellipse).GetValue(Canvas.LeftProperty));
                double currentPointy = Convert.ToDouble((sender as Ellipse).GetValue(Canvas.TopProperty));

                List<Ellipse> DotList = new List<Ellipse>();
                List<Line> LineList = new List<Line>();

                DotList = DutyCycleDotList;
                LineList = DutyCycleLineList;

                int index = GetIndexOfDot((sender as Ellipse), DotList);
                if (index == -1)
                    return;

                //非首尾点
                if (index != 0 && index != DotList.Count - 1)
                {
                    double prePointX = Convert.ToDouble(DotList[index - 1].GetValue(Canvas.LeftProperty)) + XOffset;
                    double prePointY = Convert.ToDouble(DotList[index - 1].GetValue(Canvas.TopProperty)) + YOffset;
                    double nextPointX = Convert.ToDouble(DotList[index + 1].GetValue(Canvas.LeftProperty)) + XOffset;
                    double nextPointY = Convert.ToDouble(DotList[index + 1].GetValue(Canvas.TopProperty)) + YOffset;

                    //区域限制
                    if ((xpos <= prePointX || xpos >= nextPointX) && (ypos > prePointY && ypos < nextPointY))
                    {
                        Canvas.SetTop((UIElement)sender, ypos - YOffset);
                        UpdateLineAndDot(DotList, LineList);
                    }
                    else if ((ypos <= prePointY || ypos >= nextPointY) && (xpos > prePointX && xpos < nextPointX))
                    {
                        Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                        UpdateLineAndDot(DotList, LineList);
                    }
                    else if ((xpos > prePointX && xpos < nextPointX) && (ypos > prePointY && ypos < nextPointY))
                    {
                        Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                        Canvas.SetTop((UIElement)sender, ypos - YOffset);
                        UpdateLineAndDot(DotList, LineList);
                    }
                    else
                    {
                        return;
                    }
                }
                //首尾点
                else
                {
                    double MaxX = CanvasInPath.Width;
                    double MaxY = CanvasInPath.Height;

                    if (index == 0)
                    {
                        double nextPointX = Convert.ToDouble(DotList[index + 1].GetValue(Canvas.LeftProperty)) + XOffset;
                        double nextPointY = Convert.ToDouble(DotList[index + 1].GetValue(Canvas.TopProperty)) + YOffset;

                        //四周边界判断
                        if ((xpos <= 0 || xpos >= nextPointX) && (ypos > 0 && ypos < nextPointY))
                        {
                            Canvas.SetTop((UIElement)sender, ypos - YOffset);
                            UpdateLineAndDot(DotList, LineList);

                            if (ypos - YOffset <= 0)
                            {

                            }
                            else
                            {

                            }
                        }
                        else if ((ypos <= 0 || ypos >= nextPointY) && (xpos > 0 && xpos < nextPointX))
                        {
                            Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                            UpdateLineAndDot(DotList, LineList);

                            if (xpos - XOffset <= 0)
                            {

                            }
                            else
                            {

                            }
                        }
                        else if ((xpos > 0 && xpos < nextPointX) && (ypos > 0 && ypos < nextPointY))
                        {
                            Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                            Canvas.SetTop((UIElement)sender, ypos - YOffset);
                            UpdateLineAndDot(DotList, LineList);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        double prePointX = Convert.ToDouble(DotList[index - 1].GetValue(Canvas.LeftProperty)) + XOffset;
                        double prePointY = Convert.ToDouble(DotList[index - 1].GetValue(Canvas.TopProperty)) + YOffset;

                        //四周边界判断
                        if ((xpos <= prePointX || xpos >= MaxX) && (ypos > prePointY && ypos < MaxY))
                        {
                            Canvas.SetTop((UIElement)sender, ypos - YOffset);
                            UpdateLineAndDot(DotList, LineList);

                            if (ypos + YOffset >= MaxY)
                            {

                            }
                            else
                            {

                            }
                        }
                        else if ((ypos <= prePointY || ypos >= MaxY) && (xpos > prePointX && xpos < MaxX))
                        {
                            Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                            UpdateLineAndDot(DotList, LineList);

                            if (xpos + XOffset >= MaxX)
                            {

                            }
                            else
                            {

                            }
                        }
                        else if ((xpos > prePointX && xpos < MaxX) && (ypos > prePointY && ypos < MaxY))
                        {
                            Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                            Canvas.SetTop((UIElement)sender, ypos - YOffset);
                            UpdateLineAndDot(DotList, LineList);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }

        //小球拖动事件开始,更改标志
        private void Uic_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //DoubleClick结束后会触发MouseDown,判断鼠标点击次数决定是否是移动需求,避免删除后鼠标移到小球可以直接移动
            if (e.ClickCount != 2)
                Ismove = true;
            else
                Ismove = false;
        }

        //小球拖动事件结束,更改标志
        private void Uic_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Ismove = false;

            //防止鼠标拖动超快导致小球位置更新线段未更新到位,除Move时更新以外结束时在更新一次
            UpdateLineAndDot(DutyCycleDotList, DutyCycleLineList);
        }

        private int GetIndexOfDot(Ellipse dot, List<Ellipse> DotList)
        {
            if (dot == null)
                return -1;
            for (int i = 0; i < DotList.Count; ++i)
            {
                if (dot == DotList[i])
                    return i;
            }
            return -1;
        }

        private void UpdateLineAndDot(List<Ellipse> DotList, List<Line> LineList)
        {
            foreach (Line each in LineList)
            {
                if (each == null)
                    return;
            }

            for (int index = 0; index < DotList.Count; ++index)
            {
                double xpos = Convert.ToDouble(DotList[index].GetValue(Canvas.LeftProperty)) + XOffset;
                double ypos = Convert.ToDouble(DotList[index].GetValue(Canvas.TopProperty)) + YOffset;

                if (index == 0)
                {
                    LineList[index].Y1 = ypos;
                }
                LineList[index].X2 = xpos;
                LineList[index].Y2 = ypos;

                LineList[index + 1].X1 = xpos;
                LineList[index + 1].Y1 = ypos;

                if (index != (DotList.Count - 1))
                {
                    LineList[index + 1].X2 = Convert.ToDouble(DotList[index + 1].GetValue(Canvas.LeftProperty)) + XOffset;
                    LineList[index + 1].Y2 = Convert.ToDouble(DotList[index + 1].GetValue(Canvas.TopProperty)) + YOffset;
                }
                else
                {
                    LineList[index + 1].X2 = MaxCoordinateAxisX;
                    LineList[index + 1].Y2 = ypos;
                }
            }
        }

        //曲线清除
        private void ClearCurve(List<Ellipse> DotList, List<Line> LineList)
        {
            for (int i = 0; i < DotList.Count; ++i)
            {
                CanvasInPath.Children.Remove(DotList[i]);
            }
            for (int i = 0; i < LineList.Count; ++i)
            {
                CanvasInPath.Children.Remove(LineList[i]);
            }

            DotList.Clear();
            LineList.Clear();
        }
        private void InitDutyCycleDotAndLine()
        {
            AddDefaultDutyCycleDotAndLine();
            UpdateLineAndDot(DutyCycleDotList, DutyCycleLineList);
        }
        private void AddDefaultDutyCycleDotAndLine()
        {
            SetDotPosition(DutyCycleCurveDot1, 10, 10);
            SetDotPosition(DutyCycleCurveDot2, 50, 70);
            DutyCycleDotList.Add(DutyCycleCurveDot1);
            DutyCycleDotList.Add(DutyCycleCurveDot2);

            Line line1 = CloneLine(DutyCycleline1, new SolidColorBrush(Colors.Orange));   //(Line)CloneObject(DutyCycleline1);
            Line line2 = CloneLine(DutyCycleline2, new SolidColorBrush(Colors.Orange));   //(Line)CloneObject(DutyCycleline2);
            Line line3 = CloneLine(DutyCycleline3, new SolidColorBrush(Colors.Orange));   //(Line)CloneObject(DutyCycleline3);
            DutyCycleLineList.Add(line1);
            DutyCycleLineList.Add(line2);
            DutyCycleLineList.Add(line3);
        }

        /// <summary>
        /// 设置点的位置
        /// </summary>
        /// <param dot="小球对象"></param>
        /// <param x="x轴刻度"></param>
        /// <param y="y轴刻度"></param>
        private void SetDotPosition(Ellipse dot, double x, double y)
        {
            double xpoint = TransFromX(x);
            double ypoint = TransFromY(y);
            Canvas.SetLeft(dot, xpoint);
            Canvas.SetTop(dot, ypoint);
        }
        private Line CloneLine(Line Line, SolidColorBrush brush)
        {
            Line line = new Line();
            CanvasInPath.Children.Add(line);
            line.Stroke = brush;
            line.StrokeThickness = 3;
            line.X1 = Line.X1;
            line.Y1 = Line.Y1;
            line.X2 = Line.X2;
            line.Y2 = Line.Y2;
            return line;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DrawAxisAndText();
            InitDutyCycleDotAndLine();
        }

        /// <summary>
        /// 绘制坐标轴和刻度
        /// </summary>
        private void DrawAxisAndText()
        {
            for (int i = 0; i < 10; ++i)
            {
                //坐标线
                Line lineX = new Line()
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeDashArray = new DoubleCollection(6),
                    StrokeThickness = 1,
                };
                Canvas.SetZIndex(lineX, 0);
                Line lineY = new Line()
                {
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeDashArray = new DoubleCollection(6),
                    StrokeThickness = 1,
                };
                Canvas.SetZIndex(lineY, 0);
                lineX.X1 = (double)((decimal)CanvasInPath.Width / 10) * i;
                lineX.X2 = (double)((decimal)CanvasInPath.Width / 10) * i;
                lineX.Y1 = 0;
                lineX.Y2 = CanvasInPath.Height;

                lineY.X1 = 0;
                lineY.X2 = CanvasInPath.Width;
                lineY.Y1 = (double)((decimal)CanvasInPath.Height / 10) * i;
                lineY.Y2 = (double)((decimal)CanvasInPath.Height / 10) * i;
                CanvasInPath.Children.Add(lineX);
                CanvasInPath.Children.Add(lineY);

                //刻度
                if (i < 9)
                {
                    TextBlock xblock = new TextBlock();
                    xblock.Foreground = new SolidColorBrush(Colors.Black);
                    xblock.FontSize = 10;
                    TranslateTransform translateTransform = new TranslateTransform(0, xblock.ActualHeight);
                    ScaleTransform scaleTransform = new ScaleTransform();
                    scaleTransform.ScaleY = -1;
                    TransformGroup transformGroup = new TransformGroup();
                    transformGroup.Children.Add(translateTransform);
                    transformGroup.Children.Add(scaleTransform);
                    xblock.RenderTransform = transformGroup;

                    xblock.Text = (i + 1) * 10 + "%";
                    Canvas.SetLeft(xblock, TransFromX((i + 1) * 10));
                    Canvas.SetTop(xblock, 15);
                    CanvasInPath.Children.Add(xblock);
                    Canvas.SetZIndex(xblock, 1);

                    TextBlock yblock = new TextBlock();
                    yblock.Foreground = new SolidColorBrush(Colors.Black);
                    yblock.FontSize = 10;
                    translateTransform = new TranslateTransform(0, yblock.ActualHeight);
                    scaleTransform = new ScaleTransform();
                    scaleTransform.ScaleY = -1;
                    transformGroup = new TransformGroup();
                    transformGroup.Children.Add(translateTransform);
                    transformGroup.Children.Add(scaleTransform);
                    yblock.RenderTransform = transformGroup;

                    yblock.Text = (i + 1) * 10 + "%";
                    Canvas.SetLeft(yblock, 5);
                    Canvas.SetTop(yblock, TransFromY((i + 1) * 10));
                    CanvasInPath.Children.Add(yblock);
                    Canvas.SetZIndex(yblock, 1);
                }
            }
        }

        /// <summary>
        /// 尺寸改变,重绘
        /// </summary>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.Width != 500 || this.Height != 600)
            {
                double x1 = (double)((decimal)(Canvas.GetLeft(DutyCycleCurveDot1) + XOffset) / (decimal)(CanvasInPath.Width / 100));
                double y1 = (double)((decimal)(Canvas.GetTop(DutyCycleCurveDot1) + YOffset) / (decimal)(CanvasInPath.Height / 100));
                double x2 = (double)((decimal)(Canvas.GetLeft(DutyCycleCurveDot2) + XOffset) / (decimal)(CanvasInPath.Width / 100));
                double y2 = (double)((decimal)(Canvas.GetTop(DutyCycleCurveDot2) + YOffset) / (decimal)(CanvasInPath.Height / 100));

                CanvasInPath.Children.Clear();
                CanvasInPath.Height = this.ActualHeight - 100;
                CanvasInPath.Width = this.ActualWidth;
                MaxCoordinateAxisX = this.ActualWidth;
                MaxCoordinateAxisY = this.ActualHeight - 100;
                CanvasInPath.Children.Add(DutyCycleCurveDot1);
                CanvasInPath.Children.Add(DutyCycleCurveDot2);
                CanvasInPath.Children.Add(DutyCycleLineList[0]);
                CanvasInPath.Children.Add(DutyCycleLineList[1]);
                CanvasInPath.Children.Add(DutyCycleLineList[2]);
                DrawAxisAndText();
                SetDotPosition(DutyCycleCurveDot1, x1, y1);
                SetDotPosition(DutyCycleCurveDot2, x2, y2);
                UpdateLineAndDot(DutyCycleDotList, DutyCycleLineList);
            }
        }
    }
}
