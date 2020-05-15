using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace radio.ViewModel
{
    public class MainWindowViewModel:NotificationObject
    {
        public DelegateCommand<object> CloseCommand { get; set; }
        public DelegateCommand ResetNameCommand { get; set; }
        public Model model { get; set; }
        public MainWindowViewModel()
        {
            CloseCommand = new DelegateCommand<object>((p) =>
            {
                Application.Current.Shutdown();
            });

            model = new Model();
            ResetNameCommand = new DelegateCommand();
            ResetNameCommand.ExcuteCommand = new Action<object>(model.AddName);
        }
    }

    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Func<object, bool> CanExcuteCommand;
        public Action<object> ExcuteCommand;
        public bool CanExecute(object parameter)
        {
            if (CanExcuteCommand != null)
                return CanExcuteCommand(parameter);
            else
                return true;
        }

        public void Execute(object parameter)
        {
            ExcuteCommand?.Invoke(parameter);
        }

        public void RaiseCanExcuteChanged()
        {
            if (CanExcuteCommand != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
