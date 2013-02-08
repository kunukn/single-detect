using System;
using System.Collections.Generic;
using System.Text;
using SingleDetectLibrary.Code.Contract;

namespace SingleDetectLibrary.Code.Data
{
    public class DistPoints : IDistPoints
    {
        public List<IPDist> Data { get; set; }

        public int Count { get { return Data.Count; } }

        public IPDist this[int i]
        {
            get { return Data[i]; }
            set { Data[i] = value; }
        }

        public void Add(IPDist p)
        {
            Data.Add(p);
        }

        public DistPoints()
        {
            Data = new List<IPDist>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in Data) sb.Append(string.Format("[{0}] ",p));            
            return sb.ToString();
        }
    }
}
