//按钮点击,调用xaml中注册的方法
//也可以在publish中传递参数,subscribe中直接获取
private void Editor_Click(object sender, RoutedEventArgs e)
{
    TechnicsData data = new TechnicsData();
    EventAggregatorRepository.EventAggregator.GetEvent<GetTechnicFileListEvent>().Publish(true);
    TechnicsPage.TechnicsDatasList.Add(data);
}

//xaml中Load事件添加注册
private void UserControl_Loaded(object sender, RoutedEventArgs e)
{
    EventAggregatorRepository.EventAggregator.GetEvent<GetTechnicFileListEvent>().Subscribe(GetTechnicFileList);
}

private void GetTechnicFileList(bool obj)
{
    TechnicsDatasList = viewModel.TechnicsDataList.ToList();
}

//创建的类库,消息器
public class EventAggregatorRepository
{
    //消息器，共用
    private static IEventAggregator _eventAggregator;
    public static IEventAggregator EventAggregator
    {
        get
        {
            if (_eventAggregator == null)
            {
                _eventAggregator = new EventAggregator();
            }
            return _eventAggregator;
        }
    }
}

//事件类,提供事件的名字和publish中的参数类型
/// <summary>
/// 获取工艺文件列表的事件
/// </summary>
public class GetTechnicFileListEvent : CompositePresentationEvent<bool>
{

}