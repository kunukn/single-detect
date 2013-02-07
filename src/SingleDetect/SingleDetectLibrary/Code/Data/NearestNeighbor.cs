using System.Collections.Generic;
using System.Linq;
using SingleDetectLibrary.Code.Contract;

namespace SingleDetectLibrary.Code.Data
{
    public class NearestNeighbor
    {
        public P Origin { get; set; } // origin
        public int K { get; set; }  // k nearest   
        public IDistPoints NNs { get; set; } // nearest neighbor

        public NearestNeighbor()
        {
            NNs = new DistPoints{};                
        }

        public List<P> GetNNs()
        {            
            return NNs.Data.Select(i => i.Point).ToList();            
        }

        public double GetDistanceSum()
        {            
            return NNs.Data.Aggregate(0d, (a, b) => (a + b.Distance) );            
        }
        
        public void Clear()
        {
            Origin = null;
            NNs.Data.Clear();
            K = 0;
        }
    }
}
