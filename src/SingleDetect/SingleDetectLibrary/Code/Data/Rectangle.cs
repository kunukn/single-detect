namespace SingleDetectLibrary.Code.Data
{
    public class Rectangle
    {
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }

        public double Width { get { return XMax - XMin; } }
        public double Height { get { return YMax - YMin; } }

        public override string ToString()
        {
            return string.Format("{0}; {1}; {2}; {3}", XMin, XMax, YMin, YMax);
        }
    }
}
