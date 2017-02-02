using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace uboot.ExtendedImmediateWindow.Implementation.Helpers
{
    public class TempFileNameService : IDisposable
    {
        private static List<string> m_files = new List<string>();

        public static string GetNewFileName(string extension)
        {
            string tempFile = Path.GetTempFileName();
            string requestedFile = tempFile + "." + extension.Replace(".", "");
            m_files.Add(tempFile);
            m_files.Add(requestedFile);
            return requestedFile;
        }

        public static void CleanUp()
        {
            foreach (string file in m_files)
            {
                try { File.Delete(file); }
                catch { }
            }
            m_files = new List<string>();
        }

        #region IDisposable Members

        public void Dispose()
        {
            CleanUp();
        }

        #endregion
    }
}
