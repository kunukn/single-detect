using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using SingleDetectLibrary.Code.Data;
using SingleDetectLibrary.Code.MathUtil;

namespace SingleDetectGui.Code.Gui
{
    public static class DrawUtil
    {
        public static bool IsDrawEnabled = true; // default
        private static int PSize = 2; // default

        public static void Init(int psize)
        {
            PSize = psize;
        }

        public static System.Windows.Rect GetShape(P p, ShapeType st = ShapeType.Default)
        {
            switch (st)
            {
                case ShapeType.Default:
                    return new System.Windows.Rect(p.X - PSize / 2, p.Y - PSize / 2, PSize, PSize);
                case ShapeType.Single:
                    return new System.Windows.Rect(p.X - PSize / 2, p.Y - PSize / 2, PSize, PSize);
                //case ShapeType.Single:
                //    return new Rect(p.X - PSize / 2 - 1, p.Y - PSize / 2 - 1, PSize + 2, PSize + 2);
            }
            throw new ApplicationException(string.Format("Unknown type: {0}", st));
        }

        public static void ClearSingles(DrawingContext dc, ICollection<P> singles)
        {        
            if(!IsDrawEnabled) return;

            foreach (var p in singles.Where(p => p.Visible))
            {
                dc.DrawRectangle(Pens.BackgroundColor, null, GetShape(p,ShapeType.Single));
                dc.DrawRectangle(Pens.DotColor, null, GetShape(p));
            }
        }

        public static void ClearDots(DrawingContext dc, ICollection<P> dots)
        {
            if (!IsDrawEnabled) return;

            foreach (var p in dots.Where(p => p.Visible))
            {
                dc.DrawRectangle(Pens.BackgroundColor, null, GetShape(p));
            }
        }

        public static void DrawDots(DrawingContext dc, ICollection<P> dots)
        {
            if (!IsDrawEnabled) return;

            foreach (var p in dots.Where(p => p.Visible))
            {
                dc.DrawRectangle(Pens.DotColor, null, GetShape(p));
            }
        }

        public static void DrawSingles(DrawingContext dc, ICollection<P> dots)
        {
            if (!IsDrawEnabled) return;

            foreach (var p in dots.Where(p => p.Visible))
            {
                dc.DrawRectangle(Pens.DotColorSingle, null, GetShape(p,ShapeType.Single));
            }
        }

        public static void ClearBackground(DrawingContext dc, int x, int y, int w, int h)
        {
            if (!IsDrawEnabled) return;

            dc.DrawRectangle(Pens.BackgroundColor, null, new System.Windows.Rect(x, y, w, h));
        }

        public static void DrawGrid(DrawingContext dc, bool showGrid, int width, int height, int xgrid, int ygrid)
        {
            if (!IsDrawEnabled) return;
                                 
            Pen pen = showGrid ? Pens.PenBlue : Pens.PenBackground;
            double dw = width / (double)xgrid; // delta w
            for (var i = 0; i <= xgrid; i++)
            {
                dc.DrawLine(pen, new Point(M.Round(i * dw), 0), new Point(M.Round(i * dw), height));
            }
            double dh = height / (double)ygrid; // delta h
            for (var i = 0; i <= ygrid; i++)
            {
                dc.DrawLine(pen, new Point(0, M.Round(i * dh)), new Point(width, M.Round(i * dh)));
            }            
        }
    }
}
