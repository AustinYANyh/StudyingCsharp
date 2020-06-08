private void status_info_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
{
    ModeSwitchPage ModeSwitchPage = Application.Current.MainWindow.FindName("ModeSwitchPage") as ModeSwitchPage;
    if (ModeSwitchPage == null)
        return;
    ModeSwitchPage.MainTabControl.SelectedIndex = 2;
    ModeSwitchPage.DiagnosePage.MonitorTab.SelectedIndex = 2;
}