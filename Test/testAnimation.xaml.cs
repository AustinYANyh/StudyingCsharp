﻿using System;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace testAnimation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ObjectAnimationUsingKeyFrames animate = new ObjectAnimationUsingKeyFrames();
            animate.Duration = new TimeSpan(0, 0, 1);
            DiscreteObjectKeyFrame kf1 = new DiscreteObjectKeyFrame(Visibility.Visible, new TimeSpan(0, 0, 1));
            animate.KeyFrames.Add(kf1);
            line.BeginAnimation(Line.VisibilityProperty, animate);
        }
    }
}
