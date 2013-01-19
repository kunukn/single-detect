using System;

namespace SingleDetectLibrary.Code.Data
{
    public class P : XY
    {        
        private static int _counter;
        public int Id { get; private set; }        
        public bool Visible { get; set; }
        public GridIndex GridIndex { get; set; }

        public P()
        {
            Id = ++_counter;                       
            GridIndex = new GridIndex();
            Visible = true;
        }
        
        public override bool Equals(object obj)
        {
            var other = obj as P;
            if (other == null) { return false; }
            return Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
               
        public double Distance(P p)
        {
            return Distance(p.X, p.Y);            
        }
        public double Distance(double x, double y)
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
            return string.Format("Id: {0}, {1}",Id, base.ToString());            
        }
    }
}
