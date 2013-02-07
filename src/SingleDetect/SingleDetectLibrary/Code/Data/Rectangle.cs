using System;
using System.Reflection;
using SingleDetectLibrary.Code.Contract;
using SingleDetectLibrary.Code.MathUtil;
using Math = System.Math;

namespace SingleDetectLibrary.Code.Data
{
    public class Rectangle : IRectangle
    {
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }

        public double Width { get { return XMax - XMin; } }
        public double Height { get { return YMax - YMin; } }

        public double MaxDistance { get; set; }
        public double Square
        {
            get { return M.CtoA(MaxDistance); }
        }
        public int XGrid
        {
            get { return (int)(Math.Ceiling(Width / Square)); }
        }
        public int YGrid
        {
            get { return (int)(Math.Ceiling(Height / Square)); }
        }
        
        public double XO
        {
            get
            {
                if (XMin < 0) return -XMin;
                return 0;
            }
        }
        
        public double YO
        {
            get
            {
                if (YMin < 0) return -YMin;
                return 0;
            }
        }

        public void Validate()
        {
            if (XMin > XMax || YMin > YMax) throw new ApplicationException(MethodBase.GetCurrentMethod().ToString());
        }

        public override string ToString()
        {
            return string.Format("{0}; {1}; {2}; {3}; {4}; {5}; {6}; {7}",
                XMin, XMax, YMin, YMax, MaxDistance, Square, XGrid, YGrid);
        }
    }
}
