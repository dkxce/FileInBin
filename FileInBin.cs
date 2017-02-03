using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text;

namespace FileInBin
{
    public interface IFileInBin
    {
        void Save(string filename);
        ulong Size();
    }

    public class FileInBinCreator : IFileInBin
    {
        private DateTime ModifiedUTC = DateTime.MinValue;
        private DateTime CreatedUTC = DateTime.MinValue;
        private string FileName = String.Empty;
        
        public FileInBinCreator(string fileName)
        {
            this.FileName = fileName;
            FileInfo fi = new FileInfo(fileName);
            CreatedUTC = fi.CreationTimeUtc;
            ModifiedUTC = fi.LastWriteTimeUtc;
        }   

        public void Save(string filename)
        {
            FileStream header = new FileStream(GetCurrentDir() + @"\FileInBin.header", FileMode.Open, FileAccess.Read);
            FileStream modul = new FileStream(filename, FileMode.Create, FileAccess.Write);
            FileStream input = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);

            StreamReader hd = new StreamReader(header);
            string head = hd.ReadToEnd();
            hd.Close();
            header.Close();

            head = head.Replace("{ModifiedUTC}", String.Format("DateTime.Parse(\"{0}\")", ModifiedUTC.ToString("dd.MM.yyyy HH:mm:ss")));
            head = head.Replace("{CreatedUTC}", String.Format("DateTime.Parse(\"{0}\")", CreatedUTC.ToString("dd.MM.yyyy HH:mm:ss")));
            head = head.Replace("{FileName}", System.IO.Path.GetFileName(FileName));
            head = head.Replace("{FileNameWE}", System.IO.Path.GetFileName(FileName).Replace(".", "_d_"));

            StreamWriter sw = new StreamWriter(modul);
            sw.Write(head);
            sw.Flush();
            {
                int rb = input.ReadByte();
                sw.Write(rb.ToString());
                while ((rb = input.ReadByte()) >= 0)
                    sw.Write(","+rb.ToString());

                sw.Write("};\r\n}\r\n}");
                sw.Flush();
            };
            
            modul.Close();
            input.Close();
            
        }

        public ulong Size()
        {
            return 0;
        }

        public static string GetCurrentDir()
        {
            string fname = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString();
            fname = fname.Replace("file:///", "");
            fname = fname.Replace("/", @"\");
            fname = fname.Substring(0, fname.LastIndexOf(@"\") + 1);
            return fname;
        }
    }

    public class FileInBinSample : IFileInBin
    {                
        public void Save(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            fs.Write(filedata, 0, filedata.Length);
            fs.Close();

            File.SetCreationTimeUtc(filename, DateTime.UtcNow);
            File.SetLastWriteTimeUtc(filename, DateTime.UtcNow);
        }

        public ulong Size()
        {
            return (ulong)filedata.Length;
        }

        private DateTime ModifiedUTC = DateTime.MinValue;
        private DateTime CreatedUTC = DateTime.MinValue;
        private string FileName = String.Empty;
        internal static byte[] filedata = new byte[] { 200, 201, 202 };
    }
}
