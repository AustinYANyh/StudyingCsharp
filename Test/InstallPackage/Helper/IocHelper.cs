using Microsoft.Practices.Unity;

namespace Installationpackage.Helper
{
    public class IocHelper
    {
        public static UnityContainer container = new UnityContainer();

        public static UnityServiceLocator SimpleIOC = new UnityServiceLocator(IocHelper.container);
    }
}
