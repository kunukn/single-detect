using System;
using System.Collections.Generic;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;

namespace Kunukn.SingleDetectGuiCore.Code.Gui
{
    public class Animation
    {        
        private readonly ISingleDetectAlgorithm _algo;
        private readonly Rectangle _rect;
        readonly Random _rand = new Random();

        public HashSet<IP> Moving { get; private set; }

        public Animation(ISingleDetectAlgorithm algorithm, Rectangle rect)
        {
            _algo = algorithm;
            _rect = rect;            
            Moving = new HashSet<IP>();
        }
        
        public void SelectMovingDots(int n)
        {
            Moving.Clear();
            for (var i = 0; i < n; i++)
            {
                var a = _rand.Next(_algo.Points.Count);
                Moving.Add(_algo.Points[a]);
            }
        }

        public void UpdateMovingPosition(int min, int max)
        {
            if (min == 0 && max == 0) return;

            // Update pos
            foreach (var p in Moving)
            {
                var x = _rand.Next(min, max);
                var y = _rand.Next(min, max);
                if (x == 0 && y == 0) continue;

                // update p
                p.X += x;
                p.Y += y;
                if (p.X <= _rect.XMin) { p.X = _rect.XMin + 1; }
                if (p.Y <= _rect.YMin) { p.Y = _rect.YMin + 1; }
                if (p.X >= _rect.XMax) { p.X = _rect.XMax - 1; }
                if (p.Y >= _rect.YMax) { p.Y = _rect.YMax - 1; }

                _algo.UpdateGrid(p);
            }
        }
    }
}
