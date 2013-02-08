using System.Collections.Generic;
using System.Linq;
using Kunukn.SingleDetectLibrary.Code.Contract;

namespace Kunukn.SingleDetectLibrary.Code.Data
{
    public class NearestNeighbor
    {
        public IP Origin { get; set; } // origin
        public int K { get; set; }  // k nearest   
        public IDistPoints NNs { get; set; } // nearest neighbor

        public NearestNeighbor()
        {
            NNs = new DistPoints{};                
        }

        public List<IP> GetNNs()
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
