public partial class TechnicSaveAsDialog : UserControl
    {
        public Window _window;
        public TechnicSaveAsDialog(Window window)
        {
            InitializeComponent();
            _window = window;
        }

        private List<string> MaterialNameList;
        private List<string> NozzleTypeList;
        private List<string> GasTypeList;
        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            _window.DialogResult = true;
            _window.Close();
            Application.Current.MainWindow.Activate();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            _window.DialogResult = false;
            _window.Close();
            Application.Current.MainWindow.Activate();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateRemarksAndPicName();
        }

        private void Combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MaterialNameList == null || NozzleTypeList == null || GasTypeList == null)
                return;
            UpdateRemarksAndPicName();
        }

        private void UpdateRemarksAndPicName()
        {
            if (ThicknessTextBox == null || ThicknessUnit == null || LaserPowerTextBox == null || LaserPowerUnit == null
                || ProcessGasCombox == null || ProcessTypeCombox == null || NozzleDiameterCombox == null || NozzleTypeCombox == null)
                return;
            string material = MaterialNameList[MaterialCombox.SelectedIndex];
            string thickness = ThicknessTextBox.Text + ThicknessUnit.Content;
            string laserpower = LaserPowerTextBox.Text + LaserPowerUnit.Content;
            string processtype = (ProcessTypeCombox.SelectedItem as ComboBoxItem).Content.ToString();
            string nozzletype = NozzleTypeList[NozzleTypeCombox.SelectedIndex];
            string nozzlediameter = (NozzleDiameterCombox.SelectedItem as ComboBoxItem).Content.ToString();
            string gastype = GasTypeList[ProcessGasCombox.SelectedIndex];

            RemarksTextBox.Text = string.Format("{0}-{1}-{2}-{3}-{4}{5}-{6}",
                material, thickness, laserpower, processtype, nozzletype, nozzlediameter, gastype);

            material = MaterialNameList[MaterialCombox.SelectedIndex + 3];
            thickness = ThicknessTextBox.Text;
            laserpower = LaserPowerTextBox.Text;
            processtype = (ProcessTypeCombox.SelectedItem as ComboBoxItem).Content.ToString();
            nozzletype = NozzleTypeList[NozzleTypeCombox.SelectedIndex + 2];
            nozzlediameter = (NozzleDiameterCombox.SelectedItem as ComboBoxItem).Content.ToString();
            gastype = GasTypeList[ProcessGasCombox.SelectedIndex + 3];

            PicNameTextBox.Text = string.Format("{0}-{1}-{2}-{3}-{4}{5}-{6}",
                material, thickness, laserpower, processtype, nozzletype, nozzlediameter, gastype);
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MaterialNameList = new List<string> { "不锈钢", "铝合金", "黄铜", "Ss", "Ai", "Br" };
            NozzleTypeList = new List<string> { "单层", "双层", "S", "D" };
            GasTypeList = new List<string> { "空气", "氮气", "氧气", "Air", "N2", "O2" };
        }
    }