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

namespace CurveEdit
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        CurveEditViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new CurveEditViewModel();
            this.DataContext = viewModel;
        }

        #region 变量
        //小球是否开始拖动的标志
        private bool Ismove = false;

        private bool IsDelete = false;

        //设置小球坐标时的偏移量
        private double XOffset = 7.5;//小球的长度的1/2

        private double YOffset = 7.5;//小球的宽度的1/2

        //X轴刻度Max值
        private double MaxCoordinateAxisX = 500;

        //Y轴刻度Max值
        private double MaxCoordinateAxisY = 500;

        //修改前的坐标轴数据(Speed和Ratip共用)
        private string PreData = "";

        //修改前小球的索引
        private int PreIndex = 0;

        //存放占空比曲线小球对象的列表
        private List<PowerCurveDot> DutyCycleDotList = new List<PowerCurveDot>();

        //存放占空比曲线的线段的列表
        private List<Line> DutyCycleLineList = new List<Line>();

        //存放频率曲线小球对象的列表
        private List<PowerCurveDot> FrequencyDotList = new List<PowerCurveDot>();

        //存放频率曲线的线段的列表
        private List<Line> FrequencyLineList = new List<Line>();
        #endregion

        #region 初始化小球和线段
        private void InitDutyCycleDotAndLine()
        {
            DutyCycleLineList.Add(DutyCycleline1);
            DutyCycleLineList.Add(DutyCycleline2);
            DutyCycleLineList.Add(DutyCycleline3);

            DutyCycleDotList.Add(DutyCycleCurveDot1);
            DutyCycleDotList.Add(DutyCycleCurveDot2);

            //创建viewModel中的数据
            foreach (PowerCurveDot each in DutyCycleDotList)
            {
                CurveEditViewModel.CurveDot dot = new CurveEditViewModel.CurveDot();
                dot.Speed = ((Convert.ToDouble(each.GetValue(Canvas.LeftProperty)) + XOffset) / 5).ToString();
                dot.Dutycycle = ((Convert.ToDouble(each.GetValue(Canvas.TopProperty)) + YOffset) / 5).ToString();

                if (viewModel.DutyCycleCurveDotList == null)
                    viewModel.DutyCycleCurveDotList = new System.Collections.ObjectModel.ObservableCollection<CurveEditViewModel.CurveDot>();
                viewModel.DutyCycleCurveDotList.Add(dot);
            }

            UpdateDataGridIndex();
        }

        private void InitFrequencyDotAndLine()
        {
            FrequencyLineList.Add(Frequencyline1);
            FrequencyLineList.Add(Frequencyline2);
            FrequencyLineList.Add(Frequencyline3);

            FrequencyDotList.Add(FrequencyCurveDot1);
            FrequencyDotList.Add(FrequencyCurveDot2);

            foreach (PowerCurveDot each in FrequencyDotList)
            {
                CurveEditViewModel.CurveDot dot = new CurveEditViewModel.CurveDot();
                dot.Speed = ((Convert.ToDouble(each.GetValue(Canvas.LeftProperty)) + XOffset) / 5).ToString();
                dot.Dutycycle = ((Convert.ToDouble(each.GetValue(Canvas.TopProperty)) + YOffset) / 5).ToString();

                if (viewModel.FrequencyCurveDotList == null)
                    viewModel.FrequencyCurveDotList = new System.Collections.ObjectModel.ObservableCollection<CurveEditViewModel.CurveDot>();
                viewModel.FrequencyCurveDotList.Add(dot);
            }

            UpdateDataGridIndex();
        }
        #endregion

        #region 按钮操作
        //单击标题栏启用或禁用控件
        private void TitleGrid_Click(object sender, MouseButtonEventArgs e)
        {
            EditStatus.IsChecked = !EditStatus.IsChecked;
            ChangeLineAndDotColor();
        }

        //曲线重置
        private void ResetCurve(List<PowerCurveDot> DotList, List<Line> LineList)
        {
            //清空曲线的点和线
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

            //清除ViewModel的数据并重新初始化
            if (FrequencyLineSatus.IsChecked == false)
            {
                viewModel.DutyCycleCurveDotList.Clear();
                InitDutyCycleDotAndLine();
            }
            else
            {
                viewModel.FrequencyCurveDotList.Clear();
                InitFrequencyDotAndLine();
            }

            //将线和点重新加入画布
            for (int i = 0; i < LineList.Count; ++i)
            {
                CanvasInPath.Children.Add(LineList[i]);
            }

            for (int i = 0; i < DotList.Count; ++i)
            {
                CanvasInPath.Children.Add(DotList[i]);
            }

            //更新点和线的位置
            UpdateLineAndDot(DotList, LineList);
            AdjustLineAndDot();
        }

        //曲线清除
        private void ClearCurve(List<PowerCurveDot> DotList, List<Line> LineList)
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

        //添加按钮
        private void AddDotbtn_Click(object sender, RoutedEventArgs e)
        {
            //插入到占空比曲线中
            if (FrequencyLineSatus.IsChecked == false)
            {
                //默认插入到倒数第二的位置
                double xpos = (Convert.ToDouble(DutyCycleDotList[DutyCycleDotList.Count - 1].GetValue(Canvas.LeftProperty))
                    + Convert.ToDouble(DutyCycleDotList[DutyCycleDotList.Count - 2].GetValue(Canvas.LeftProperty))) / 2;
                double ypos = (Convert.ToDouble(DutyCycleDotList[DutyCycleDotList.Count - 1].GetValue(Canvas.TopProperty))
                    + Convert.ToDouble(DutyCycleDotList[DutyCycleDotList.Count - 2].GetValue(Canvas.TopProperty))) / 2;
                int DotCount = DutyCycleDotList.Count;

                PowerCurveDot dot = new PowerCurveDot();
                CanvasInPath.Children.Add(dot);
                Canvas.SetLeft(dot, xpos);
                Canvas.SetTop(dot, ypos);
                dot.PreviewMouseMove += Uic_MouseMove;
                dot.PreviewMouseDown += Uic_MouseDown;
                dot.PreviewMouseUp += Uic_MouseLeftButtonUp;
                DutyCycleDotList.Insert(DutyCycleDotList.Count - 1, dot);

                Line line = new Line();
                CanvasInPath.Children.Add(line);
                line.Stroke = new SolidColorBrush(Colors.Orange);
                line.StrokeThickness = 3;

                //前面插入点之后,DotList的Count变化了
                DutyCycleLineList.Insert(DotCount - 1, line);

                CurveEditViewModel.CurveDot dutyCycleCurveDot = new CurveEditViewModel.CurveDot();
                dutyCycleCurveDot.Speed = (xpos / 5).ToString();
                dutyCycleCurveDot.Dutycycle = (ypos / 5).ToString();
                dutyCycleCurveDot.Index = DotCount - 1;
                viewModel.DutyCycleCurveDotList.Insert(DotCount - 1, dutyCycleCurveDot);

                UpdateLineAndDot(DutyCycleDotList, DutyCycleLineList);
            }
            //插入到频率曲线中
            else
            {
                //默认插入到倒数第二的位置
                double xpos = (Convert.ToDouble(FrequencyDotList[FrequencyDotList.Count - 1].GetValue(Canvas.LeftProperty))
                    + Convert.ToDouble(FrequencyDotList[FrequencyDotList.Count - 2].GetValue(Canvas.LeftProperty))) / 2;
                double ypos = (Convert.ToDouble(FrequencyDotList[FrequencyDotList.Count - 1].GetValue(Canvas.TopProperty))
                    + Convert.ToDouble(FrequencyDotList[FrequencyDotList.Count - 2].GetValue(Canvas.TopProperty))) / 2;
                int DotCount = FrequencyDotList.Count;

                PowerCurveDot dot = new PowerCurveDot();
                dot.ui.Tag = "Blue";
                CanvasInPath.Children.Add(dot);
                Canvas.SetLeft(dot, xpos);
                Canvas.SetTop(dot, ypos);
                dot.PreviewMouseMove += Uic_MouseMove;
                dot.PreviewMouseDown += Uic_MouseDown;
                dot.PreviewMouseUp += Uic_MouseLeftButtonUp;
                FrequencyDotList.Insert(FrequencyDotList.Count - 1, dot);

                Line line = new Line();
                CanvasInPath.Children.Add(line);
                line.Stroke = new SolidColorBrush(Colors.Blue);
                line.StrokeThickness = 3;
                //设置层级,防止复制到占空比操作导致频率曲线被遮住
                Canvas.SetZIndex(line, 1);

                //前面插入点之后,DotList的Count变化了
                FrequencyLineList.Insert(DotCount - 1, line);

                CurveEditViewModel.CurveDot frequencyCurveDot = new CurveEditViewModel.CurveDot();
                frequencyCurveDot.Speed = (xpos / 5).ToString();
                frequencyCurveDot.Dutycycle = (ypos / 5).ToString();
                frequencyCurveDot.Index = DotCount - 1;
                viewModel.FrequencyCurveDotList.Insert(DotCount - 1, frequencyCurveDot);

                UpdateLineAndDot(FrequencyDotList, FrequencyLineList);
            }

            UpdateDataGridIndex();
        }

        //重置按钮
        private void Resetbtn_Click(object sender, RoutedEventArgs e)
        {
            if (FrequencyLineSatus.IsChecked == false)
            {
                ResetCurve(DutyCycleDotList, DutyCycleLineList);
            }
            else
            {
                ResetCurve(FrequencyDotList, FrequencyLineList);
            }
        }

        //复制到占空比
        private void CopyToDutyCyclebtn_Click(object sender, RoutedEventArgs e)
        {
            ClearCurve(DutyCycleDotList, DutyCycleLineList);
            viewModel.DutyCycleCurveDotList.Clear();

            for (int i = 0; i < FrequencyDotList.Count; ++i)
            {
                PowerCurveDot dot = new PowerCurveDot();
                CanvasInPath.Children.Add(dot);
                Canvas.SetLeft(dot, Convert.ToDouble(FrequencyDotList[i].GetValue(Canvas.LeftProperty)));
                Canvas.SetTop(dot, Convert.ToDouble(FrequencyDotList[i].GetValue(Canvas.TopProperty)));
                dot.PreviewMouseMove += Uic_MouseMove;
                dot.PreviewMouseDown += Uic_MouseDown;
                dot.PreviewMouseUp += Uic_MouseLeftButtonUp;
                DutyCycleDotList.Add(dot);
            }

            for (int i = 0; i < FrequencyLineList.Count; ++i)
            {
                Line line = new Line();
                CanvasInPath.Children.Add(line);
                line.Stroke = new SolidColorBrush(Colors.Orange);
                line.StrokeThickness = 3;
                line.X1 = FrequencyLineList[i].X1;
                line.Y1 = FrequencyLineList[i].Y1;
                line.X2 = FrequencyLineList[i].X2;
                line.Y2 = FrequencyLineList[i].Y2;
                DutyCycleLineList.Add(line);
            }

            for (int i = 0; i < viewModel.FrequencyCurveDotList.Count; ++i)
            {
                CurveEditViewModel.CurveDot dot = new CurveEditViewModel.CurveDot();
                dot.Index = viewModel.FrequencyCurveDotList[i].Index;
                dot.Speed = viewModel.FrequencyCurveDotList[i].Speed;
                dot.Dutycycle = viewModel.FrequencyCurveDotList[i].Dutycycle;
                viewModel.DutyCycleCurveDotList.Add(dot);
            }

            //调整占空比曲线的线段和点的颜色
            for (int i = 0; i < DutyCycleDotList.Count; ++i)
            {
                DutyCycleDotList[i].Visibility = Visibility.Collapsed;
            }
            for (int i = 0; i < DutyCycleLineList.Count; ++i)
            {
                DutyCycleLineList[i].Stroke = new SolidColorBrush(Color.FromRgb(89, 89, 89));
            }

            UpdateLineAndDot(DutyCycleDotList, DutyCycleLineList);
        }
        #endregion

        #region DataGrid的键盘输入修改小球坐标
        //恢复之前的Speed数据
        private void SpeedCol_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtbox = sender as TextBox;

            double nowvalue = 0;

            try
            {
                nowvalue = Convert.ToDouble(txtbox.Text.Trim()) * 5 - XOffset;
            }
            catch (Exception error)
            {

            }

            List<PowerCurveDot> DotList = new List<PowerCurveDot>();
            List<Line> LineList = new List<Line>();

            //区分操作的小球对象
            if (FrequencyLineSatus.IsChecked == false)
            {
                DotList = DutyCycleDotList;
                LineList = DutyCycleLineList;
            }
            else
            {
                DotList = FrequencyDotList;
                LineList = FrequencyLineList;
            }

            //恢复之前的speed数据
            if (PreIndex == 0)
            {
                double nextpoint = Convert.ToDouble(DotList[PreIndex + 1].GetValue(Canvas.LeftProperty));
                if (nowvalue > nextpoint || nowvalue < 0)
                {
                    txtbox.Text = PreData;
                }
            }
            else if (PreIndex == DotList.Count - 1)
            {
                double prepoint = Convert.ToDouble(DotList[PreIndex - 1].GetValue(Canvas.LeftProperty));
                if (nowvalue < prepoint || nowvalue > MaxCoordinateAxisX)
                {
                    txtbox.Text = PreData;
                }
            }
            else
            {
                if (nowvalue < Convert.ToDouble(DotList[PreIndex - 1].GetValue(Canvas.LeftProperty))
                    || nowvalue > Convert.ToDouble(DotList[PreIndex + 1].GetValue(Canvas.LeftProperty)))
                {
                    txtbox.Text = PreData;
                }
            }
        }

        //通过输入更改小球Speed
        private void SpeedCol_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtbox = sender as TextBox;

            if (txtbox.IsFocused == true)
            {
                txtbox.SelectAll();

                if (txtbox.Text.Trim() == "")
                    return;

                double newvalue = 0;

                try
                {
                    newvalue = Convert.ToDouble(txtbox.Text.Trim()) * 5 - XOffset;
                }
                catch (Exception error)
                {
                    //输入错误
                    return;
                }

                if (newvalue <= 0)
                    newvalue = -1 * XOffset;
                if (newvalue >= MaxCoordinateAxisX)
                    newvalue = MaxCoordinateAxisX - XOffset;

                CurveEditViewModel.CurveDot dot = DataGrid.SelectedItem as CurveEditViewModel.CurveDot;

                int index = dot.Index;

                List<PowerCurveDot> DotList = new List<PowerCurveDot>();
                List<Line> LineList = new List<Line>();

                //区分操作的小球对象
                if (FrequencyLineSatus.IsChecked == false)
                {
                    DotList = DutyCycleDotList;
                    LineList = DutyCycleLineList;
                }
                else
                {
                    DotList = FrequencyDotList;
                    LineList = FrequencyLineList;
                }

                //更新小球位置
                if (index == 0)
                {
                    double nextpoint = Convert.ToDouble(DotList[index + 1].GetValue(Canvas.LeftProperty));
                    if (newvalue > nextpoint)
                    {
                        return;
                    }
                }
                else if (index == DotList.Count - 1)
                {
                    double prepoint = Convert.ToDouble(DotList[index - 1].GetValue(Canvas.LeftProperty));
                    if (newvalue < prepoint)
                    {
                        return;
                    }
                }
                else
                {
                    if (newvalue < Convert.ToDouble(DotList[index - 1].GetValue(Canvas.LeftProperty))
                        || newvalue > Convert.ToDouble(DotList[index + 1].GetValue(Canvas.LeftProperty)))
                    {
                        return;
                    }
                }

                //保存speed数据
                PreIndex = index;
                PreData = ((newvalue + XOffset) / 5).ToString();

                Canvas.SetLeft(DotList[index], newvalue);
                UpdateLineAndDot(DotList, LineList);

                //更新viewModel的数据
                if (FrequencyLineSatus.IsChecked == false)
                    viewModel.DutyCycleCurveDotList[index].Speed = PreData;
                else
                    viewModel.FrequencyCurveDotList[index].Speed = PreData;
            }
            //无焦点说明时拖动小球导致的数据改变,不做处理
            else
            {
                return;
            }
        }

        //恢复之前的Ratip数据
        private void RatipCol_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtbox = sender as TextBox;

            double nowvalue = 0;

            try
            {
                nowvalue = Convert.ToDouble(txtbox.Text.Trim()) * 5 - XOffset;
            }
            catch (Exception error)
            {

            }

            List<PowerCurveDot> DotList = new List<PowerCurveDot>();
            List<Line> LineList = new List<Line>();

            //区分操作的小球对象
            if (FrequencyLineSatus.IsChecked == false)
            {
                DotList = DutyCycleDotList;
                LineList = DutyCycleLineList;
            }
            else
            {
                DotList = FrequencyDotList;
                LineList = FrequencyLineList;
            }

            //恢复之前的speed数据
            if (PreIndex == 0)
            {
                double nextpoint = Convert.ToDouble(DotList[PreIndex + 1].GetValue(Canvas.TopProperty));
                if (nowvalue > nextpoint || nowvalue < 0)
                {
                    txtbox.Text = PreData;
                }
            }
            else if (PreIndex == DotList.Count - 1)
            {
                double prepoint = Convert.ToDouble(DotList[PreIndex - 1].GetValue(Canvas.TopProperty));
                if (nowvalue < prepoint || nowvalue > MaxCoordinateAxisY)
                {
                    txtbox.Text = PreData;
                }
            }
            else
            {
                if (nowvalue < Convert.ToDouble(DotList[PreIndex - 1].GetValue(Canvas.TopProperty))
                    || nowvalue > Convert.ToDouble(DotList[PreIndex + 1].GetValue(Canvas.TopProperty)))
                {
                    txtbox.Text = PreData;
                }
            }
        }

        //通过输入更改小球Ratip
        private void RatipCol_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtbox = sender as TextBox;

            if (txtbox.IsFocused == true)
            {
                txtbox.SelectAll();

                if (txtbox.Text.Trim() == "")
                    return;

                double newvalue = 0;

                try
                {
                    newvalue = Convert.ToDouble(txtbox.Text.Trim()) * 5 - YOffset;
                }
                catch (Exception error)
                {
                    //输入错误
                    return;
                }

                if (newvalue <= 0)
                    newvalue = -1 * YOffset;
                if (newvalue >= MaxCoordinateAxisY)
                    newvalue = MaxCoordinateAxisY - YOffset;

                CurveEditViewModel.CurveDot dot = DataGrid.SelectedItem as CurveEditViewModel.CurveDot;

                int index = dot.Index;

                List<PowerCurveDot> DotList = new List<PowerCurveDot>();
                List<Line> LineList = new List<Line>();

                //区分操作的小球对象
                if (FrequencyLineSatus.IsChecked == false)
                {
                    DotList = DutyCycleDotList;
                    LineList = DutyCycleLineList;
                }
                else
                {
                    DotList = FrequencyDotList;
                    LineList = FrequencyLineList;
                }

                //更新小球位置
                if (index == 0)
                {
                    double nextpoint = Convert.ToDouble(DotList[index + 1].GetValue(Canvas.TopProperty));
                    if (newvalue > nextpoint)
                    {
                        return;
                    }
                }
                else if (index == DotList.Count - 1)
                {
                    double prepoint = Convert.ToDouble(DotList[index - 1].GetValue(Canvas.TopProperty));
                    if (newvalue < prepoint)
                    {
                        return;
                    }
                }
                else
                {
                    if (newvalue < Convert.ToDouble(DotList[index - 1].GetValue(Canvas.TopProperty))
                        || newvalue > Convert.ToDouble(DotList[index + 1].GetValue(Canvas.TopProperty)))
                    {
                        return;
                    }
                }

                //保存ratip数据
                PreIndex = index;
                PreData = ((newvalue + YOffset) / 5).ToString();

                Canvas.SetTop(DotList[index], newvalue);
                UpdateLineAndDot(DotList, LineList);

                //更新viewModel的数据
                if (FrequencyLineSatus.IsChecked == false)
                    viewModel.DutyCycleCurveDotList[index].Dutycycle = PreData;
                else
                    viewModel.FrequencyCurveDotList[index].Dutycycle = PreData;
            }
            //无焦点说明时拖动小球导致的数据改变,不做处理
            else
            {
                return;
            }
        }
        #endregion

        #region 对小球的相关操作
        //双击生成小球事件
        private void CanvasInPath_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsDelete == true)
                    return;

                double currentPointx = e.GetPosition(CanvasInPath).X;
                double currentPointy = e.GetPosition(CanvasInPath).Y;

                if (FrequencyLineSatus.IsChecked == false)
                {
                    if (currentPointx < Convert.ToDouble(DutyCycleCurveDot1.GetValue(Canvas.LeftProperty)) + XOffset
                    || currentPointx > Convert.ToDouble(DutyCycleCurveDot2.GetValue(Canvas.LeftProperty)) + XOffset)
                    {
                        return;
                    }
                }
                else
                {
                    if (currentPointx < Convert.ToDouble(FrequencyCurveDot1.GetValue(Canvas.LeftProperty)) + YOffset
                    || currentPointx > Convert.ToDouble(FrequencyCurveDot2.GetValue(Canvas.LeftProperty)) + YOffset)
                    {
                        return;
                    }
                }

                //生成小球
                PowerCurveDot dot = new PowerCurveDot();
                CanvasInPath.Children.Add(dot);
                Canvas.SetLeft(dot, currentPointx - XOffset);   //Y轴坐标通过之后的斜率计算得出
                Canvas.SetZIndex(dot, 100);
                dot.PreviewMouseMove += Uic_MouseMove;
                dot.PreviewMouseDown += Uic_MouseDown;
                dot.MouseEnter += Uic_MouseEnter;
                dot.MouseLeave += Uic_MouseLeave;
                dot.PreviewMouseUp += Uic_MouseLeftButtonUp;
                dot.PreviewMouseDoubleClick += Uic_PreviewMouseDoubleClick;

                //将新生成的小球放入鼠标左右小球的中间
                int insertindex = 0;

                //插入到占空比曲线中
                if (FrequencyLineSatus.IsChecked == false)
                {
                    for (int i = 0; i < DutyCycleDotList.Count - 1; ++i)
                    {
                        if (currentPointx >= Convert.ToDouble(DutyCycleDotList[i].GetValue(Canvas.LeftProperty)) + XOffset
                            && currentPointx <= Convert.ToDouble(DutyCycleDotList[i + 1].GetValue(Canvas.LeftProperty)) + XOffset)
                        {
                            insertindex = i + 1;
                            currentPointy = GetYAxis(DutyCycleDotList[i], DutyCycleDotList[i + 1], currentPointx);

                            if (currentPointy == -1)
                                return;

                            Canvas.SetTop(dot, currentPointy - YOffset);
                        }
                    }
                    if (Double.IsNaN(Convert.ToDouble(dot.GetValue(Canvas.TopProperty))) == true)
                    {
                        CanvasInPath.Children.Remove(dot);
                        return;
                    }

                    DutyCycleDotList.Insert(insertindex, dot);

                    //将点的数据添加到viewModel中
                    CurveEditViewModel.CurveDot dutyCycleCurveDot = new CurveEditViewModel.CurveDot();
                    dutyCycleCurveDot.Speed = (currentPointx / 5).ToString();
                    dutyCycleCurveDot.Dutycycle = (currentPointy / 5).ToString();
                    dutyCycleCurveDot.Index = insertindex;
                    viewModel.DutyCycleCurveDotList.Insert(insertindex, dutyCycleCurveDot);

                    //生成线段
                    Line line = new Line();
                    CanvasInPath.Children.Add(line);
                    line.Stroke = new SolidColorBrush(Colors.Orange);
                    line.StrokeThickness = 3;
                    DutyCycleLineList.Insert(insertindex + 1, line);

                    UpdateLineAndDot(DutyCycleDotList, DutyCycleLineList);
                }
                //插入到频率曲线中
                else
                {
                    //修改小球的颜色
                    dot.ui.Tag = "Blue";

                    for (int i = 0; i < FrequencyDotList.Count - 1; ++i)
                    {
                        if (currentPointx >= Convert.ToDouble(FrequencyDotList[i].GetValue(Canvas.LeftProperty)) + XOffset
                            && currentPointx <= Convert.ToDouble(FrequencyDotList[i + 1].GetValue(Canvas.LeftProperty)) + XOffset)
                        {
                            insertindex = i + 1;
                            currentPointy = GetYAxis(FrequencyDotList[i], FrequencyDotList[i + 1], currentPointx);
                            if (currentPointy == -1)
                                return;
                            Canvas.SetTop(dot, currentPointy - YOffset);
                        }
                    }
                    FrequencyDotList.Insert(insertindex, dot);

                    //将点的数据添加到viewModel中
                    CurveEditViewModel.CurveDot frequencyCurveDot = new CurveEditViewModel.CurveDot();
                    frequencyCurveDot.Speed = (currentPointx / 5).ToString();
                    frequencyCurveDot.Dutycycle = (currentPointy / 5).ToString();
                    frequencyCurveDot.Index = insertindex;
                    viewModel.FrequencyCurveDotList.Insert(insertindex, frequencyCurveDot);

                    //生成线段
                    Line line = new Line();
                    CanvasInPath.Children.Add(line);
                    line.Stroke = new SolidColorBrush(Colors.Blue);
                    line.StrokeThickness = 3;
                    //设置层级,防止复制到占空比操作导致频率曲线被遮住
                    Canvas.SetZIndex(line, 1);
                    FrequencyLineList.Insert(insertindex + 1, line);

                    UpdateLineAndDot(FrequencyDotList, FrequencyLineList);
                }

                //更新ViewModel的索引
                UpdateDataGridIndex();
            }
        }

        //双击删除小球事件
        private void Uic_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PowerCurveDot dot = sender as PowerCurveDot;
            //将点移除
            CanvasInPath.Children.Remove(dot);

            if (FrequencyLineSatus.IsChecked == false)
            {
                int index = GetIndexOfDot(dot, DutyCycleDotList);
                DutyCycleDotList.Remove(dot);
                //将线段移除
                CanvasInPath.Children.Remove(DutyCycleLineList[index + 1]);
                DutyCycleLineList.RemoveAt(index + 1);
                viewModel.DutyCycleCurveDotList.RemoveAt(index);
                UpdateLineAndDot(DutyCycleDotList, DutyCycleLineList);
            }
            else
            {
                int index = GetIndexOfDot(dot, FrequencyDotList);
                FrequencyDotList.Remove(dot);
                //将线段移除
                CanvasInPath.Children.Remove(FrequencyLineList[index + 1]);
                FrequencyLineList.RemoveAt(index + 1);
                viewModel.FrequencyCurveDotList.RemoveAt(index);
                UpdateLineAndDot(FrequencyDotList, FrequencyLineList);
            }
            UpdateDataGridIndex();
        }

        //小球的拖动事件
        private void Uic_MouseMove(object sender, MouseEventArgs e)
        {
            if (Ismove == true)
            {
                double xpos = (double)e.GetPosition(CanvasInPath).X;
                double ypos = (double)e.GetPosition(CanvasInPath).Y;
                double currentPointx = Convert.ToDouble((sender as PowerCurveDot).GetValue(Canvas.LeftProperty));
                double currentPointy = Convert.ToDouble((sender as PowerCurveDot).GetValue(Canvas.TopProperty));

                List<PowerCurveDot> DotList = new List<PowerCurveDot>();
                List<Line> LineList = new List<Line>();

                if (FrequencyLineSatus.IsChecked == false)
                {
                    DotList = DutyCycleDotList;
                    LineList = DutyCycleLineList;
                }
                else
                {
                    DotList = FrequencyDotList;
                    LineList = FrequencyLineList;
                }

                int index = GetIndexOfDot((sender as PowerCurveDot), DotList);
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

                        if (FrequencyLineSatus.IsChecked == false)
                            viewModel.DutyCycleCurveDotList[index].Dutycycle = (ypos / 5).ToString();
                        else
                            viewModel.FrequencyCurveDotList[index].Dutycycle = (ypos / 5).ToString();
                    }
                    else if ((ypos <= prePointY || ypos >= nextPointY) && (xpos > prePointX && xpos < nextPointX))
                    {
                        Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                        UpdateLineAndDot(DotList, LineList);

                        if (FrequencyLineSatus.IsChecked == false)
                            viewModel.DutyCycleCurveDotList[index].Speed = (xpos / 5).ToString();
                        else
                            viewModel.FrequencyCurveDotList[index].Speed = (xpos / 5).ToString();
                    }
                    else if ((xpos > prePointX && xpos < nextPointX) && (ypos > prePointY && ypos < nextPointY))
                    {
                        Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                        Canvas.SetTop((UIElement)sender, ypos - YOffset);
                        UpdateLineAndDot(DotList, LineList);

                        if (FrequencyLineSatus.IsChecked == false)
                        {
                            viewModel.DutyCycleCurveDotList[index].Speed = (xpos / 5).ToString();
                            viewModel.DutyCycleCurveDotList[index].Dutycycle = (ypos / 5).ToString();
                        }
                        else
                        {
                            viewModel.FrequencyCurveDotList[index].Speed = (xpos / 5).ToString();
                            viewModel.FrequencyCurveDotList[index].Dutycycle = (ypos / 5).ToString();
                        }

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
                                if (FrequencyLineSatus.IsChecked == false)
                                    viewModel.DutyCycleCurveDotList[index].Dutycycle = "0";
                                else
                                    viewModel.FrequencyCurveDotList[index].Dutycycle = "0";
                            }
                            else
                            {
                                if (FrequencyLineSatus.IsChecked == false)
                                    viewModel.DutyCycleCurveDotList[index].Dutycycle = (ypos / 5).ToString();
                                else
                                    viewModel.FrequencyCurveDotList[index].Dutycycle = (ypos / 5).ToString();
                            }
                        }
                        else if ((ypos <= 0 || ypos >= nextPointY) && (xpos > 0 && xpos < nextPointX))
                        {
                            Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                            UpdateLineAndDot(DotList, LineList);

                            if (xpos - XOffset <= 0)
                            {
                                if (FrequencyLineSatus.IsChecked == false)
                                    viewModel.DutyCycleCurveDotList[index].Speed = "0";
                                else
                                    viewModel.FrequencyCurveDotList[index].Speed = "0";
                            }
                            else
                            {
                                if (FrequencyLineSatus.IsChecked == false)
                                    viewModel.DutyCycleCurveDotList[index].Speed = (xpos / 5).ToString();
                                viewModel.FrequencyCurveDotList[index].Speed = (xpos / 5).ToString();
                            }
                        }
                        else if ((xpos > 0 && xpos < nextPointX) && (ypos > 0 && ypos < nextPointY))
                        {
                            Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                            Canvas.SetTop((UIElement)sender, ypos - YOffset);
                            UpdateLineAndDot(DotList, LineList);

                            if (FrequencyLineSatus.IsChecked == false)
                            {
                                viewModel.DutyCycleCurveDotList[index].Speed = (xpos / 5).ToString();
                                viewModel.DutyCycleCurveDotList[index].Dutycycle = (ypos / 5).ToString();
                            }
                            else
                            {
                                viewModel.FrequencyCurveDotList[index].Speed = (xpos / 5).ToString();
                                viewModel.FrequencyCurveDotList[index].Dutycycle = (ypos / 5).ToString();
                            }
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
                                if (FrequencyLineSatus.IsChecked == false)
                                    viewModel.DutyCycleCurveDotList[index].Dutycycle = "100";
                                else
                                    viewModel.FrequencyCurveDotList[index].Dutycycle = "100";
                            }
                            else
                            {
                                if (FrequencyLineSatus.IsChecked == false)
                                    viewModel.DutyCycleCurveDotList[index].Dutycycle = (ypos / 5).ToString();
                                else
                                    viewModel.FrequencyCurveDotList[index].Dutycycle = (ypos / 5).ToString();
                            }
                        }
                        else if ((ypos <= prePointY || ypos >= MaxY) && (xpos > prePointX && xpos < MaxX))
                        {
                            Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                            UpdateLineAndDot(DotList, LineList);

                            if (xpos + XOffset >= MaxX)
                            {
                                if (FrequencyLineSatus.IsChecked == false)
                                    viewModel.DutyCycleCurveDotList[index].Speed = "100";
                                else
                                    viewModel.FrequencyCurveDotList[index].Speed = "100";
                            }
                            else
                            {
                                if (FrequencyLineSatus.IsChecked == false)
                                    viewModel.DutyCycleCurveDotList[index].Dutycycle = (xpos / 5).ToString();
                                else
                                    viewModel.FrequencyCurveDotList[index].Dutycycle = (xpos / 5).ToString();
                            }
                        }
                        else if ((xpos > prePointX && xpos < MaxX) && (ypos > prePointY && ypos < MaxY))
                        {
                            Canvas.SetLeft((UIElement)sender, xpos - XOffset);
                            Canvas.SetTop((UIElement)sender, ypos - YOffset);
                            UpdateLineAndDot(DotList, LineList);

                            if (FrequencyLineSatus.IsChecked == false)
                            {
                                viewModel.DutyCycleCurveDotList[index].Speed = (xpos / 5).ToString();
                                viewModel.DutyCycleCurveDotList[index].Dutycycle = (ypos / 5).ToString();
                            }
                            else
                            {
                                viewModel.FrequencyCurveDotList[index].Speed = (xpos / 5).ToString();
                                viewModel.FrequencyCurveDotList[index].Dutycycle = (ypos / 5).ToString();
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }

        //小球删除事件开始,更改标志
        private void Uic_MouseEnter(object sender, MouseEventArgs e)
        {
            IsDelete = true;
        }

        //小球删除事件结束,更改标志
        private void Uic_MouseLeave(object sender, MouseEventArgs e)
        {
            IsDelete = false;
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
            if (FrequencyLineSatus.IsChecked == false)
                UpdateLineAndDot(DutyCycleDotList, DutyCycleLineList);
            else
                UpdateLineAndDot(FrequencyDotList, FrequencyLineList);
        }

        //计算Y轴坐标
        private double GetYAxis(PowerCurveDot dot1, PowerCurveDot dot2, double x)
        {
            if (dot1 == null || dot2 == null)
                return -1;

            double y = 0;
            double dot1x = Convert.ToDouble(dot1.GetValue(Canvas.LeftProperty)) + XOffset;  //  这里都是坐标,并不是Canvas中的属性
            double dot1y = Convert.ToDouble(dot1.GetValue(Canvas.TopProperty)) + YOffset;   //  这里都是坐标,并不是Canvas中的属性
            double dot2x = Convert.ToDouble(dot2.GetValue(Canvas.LeftProperty)) + XOffset;  //  这里都是坐标,并不是Canvas中的属性
            double dot2y = Convert.ToDouble(dot2.GetValue(Canvas.TopProperty)) + YOffset;   //  这里都是坐标,并不是Canvas中的属性

            double k = (dot2y - dot1y) / (dot2x - dot1x);

            double b = dot2y - k * dot2x;

            return y = k * x + b;
        }
        #endregion

        #region 对线和点的相关操作
        //切换曲线
        private void FrequencyLineSatus_Click(object sender, RoutedEventArgs e)
        {
            //FrequencyLineSatus.IsChecked = !FrequencyLineSatus.IsChecked;

            AdjustLineAndDot();
        }

        //更新点和线的位置
        private async void UpdateLineAndDot(List<PowerCurveDot> DotList, List<Line> LineList)
        {
            foreach (var each in LineList)
            {
                if (each == null)
                    return;
            }

            await Dispatcher.BeginInvoke(new Action(() =>
            {
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
                        LineList[index + 1].Y2 = ypos;
                    }
                }
            }));
        }

        private void AdjustLineAndDot()
        {
            if (FrequencyLineSatus.IsChecked == false)
            {
                //隐藏频率曲线
                for (int i = 0; i < FrequencyLineList.Count; ++i)
                {
                    FrequencyLineList[i].Visibility = Visibility.Collapsed;
                }
                //隐藏频率曲线的点
                for (int i = 0; i < FrequencyDotList.Count; ++i)
                {
                    FrequencyDotList[i].Visibility = Visibility.Collapsed;
                }
                //调整占空比曲线的线段和点的颜色
                for (int i = 0; i < DutyCycleDotList.Count; ++i)
                {
                    DutyCycleDotList[i].Visibility = Visibility.Visible;
                }
                for (int i = 0; i < DutyCycleLineList.Count; ++i)
                {
                    DutyCycleLineList[i].Stroke = new SolidColorBrush(Colors.Orange);
                }
            }
            else
            {
                //显示频率曲线
                for (int i = 0; i < FrequencyLineList.Count; ++i)
                {
                    FrequencyLineList[i].Visibility = Visibility.Visible;
                    Canvas.SetZIndex(FrequencyLineList[i], 1);
                    FrequencyLineList[i].Stroke = new SolidColorBrush(Colors.Blue);
                }
                //显示频率曲线的点
                for (int i = 0; i < FrequencyDotList.Count; ++i)
                {
                    FrequencyDotList[i].Visibility = Visibility.Visible;
                    FrequencyDotList[i].ui.Tag = "Blue";
                }
                //调整占空比曲线的线段和点的颜色
                for (int i = 0; i < DutyCycleDotList.Count; ++i)
                {
                    DutyCycleDotList[i].Visibility = Visibility.Collapsed;
                }
                for (int i = 0; i < DutyCycleLineList.Count; ++i)
                {
                    DutyCycleLineList[i].Stroke = new SolidColorBrush(Color.FromRgb(89, 89, 89));
                }
            }
        }

        //根据控件状态调整点和线段的颜色
        private void ChangeLineAndDotColor()
        {
            if (EditStatus.IsChecked == false)
            {
                for (int i = 0; i < DutyCycleDotList.Count; ++i)
                {
                    DutyCycleDotList[i].Uic.IsEnabled = false;
                    DutyCycleDotList[i].Visibility = Visibility.Collapsed;
                }
                for (int i = 0; i < DutyCycleLineList.Count; ++i)
                {
                    DutyCycleLineList[i].Stroke = new SolidColorBrush(Color.FromRgb(89, 89, 89));
                }
                //隐藏频率曲线
                for (int i = 0; i < FrequencyLineList.Count; ++i)
                {
                    FrequencyLineList[i].Visibility = Visibility.Collapsed;
                }
                //隐藏频率曲线的点
                for (int i = 0; i < FrequencyDotList.Count; ++i)
                {
                    FrequencyDotList[i].Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                for (int i = 0; i < DutyCycleDotList.Count; ++i)
                {
                    DutyCycleDotList[i].Uic.IsEnabled = true;
                    DutyCycleDotList[i].Visibility = Visibility.Visible;
                }
                for (int i = 0; i < DutyCycleLineList.Count; ++i)
                {
                    DutyCycleLineList[i].Stroke = new SolidColorBrush(Colors.Orange);
                }
            }
        }

        //更新DataGrid的索引
        private void UpdateDataGridIndex()
        {
            for (int i = 0; i < viewModel.DutyCycleCurveDotList.Count; ++i)
            {
                viewModel.DutyCycleCurveDotList[i].Index = i;
            }

            for (int i = 0; i < viewModel.FrequencyCurveDotList.Count; ++i)
            {
                viewModel.FrequencyCurveDotList[i].Index = i;
            }

        }

        //获取点在列表中的索引
        private int GetIndexOfDot(PowerCurveDot dot, List<PowerCurveDot> DotList)
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
        #endregion

        private void LineType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化小球和线段
            InitDutyCycleDotAndLine();
            InitFrequencyDotAndLine();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //将所有小球的事件取消注册
            foreach (PowerCurveDot each in DutyCycleDotList)
            {
                each.PreviewMouseMove -= Uic_MouseMove;
                each.PreviewMouseDown -= Uic_MouseDown;
                each.PreviewMouseUp -= Uic_MouseLeftButtonUp;
            }

            foreach (PowerCurveDot each in FrequencyDotList)
            {
                each.PreviewMouseMove -= Uic_MouseMove;
                each.PreviewMouseDown -= Uic_MouseDown;
                each.PreviewMouseUp -= Uic_MouseLeftButtonUp;
            }
        }
    }
}
