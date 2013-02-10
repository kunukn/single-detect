using System;
using System.Runtime.Serialization;
using Kunukn.SingleDetectLibrary.Code.Contract;

namespace Kunukn.SingleDetectLibrary.Code.Data
{
    [Serializable]
    public class P : IP, ISerializable
    {
        private static int _counter;

        public virtual int Uid { get; private set; }        
        public virtual GridIndex GridIndex { get; set; }
        //public virtual Categories Categories { get; set; }
        public virtual int Type { get; set; }
        public virtual double X { get; set; }
        public virtual double Y { get; set; }

        public virtual object Data { get; set; }

        public P()
        {
            Uid = ++_counter;
            GridIndex = new GridIndex();
        }

        public override bool Equals(object obj)
        {
            var o = obj as P;
            if (o == null) { return false; }
            return GetHashCode() == o.GetHashCode();
        }
        public override int GetHashCode()
        {
            return Uid.GetHashCode();
        }      

        // Euclidean distance
        public virtual double Distance(double x, double y)
        {
            var dx = X - x;
            var dy = Y - y;
            var dist = (dx * dx) + (dy * dy);
            dist = Math.Sqrt(dist);
            //dist = Math.Round(dist, 6);
            return dist;
        }
        public override string ToString()
        {
            return string.Format("Id: {0}, X: {1}, Y: {2}", Uid, X, Y);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Uid", this.Uid);
            info.AddValue("X", this.X);
            info.AddValue("Y", this.Y);
        }
    }
}
