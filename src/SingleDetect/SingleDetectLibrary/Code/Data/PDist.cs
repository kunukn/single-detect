using System;
using System.Runtime.Serialization;
using Kunukn.SingleDetectLibrary.Code.Contract;

namespace Kunukn.SingleDetectLibrary.Code.Data
{
    /// <summary>
    /// Used for Grid K Nearest Neighbor
    /// </summary>    
    /// 
    [Serializable]
    public class PDist : IPDist, IComparable, ISerializable
    {
        public IP Point { get; set; }
        public double Distance { get; set; }

        public int CompareTo(object obj)
        {
            var o = obj as IPDist;
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
            return Point.Uid.GetHashCode();
        }
       
        public override string ToString()
        {
            return string.Format("{0}, Distance: {1}", Point, Math.Round(Distance,4));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", this.Point.X);
            info.AddValue("Y", this.Point.Y);
            info.AddValue("Dist", this.Distance);
        }
    }
}
