using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testUnit.Model
{
    public class ModelLocator
    {
        public static ModelLocator Locator { get; } = new ModelLocator();

        public ModelLocator()
        {
            SimpleIOC.container.RegisterInstance(new MainModel());
        }

        public MainModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainModel>(); }
        }
    }
}
