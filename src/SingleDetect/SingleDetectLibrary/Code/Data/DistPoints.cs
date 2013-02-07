using System;
using System.Collections.Generic;
using System.Text;
using SingleDetectLibrary.Code.Contract;

namespace SingleDetectLibrary.Code.Data
{
    public class DistPoints : IDistPoints
    {
        public List<PDist> Data { get; set; }
        public DistPoints()
        {
            Data = new List<PDist>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in Data) sb.Append(string.Format("[{0}] ",p));            
            return sb.ToString();
        }
    }
}
