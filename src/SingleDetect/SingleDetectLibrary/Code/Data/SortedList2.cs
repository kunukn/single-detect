using System;
using System.Collections.Generic;
using System.Linq;
using Kunukn.SingleDetectLibrary.Code.Contract;

namespace Kunukn.SingleDetectLibrary.Code.Data
{
    /// <summary>
    /// Used for Naive K Nearest Neighbor
    /// 
    /// Like SortedList with multiple same values allowed
    /// Where remove and add operations are O(logk)
    /// </summary>    
    /// 

    public class SortedList2
    {
        private int _count;
        private readonly Dictionary<double, LinkedList<IPDist>> _lookup = new Dictionary<double, LinkedList<IPDist>>();
        private readonly SortedSet<double> _set = new SortedSet<double>();

        public void Add(IPDist p, int k)
        {
            if(p==null || k<=0) return;

            var dist = p.Distance;

            if(_count<k) _count++;            
            else
            {
                var max = _set.Max;                
                if(dist >= max) return; // Don't add

                // Remove old
                var bag = _lookup[max];
                bag.RemoveLast();
                if(bag.Count==0)
                {
                    _lookup.Remove(max);
                    _set.Remove(max);
                }                                
            }

            if (_set.Contains(dist)) _lookup[dist].AddLast(p); // Append
            else
            {
                // Insert new
                _set.Add(dist);
                var ps = new LinkedList<IPDist>();
                ps.AddLast(p);
                _lookup.Add(dist, ps);
            }
        }

        public List<IPDist> GetAll()
        {
            var all = new List<IPDist>();
            var dists = _set.ToList();
            foreach (var dist in dists) all.AddRange(_lookup[dist]);

            return all;
        }  

        public int Count { get { return _count; } }            
    }
}
