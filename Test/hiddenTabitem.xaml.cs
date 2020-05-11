        List<int> TabControlIndex = new List<int>();
        
        int tabindex = 0;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.Left = 0;
            this.Top = 0;
            this.Width = SystemParameters.PrimaryScreenWidth;//得到屏幕整体宽度
            this.Height = SystemParameters.PrimaryScreenHeight;//得到屏幕整体高度
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tabindex = 0;
            for (int i = 0; i < ConfigTabControl.Items.Count; ++i)
            {
                if ((ConfigTabControl.Items[i] as TabItem).Visibility == Visibility.Visible)
                {
                    TabControlIndex.Add(i);
                }
            }
        }

        private void LastStepBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tabindex != 0)
            {
                ConfigTabControl.SelectedIndex = TabControlIndex[--tabindex];
            }
        }

        private void NextStepBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tabindex != TabControlIndex.Count - 1)
            {
                ConfigTabControl.SelectedIndex = TabControlIndex[++tabindex];
            }
        }

        private void ConfigTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tabindex = TabControlIndex.FindIndex(x => x.Equals(ConfigTabControl.SelectedIndex));
        }