using System;
using System.Reflection;
using System.Resources;
using System.Windows;

namespace Installationpackage
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.AssemblyResolve += this.CurrentDomain_AssemblyResolve;

            base.OnStartup(e);
        }

        //找不到dll时, 从自身嵌入的资源中加载
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string text = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
            text = text.Replace(".", "_");
            Assembly result;
            if (text.EndsWith("_resources"))
            {
                return null;
            }
            else
            {
                ResourceManager resourceManager = new ResourceManager(base.GetType().Namespace + ".Properties.Resources", Assembly.GetExecutingAssembly());
                byte[] rawAssembly = (byte[])resourceManager.GetObject(text);
                result = Assembly.Load(rawAssembly);
            }
            return result;
        }

        //全局异常处理
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }
    }
}
