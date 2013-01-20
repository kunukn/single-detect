using System;
using System.Reflection;
using MathNet.Numerics;
using System.Numerics;

namespace KdTree
{
    public class DoubleArithmetic : Arithmetic<double>
    {
        public override double Zero
        {
            get { return 0.0; }
        }

        public override double One
        {
            get { return 1.0; }
        }

        public override double MinValue
        {
            get { return double.MinValue; }
        }

        public override double MaxValue
        {
            get { return double.MaxValue; }
        }

        public override bool Equal(double a, double b)
        {
            return double.Equals(a, b);
        }

        public override bool AlmostEqual(double a, double b)
        {
            return Precision.AlmostEqual(a, b);
        }

        public override double EpsilonOf(double a)
        {
            return Precision.EpsilonOf(a);
        }

        public override double Add(double a, double b)
        {
            return a + b;
        }

        public override double Subtract(double a, double b)
        {
            return a - b;
        }

        public override double Multiply(double a, double b)
        {
            return a*b;
        }

        public override double Divide(double a, double b)
        {
            return a/b;
        }

        public override double Sqrt(double a)
        {
            return Math.Sqrt(a);
        }

        public override double Abs(double a)
        {
            return Math.Abs(a);
        }
    }

    public abstract class Arithmetic<TField> : IArithmetic<TField>
    {
        public static Arithmetic<TField> Default
        {
            get
            {
                var type = typeof (TField);
                if (type == typeof (double))
                    return (Arithmetic<TField>) (object) new DoubleArithmetic();

                throw new InvalidOperationException(string.Format(
                    "{0}-{1}", MethodBase.GetCurrentMethod(), type.Name));
            }
        }

        public Arithmetic()
        {
        }

        public abstract TField Zero { get; }

        public abstract TField One { get; }

        public abstract TField MinValue { get; }

        public abstract TField MaxValue { get; }

        public abstract bool Equal(TField a, TField b);

        public abstract bool AlmostEqual(TField a, TField b);

        public abstract TField EpsilonOf(TField a);

        public abstract TField Add(TField a, TField b);

        public abstract TField Subtract(TField a, TField b);

        public abstract TField Multiply(TField a, TField b);

        public abstract TField Divide(TField a, TField b);

        public abstract TField Sqrt(TField a);

        public abstract TField Abs(TField a);
    }

    public interface IArithmetic<TField>
    {
        TField Zero { get; }

        TField One { get; }

        TField MinValue { get; }

        TField MaxValue { get; }

        bool Equal(TField a, TField b);

        bool AlmostEqual(TField a, TField b);

        TField EpsilonOf(TField a);

        TField Add(TField a, TField b);

        TField Subtract(TField a, TField b);

        TField Multiply(TField a, TField b);

        TField Divide(TField a, TField b);

        TField Sqrt(TField a);

        TField Abs(TField a);
    }
}