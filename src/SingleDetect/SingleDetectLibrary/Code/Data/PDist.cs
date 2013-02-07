using System;

namespace SingleDetectLibrary.Code.Data
{
    /// <summary>
    /// Used for Grid K Nearest Neighbor
    /// </summary>
    public class PDist : IComparable
    {
        public P Point { get; set; }
        public double Distance { get; set; }

        public int CompareTo(object obj)
        {
            var o = obj as PDist;
            if (o == null) return -1;
                        
            return Distance.CompareTo(o.Distance);            
        }
        public override bool Equals(object obj)
        {
            var o = obj as PDist;
            if (o == null) { return false; }
            return GetHashCode() == o.GetHashCode();
        }
        public override int GetHashCode()
        {
            return Point.Uid.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("{0}, Distance: {1}", Point, Math.Round(Distance,4));
        }
    }
}
