using System;
using SingleDetectLibrary.Code.Contract;

namespace SingleDetectLibrary.Code.Data
{
    public class P : XY, IP
    {
        private static int _counter;
        public int Uid { get; private set; }
        public bool Visible { get; set; }
        public GridIndex GridIndex { get; set; }
        public object Data { get; set; }

        public P()
        {
            Uid = ++_counter;
            GridIndex = new GridIndex();
            Visible = true;
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

        public virtual double Distance(IP p)
        {
            return Distance(p.X, p.Y);
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

        public virtual void Round(int decimals)
        {
            X = Math.Round(X, decimals);
            Y = Math.Round(Y, decimals);
        }
    }
}
