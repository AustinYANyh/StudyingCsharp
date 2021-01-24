using Installationpackage.Helper;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Installationpackage.Model
{
    public class ModelLocator
    {
        public static ModelLocator Locator = new ModelLocator();

        public ModelLocator()
        {
            IocHelper.container.RegisterInstance(new MainModel());
            IocHelper.container.RegisterInstance(new ProgressModel());
        }

        public MainModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainModel>(); }
        }

        public ProgressModel Progress
        {
            get { return ServiceLocator.Current.GetInstance<ProgressModel>(); }
        }
    }
}
