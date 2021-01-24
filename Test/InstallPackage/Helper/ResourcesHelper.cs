using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Installationpackage.Helper
{
    public class ResourcesHelper
    {
        public static void ExtractResFile(string resFileName, string outputFile)
        {
            BufferedStream bufferedStream = null;
            FileStream fileStream = null;
            try
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                bufferedStream = new BufferedStream(executingAssembly.GetManifestResourceStream(resFileName));
                fileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                byte[] array = new byte[1024];
                int count;
                while ((count = bufferedStream.Read(array, 0, array.Length)) > 0)
                {
                    fileStream.Write(array, 0, count);
                }
                fileStream.Flush();
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
                if (bufferedStream != null)
                {
                    bufferedStream.Close();
                }
            }
        }
    }
}
