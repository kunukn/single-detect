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
    /// Where Remove is O(1), Add is O(1)
    /// </summary>    
    /// 

    public class PP : IComparable
    {
        private readonly Guid _guid;
        public HashSet<IPDist> Set { get; set; }
        public double Distance { get; set; }
        public int Count { get { return Set.Count; } }

        public PP()
        {
            _guid = Guid.NewGuid();
            Set = new HashSet<IPDist>();
        }

        public bool Contains(double dist)
        {
            // [Suppress floating point comparison warning]
            return dist == Distance;
            
        }

        public void Add(IPDist p)
        {
            if (Set.Contains(p)) throw new ApplicationException(string.Format("Already added: {0}",p));

            Set.Add(p);
            if (Set.Count == 1) Distance = p.Distance;

            // [Suppress floating point comparison warning]
            if (this.Distance!=p.Distance) throw new ApplicationException(string.Format("Not same distance: {0}", p));
        }

        public void Remove()
        {
            if (Set.Count > 0) Set.Remove(Set.First());
            else throw new ApplicationException("Can't remove empty");

            if (Set.Count == 0) this.Distance = Double.MaxValue;
        }

        public IList<IPDist> GetAll()
        {
            return Set.ToList();
        }

        public int CompareTo(object obj)
        {
            var o = obj as PP;
            if (o == null) return -1;

            return Distance.CompareTo(o.Distance);
        }
        public override bool Equals(object obj)
        {
            var o = obj as IPDist;
            if (o == null) { return false; }
            return GetHashCode() == o.GetHashCode();
        }
        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Distance: {0}, Count: {1}", 
                Math.Round(Distance, 4), Set.Count);
        }
    }
}
