using System.Collections.Generic;
using System.Linq;
using Kunukn.SingleDetectLibrary.Code.Contract;

namespace Kunukn.SingleDetectLibrary.Code.Data
{
    public class NearestNeighbor
    {
        public IP Origin { get; set; } // origin
        public int K { get; set; }  // k nearest   
        public IList<IPDist> NNs { get; set; } // nearest neighbor

        public NearestNeighbor()
        {
            NNs = new List<IPDist>();
        }

        public List<IP> GetNNs()
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
