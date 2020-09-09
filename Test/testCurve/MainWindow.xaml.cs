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

namespace testCurve
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        DrawingVisual drawingVisual = new DrawingVisual();
        public MainWindow()
        {
            InitializeComponent();
            //this.AddVisualChild(drawingVisual);
            PathFigure pathFigure = new PathFigure();
            double xx = 100;
            double yy = 100;
            pathFigure.StartPoint = new Point(xx, yy);

            List<Point> points = new List<Point>();
            points.Add(new Point(500, 100));

            points.Add(new Point(300, 200));
            points.Add(new Point(270, 170));

            points.Add(new Point(700, 300));
            BezierSegment bz1 = new BezierSegment(new Point(500, 100), new Point(300, 200),
                new Point(700, 300), true);
            BezierSegment bz2 = new BezierSegment(new Point(700, 200), new Point(500, 300),
               new Point(900, 400), true);

            PolyBezierSegment poly = new PolyBezierSegment(points, true);
            //pathFigure.Segments.Add(bz1);
            //pathFigure.Segments.Add(bz2);
            //pathFigure.Segments.Add(poly);

            GeometryGroup group1 = new GeometryGroup();
            var g1 = GetSinGeometry(1, 100);
            group1.Children.Add(g1);
            path2.Data = group1;

            GeometryGroup group2 = new GeometryGroup();
            var g2 = GetSinGeometry(60, 50);
            group2.Children.Add(g2);
            path3.Data = group2;

            //PathGeometry pathGeometry = new PathGeometry(new PathFigure[] { pathFigure });

            //DrawingContext drawingContext = drawingVisual.RenderOpen();
            //drawingContext.DrawGeometry(null, new Pen(Brushes.Yellow, 3), pathGeometry);

            //drawingContext.Close();

            //StreamGeometry geometry = new StreamGeometry();

            //using (StreamGeometryContext ctx = geometry.Open())
            //{
            //    ctx.BeginFigure(new Point(10, 100), true, true);
            //    ctx.LineTo(new Point(100, 100), true, false);
            //    ctx.LineTo(new Point(100, 50), true, false);
            //}
        }

        //必须重载这两个方法，不然是画不出来的
        // 重载自己的VisualTree的孩子的个数，由于只有一个DrawingVisual，返回1
        //protected override int VisualChildrenCount
        //{
        //    get { return 1; }
        //}

        // 重载当WPF框架向自己要孩子的时候，返回返回DrawingVisual
        //protected override Visual GetVisualChild(int index)
        //{
        //    if (index == 0)
        //        return drawingVisual;

        //    throw new IndexOutOfRangeException();
        //}

        public StreamGeometry GetSinGeometry(int dx, int dy)
        {
            StreamGeometry g = new StreamGeometry();
            using (StreamGeometryContext ctx = g.Open())
            {
                int x0 = 360;
                double y0 = Math.Sin(-x0 * Math.PI / 180.0);
                ctx.BeginFigure(new Point(-x0, dy * y0), false, false);
                for (int x = -x0; x < x0; x += dx)
                {
                    double y = Math.Sin(x * Math.PI / 180.0);
                    ctx.LineTo(new Point(x, dy * y), true, true);
                }
            }
            g.Freeze();
            return g;
        }
    }
}
