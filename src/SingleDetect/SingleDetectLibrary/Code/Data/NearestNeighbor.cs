using System.Collections.Generic;
using System.Linq;

namespace SingleDetectLibrary.Code.Data
{
    public class NearestNeighbor
    {
        public P Origin { get; set; } // origin
        public int K { get; set; }  // k nearest   
        public List<PDist> NNs { get; set; } // nearest neighbor

        public NearestNeighbor()
        {
            NNs = new List<PDist>();
        }

        public List<P> GetNNs()
        {            
            return NNs.Select(i => i.Point).ToList();            
        }

        public double GetDistanceSum()
        {            
            return NNs.Aggregate(0d, (a, b) => (a + b.Distance) );            
        }
        
        public void Clear()
        {
            Origin = null;
            NNs.Clear();
            K = 0;
        }
    }
}
