using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testUnit
{
    public static class SimpleIOC
    {
        public static UnityContainer container = new UnityContainer();
        public static UnityServiceLocator simpleIoc = new UnityServiceLocator(container);
    }
}
