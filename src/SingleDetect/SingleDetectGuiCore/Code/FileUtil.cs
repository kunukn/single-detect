using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Kunukn.SingleDetectGuiCore.Code
{
    /// <summary>
    /// Author: Kunuk Nykjaer
    /// </summary>
    public static class FileUtil
    {
        public const string FolderPath = @"c:\temp\";
        private static readonly Encoding _encodingRead = Encoding.Default; // Encoding.Default  Encoding.UTF8  Encoding.Unicode      
        private static readonly Encoding _encodingWrite = Encoding.Unicode; //Encoding.Default  Encoding.UTF8  Encoding.Unicode

      
        public static List<string> ReadFile(FileInfo fi)
        {
            return ReadFile(fi.FullName);
        }

        public static List<string> ReadFile(string path)
        {
            var list = new List<string>();
            try
            {                
                using (var reader = new StreamReader(path, _encodingRead, true))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        list.Add(line);
                        line = reader.ReadLine();
                    }
                }
            }
            finally{}
            return list;
        }       
    }
}
