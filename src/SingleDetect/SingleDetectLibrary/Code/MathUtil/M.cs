using System;

namespace Kunukn.SingleDetectLibrary.Code.MathUtil
{
    /// <summary>
    /// MathUtil
    /// </summary>
    public static class M
    {
        public const double Epsilon = 0.0000001;

        public static int Round(double d)
        {
            return (int)Math.Round(d);
        }
   
        public static double CtoA(double c)
        {
            // Pythagoras on square
            // c = sqrt(2a^2) => a = sqrt( c^2 / 2)

            var d = Math.Sqrt((c * c) / 2);      
            d -= Epsilon; // make sure max distance in square is smaller than c           
            return d;
        }

        public static double TruncateDecimal(double value, int precision)
        {
            var step = Math.Pow(10, precision);
            var tmp = (int)Math.Truncate(step * value);
            return tmp / step;
        }
    }

}
