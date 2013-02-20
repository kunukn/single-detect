using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Kunukn.SingleDetectLibrary.Code;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;
using Kunukn.SingleDetectLibrary.Code.MathUtil;
using Kunukn.SingleDetectLibrary.Code.Util;

namespace Kunukn.SingleDetectGuiCore.Code.Gui
{
    public static class DrawUtil
    {
        public static bool IsDrawEnabled = true; // default
        private static int PSize = 2; // default, must be int

        public static void Init(int psize)
        {
            PSize = psize;
        }

        static System.Windows.Rect GetShape(IP p, Rectangle rect, ShapeType st = ShapeType.Default)
        {
            switch (st)
            {
                case ShapeType.Selected:
                    return new System.Windows.Rect((int)rect.XO + p.X - 1 - PSize / 2, (int)rect.YO + p.Y - 1 - PSize / 2, PSize + 2, PSize + 2);
                case ShapeType.NearestNeighbor:
                    return new System.Windows.Rect((int)rect.XO + p.X - 1 - PSize / 2, (int)rect.YO + p.Y - 1 - PSize / 2, PSize + 2, PSize + 2);
                case ShapeType.Default:
                case ShapeType.Single:
                default:
                    return new System.Windows.Rect((int)rect.XO + p.X - PSize / 2, (int)rect.YO + p.Y - PSize / 2, PSize, PSize);
            }
        }

        static SolidColorBrush GetColor(IP p, ShapeType st = ShapeType.Default)
        {
            switch (st)
            {
                case ShapeType.Single:
                    return Pens.DotColorSingle;
                case ShapeType.NearestNeighbor:
                    if (p.Type == 1) return Pens.DotColorType1;
                    if (p.Type == 2) return Pens.DotColorType2;
                    if (p.Type == 3) return Pens.DotColorType3;
                    return Pens.DotColorNearestNeighbor;
                case ShapeType.Selected:
                    return Pens.DotColorSelected;
                case ShapeType.Default:
                default:
                    if (p.Type == 1) return Pens.DotColorType1;
                    if (p.Type == 2) return Pens.DotColorType2;
                    if (p.Type == 3) return Pens.DotColorType3;
                    return Pens.DotColor; // default
            }
        }

        public static void RedrawDots(DrawingContext dc, ICollection<IP> dots, Rectangle rect, ShapeType t = ShapeType.Default)
        {
            if (!IsDrawEnabled) return;

            foreach (var p in dots.Where(p => p != null))
            {
                dc.DrawRectangle(Pens.BackgroundColor, null, GetShape(p, rect, t));
                dc.DrawRectangle(GetColor(p, ShapeType.Default), null, GetShape(p, rect, ShapeType.Default));
            }
        }


        public static void ClearDots(DrawingContext dc, ICollection<IP> dots, Rectangle rect, ShapeType t = ShapeType.Default)
        {
            if (!IsDrawEnabled) return;

            foreach (var p in dots.Where(p => p != null))
            {
                dc.DrawRectangle(Pens.BackgroundColor, null, GetShape(p, rect, t));
            }
        }

        public static void ClearBackground(DrawingContext dc, Rectangle rect)
        {
            if (!IsDrawEnabled) return;

            dc.DrawRectangle(Pens.BackgroundColor, null, new System.Windows.Rect(0, 0, (int)rect.Width, (int)rect.Height));
        }

        public static void DrawDots(DrawingContext dc, ICollection<IP> dots, Rectangle rect, ShapeType t = ShapeType.Default)
        {
            if (!IsDrawEnabled) return;

            foreach (var p in dots.Where(p => p != null))
            {
                dc.DrawRectangle(GetColor(p, t), null, GetShape(p, rect, t));
            }
        }

        public static void DrawGrid(DrawingContext dc, bool showGrid, Rectangle rect, Pen color = null)
        {
            if (!IsDrawEnabled) return;

            var pen = showGrid ? (color ?? Pens.PenGrid) : Pens.PenBackground;
            double dw = rect.Width / rect.XGrid; // delta w
            for (var i = 0; i <= rect.XGrid; i++)
            {
                dc.DrawLine(pen, new Point(M.Round(i * dw), 0), new Point(M.Round(i * dw), rect.Height));
            }
            var dh = rect.Height / rect.YGrid; // delta h
            for (var i = 0; i <= rect.YGrid; i++)
            {
                dc.DrawLine(pen, new Point(0, M.Round(i * dh)), new Point(rect.Width, M.Round(i * dh)));
            }
        }
    }
}
