using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace test
{
    public class func
    {
        [DllImport(@"C:\Users\Austin_Yan\Documents\Visual Studio 2013\Projects\Math\Debug\Math.dll",EntryPoint="?Math_Add@CMath@@QAEHHH@Z")]
        public static extern int Math_Add(int a,int b);

        [DllImport(@"C:\Users\Austin_Yan\Documents\Visual Studio 2013\Projects\Math\Debug\Math.dll", EntryPoint = "?Math_Sub@CMath@@QAEHHH@Z")]
        public static extern int Math_Sub(int a, int b);

        [DllImport(@"C:\Users\Austin_Yan\Documents\Visual Studio 2013\Projects\Math\Debug\Math.dll", EntryPoint = "?Math_Col@CMath@@QAEHHH@Z")]
        public static extern int Math_Col(int a, int b);

        [DllImport(@"C:\Users\Austin_Yan\Documents\Visual Studio 2013\Projects\Math\Debug\Math.dll", EntryPoint = "?Math_Dev@CMath@@QAEHHH@Z")]
        public static extern int Math_Dev(int a, int b);
    }
    class Program
    {
        static void Main(string[] args)
        {
            int a = func.Math_Add(5, 6);
            Console.WriteLine(a);
            a = func.Math_Sub(6, 5);
            Console.WriteLine(a);
            a = func.Math_Col(6, 5);
            Console.WriteLine(a);
            a = func.Math_Dev(12, 3);
            Console.WriteLine(a);
        }
    }
}
