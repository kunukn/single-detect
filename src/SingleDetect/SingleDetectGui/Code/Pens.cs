using System.Windows.Media;

namespace SingleDetectGui.Code
{
    public static class Pens
    {    
        public static readonly SolidColorBrush DotColor = new SolidColorBrush { Color = Color.FromRgb(0, 0, 0) };
        public static readonly SolidColorBrush DotColorSingle = new SolidColorBrush { Color = Color.FromRgb(255, 0, 0) };
        public static readonly SolidColorBrush BackgroundColor = new SolidColorBrush { Color = Color.FromRgb(240, 240, 240) };

        public static readonly Pen PenBlue = new Pen(Brushes.Blue, 0.1)
        {
            StartLineCap = PenLineCap.Round,
            EndLineCap = PenLineCap.Round,
        };
        public static readonly Pen PenRed = new Pen(Brushes.Red, 0.1)
        {
            StartLineCap = PenLineCap.Round,
            EndLineCap = PenLineCap.Round,
        };
        public static readonly Pen PenGreen = new Pen(Brushes.Green, 0.1)
        {
            StartLineCap = PenLineCap.Round,
            EndLineCap = PenLineCap.Round,
        };
        public static readonly Pen PenOrange = new Pen(Brushes.Orange, 0.1)
        {
            StartLineCap = PenLineCap.Round,
            EndLineCap = PenLineCap.Round,
        };
        public static readonly Pen PenBackground = new Pen(BackgroundColor, 0.1)
        {
            StartLineCap = PenLineCap.Round,
            EndLineCap = PenLineCap.Round,
        };

        static Pens()
        {
            PenBlue.Freeze();
            PenRed.Freeze();
            PenGreen.Freeze();
            PenOrange.Freeze();
            PenBackground.Freeze();       
        }
    }

    
}
