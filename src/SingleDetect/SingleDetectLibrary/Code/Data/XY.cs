namespace SingleDetectLibrary.Code.Data
{
    public class XY
    {
        public virtual double X { get; set; }
        public virtual double Y { get; set; }
        public override string ToString() { return string.Format("{0}; {1}", X,Y); }
    }
}
