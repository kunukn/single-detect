using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using SingleDetectLibrary.Code.Data;
using SingleDetectLibrary.Code.MathUtil;

namespace SingleDetectGuiCore.Code.Gui
{
    public static class DrawUtil
    {
        public static bool IsDrawEnabled = true; // default
        private static int PSize = 2; // default, must be int

        public static void Init(int psize)
        {
            PSize = psize;
        }

        public static System.Windows.Rect GetShape(P p, ShapeType st = ShapeType.Default)
        {
            switch (st)
            {
                case ShapeType.Selected:
                    return new System.Windows.Rect(p.X - 1 - PSize / 2, p.Y - 1 - PSize / 2, PSize + 2, PSize + 2);
                case ShapeType.NearestNeighbor:
                    return new System.Windows.Rect(p.X - 1 - PSize / 2, p.Y - 1 - PSize / 2, PSize + 2, PSize + 2);
                case ShapeType.Default:
                case ShapeType.Single:
                default:
                    return new System.Windows.Rect(p.X - PSize / 2, p.Y - PSize / 2, PSize, PSize);
            }
        }

        public static SolidColorBrush GetColor(P p, ShapeType st = ShapeType.Default)
        {
            switch (st)
            {
                case ShapeType.Single:
                    return Pens.DotColorSingle;
                case ShapeType.NearestNeighbor:
                    return Pens.DotColorNearestNeighbor;
                case ShapeType.Selected:
                    return Pens.DotColorSelected;
                case ShapeType.Default:
                default:
                    return Pens.DotColor;
            }
        }
      
        public static void RedrawDots(DrawingContext dc, ICollection<P> dots, ShapeType t = ShapeType.Default)
        {
            if (!IsDrawEnabled) return;

            foreach (var p in dots.Where(p => p != null && p.Visible))
            {
                dc.DrawRectangle(Pens.BackgroundColor, null, GetShape(p, t));
                dc.DrawRectangle(GetColor(p, ShapeType.Default), null, GetShape(p, ShapeType.Default));
            }
        }


        public static void ClearDots(DrawingContext dc, ICollection<P> dots, ShapeType t = ShapeType.Default)
        {
            if (!IsDrawEnabled) return;

            foreach (var p in dots.Where(p => p != null && p.Visible))
            {
                dc.DrawRectangle(Pens.BackgroundColor, null, GetShape(p, t));
            }
        }

        public static void ClearBackground(DrawingContext dc, int x, int y, int w, int h)
        {
            if (!IsDrawEnabled) return;

            dc.DrawRectangle(Pens.BackgroundColor, null, new System.Windows.Rect(x, y, w, h));
        }

        public static void DrawDots(DrawingContext dc, ICollection<P> dots, ShapeType t = ShapeType.Default)
        {
            if (!IsDrawEnabled) return;

            foreach (var p in dots.Where(p => p != null && p.Visible))
            {
                dc.DrawRectangle(GetColor(p, t), null, GetShape(p, t));
            }
        }

        public static void DrawGrid(DrawingContext dc, bool showGrid, int width, int height, int xgrid, int ygrid)
        {
            if (!IsDrawEnabled) return;

            var pen = showGrid ? Pens.PenGrid : Pens.PenBackground;
            double dw = width / (double)xgrid; // delta w
            for (var i = 0; i <= xgrid; i++)
            {
                dc.DrawLine(pen, new Point(M.Round(i * dw), 0), new Point(M.Round(i * dw), height));
            }
            var dh = height / (double)ygrid; // delta h
            for (var i = 0; i <= ygrid; i++)
            {
                dc.DrawLine(pen, new Point(0, M.Round(i * dh)), new Point(width, M.Round(i * dh)));
            }
        }
    }
}
