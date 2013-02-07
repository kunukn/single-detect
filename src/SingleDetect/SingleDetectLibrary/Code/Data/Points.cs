using System;
using System.Collections.Generic;
using System.Text;
using SingleDetectLibrary.Code.Contract;

namespace SingleDetectLibrary.Code.Data
{
    public class Points : IPoints
    {
        public List<P> Data { get; set; }        
        public Points()
        {
            Data = new List<P>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in Data) sb.Append(string.Format("[{0}] ",p));            
            return sb.ToString();
        }
    }
}
