using System.Windows.Media;

namespace Kunukn.SingleDetectGuiCore.Code.Gui
{
    public static class Pens
    {    
        public static readonly SolidColorBrush DotColor = new SolidColorBrush { Color = Color.FromRgb(0, 0, 0) };
        public static readonly SolidColorBrush DotColorSelected = new SolidColorBrush { Color = Color.FromRgb(200, 0, 0) };
        public static readonly SolidColorBrush DotColorSingle = new SolidColorBrush { Color = Color.FromRgb(255, 0, 0) };
        public static readonly SolidColorBrush DotColorNearestNeighbor = new SolidColorBrush { Color = Color.FromRgb(255, 130, 171) };
        public static readonly SolidColorBrush BackgroundColor = new SolidColorBrush { Color = Color.FromRgb(255, 255, 255) };
        public static readonly SolidColorBrush GrayColor = new SolidColorBrush { Color = Color.FromRgb(230, 230, 230) };

        public static readonly Pen PenGrid = new Pen(GrayColor, 0.1)
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
            PenGrid.Freeze();
            PenBackground.Freeze();       
        }
    }    
}
