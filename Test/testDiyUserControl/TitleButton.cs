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

namespace testLineAttritube
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:testLineAttritube"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:testLineAttritube;assembly=testLineAttritube"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:TitleButton/>
    ///
    /// </summary>

    public class TitleButton : Button
    {
        static TitleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleButton), new FrameworkPropertyMetadata(typeof(TitleButton)));
        }
        public ImageBrush NormalIcon
        {
            get { return (ImageBrush)GetValue(NormalProperty); }
            set { SetValue(NormalProperty, value); }
        }
        public static DependencyProperty NormalProperty = DependencyProperty.Register("NormalIcon", typeof(ImageBrush), typeof(TitleButton), new PropertyMetadata(null));

        public ImageBrush PressIcon
        {
            get { return (ImageBrush)GetValue(PressProperty); }
            set { SetValue(PressProperty, value); }
        }
        public static DependencyProperty PressProperty = DependencyProperty.Register("PressIcon", typeof(ImageBrush), typeof(TitleButton), new PropertyMetadata(null));

        public string NormalBackground
        {
            get { return GetValue(NormalBackgroundProperty).ToString(); }
            set { SetValue(NormalBackgroundProperty, value); }
        }
        public static DependencyProperty NormalBackgroundProperty = DependencyProperty.Register("NormalBackground", typeof(string), typeof(TitleButton), new PropertyMetadata(null));

        public string PressBackground
        {
            get { return GetValue(PressBackgroundProperty).ToString(); }
            set { SetValue(PressBackgroundProperty, value); }
        }
        public static DependencyProperty PressBackgroundProperty = DependencyProperty.Register("PressBackground", typeof(string), typeof(TitleButton), new PropertyMetadata(null));

        //外部绑定命令
        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public static DependencyProperty ButtonCommandProperty =
           DependencyProperty.Register("ButtonCommand", typeof(ICommand), typeof(TitleButton), new PropertyMetadata(null));
    }
}
