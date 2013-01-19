using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using SingleDetectLibrary.Code.Contract;
using SingleDetectLibrary.Code.Data;
using SingleDetectLibrary.Code.Grid;
using SingleDetectLibrary.Code.Logging;
using SingleDetectLibrary.Code.MathUtil;

namespace SingleDetectLibrary.Code
{
    public class SingleDetectAlgorithm : ISingleDetectAlgorithm
    {        
        private readonly Rectangle _rect;
        private readonly ILog2 _log;
        public List<P> Points { get; private set; }
        public List<P> Singles { get; private set; }

        public int XGrid { get; private set; }
        public int YGrid { get; private set; }
        public double MaxDistance { get; private set; }
        public double Square { get; private set; }        

        private readonly GridContainer _gridContainer;
               
        public SingleDetectAlgorithm(List<P> points, Rectangle rect, double distance, ILog2 log = null)
        {
            MaxDistance = distance;
            Square = M.CtoA(MaxDistance);
            _rect = rect;
            
            XGrid = (int)(Math.Ceiling(rect.Width / Square));
            YGrid = (int)(Math.Ceiling(rect.Height / Square));

            Points = points;
            Singles = new List<P>();

            _log = log ?? new NoLog();
            _gridContainer = new GridContainer(XGrid, YGrid, Square, Points, rect);

            //_log.Info(MethodBase.GetCurrentMethod(), "object init");
        }
        
        public void UpdateGrid(P p)
        {
            _gridContainer.Update(p);
        }

        public long UpdateSingles()
        {
            return UpdateSinglesGridNeighbor();
            //return UpdateSinglesNaive();
        }

        long UpdateSinglesGridNeighbor()
        {
            var sw = new Stopwatch();
            sw.Start();

            var gridSingles = _gridContainer.GetGridSingles();
            Singles.Clear();

            foreach (var p in gridSingles)
            {
                var neighbors = _gridContainer.GetGridNeighborContent(p);
                var add = neighbors
                    .Select(p.Distance)
                    .All(dist => dist > MaxDistance);

                if (add) Singles.Add(p);
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        long UpdateSinglesNaive()
        {
            var sw = new Stopwatch();
            sw.Start();

            Singles.Clear();

            var n = Points.Count;
            for (var i = 0; i < n; i++)
            {
                var p1 = Points[i];
                var add = true;
                for (var j = 0; j < n; j++)
                {
                    if (i == j) continue;

                    var p2 = Points[j];
                    var dist = p1.Distance(p2);
                    if (!(dist > MaxDistance))
                    {
                        add = false;
                        break;
                    }
                }
                if (add) Singles.Add(p1);
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }        
    }
}
