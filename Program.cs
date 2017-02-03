using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FileInBin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.*|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInBinCreator cr = new FileInBinCreator(ofd.FileName);
                cr.Save(ofd.FileName.Remove(ofd.FileName.LastIndexOf(@"\")) + @"\FileInBin[" + System.IO.Path.GetFileName(ofd.FileName) + "].cs");
                System.IO.File.Copy(FileInBinCreator.GetCurrentDir() + @"\FileInBin.iface", ofd.FileName.Remove(ofd.FileName.LastIndexOf(@"\")) + @"\FileInBin.cs", true);
            };
            ofd.Dispose();
                

            //FileInBinCreator cr = new FileInBinCreator(@"C:\Downloads\CD-REC\TEMP\FileInBin\bin\Debug\aaa.bmp");
            //cr.Save(@"C:\Downloads\CD-REC\TEMP\FileInBin\aaa.cs");

            //FileInBinSample fib = new FileInBinSample();
            //fib.Save(@"C:\Downloads\CD-REC\TEMP\FileInBin\bin\Debug\a.txt");
        }
    }
}