using System;
using System.Diagnostics;
using System.Linq;
using SingleDetectLibrary.Code.Contract;
using SingleDetectLibrary.Code.Data;

namespace SingleDetectLibrary.Code.StrategyPattern
{
    public class GridStrategy : AlgorithmStrategy
    {        
        public override string Name
        {
            get { return "Grid Strategy"; }
        }
        
        /// <summary>
        /// O(n * m)  where m is grid cells 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public override long UpdateSingles(ISingleDetectAlgorithm s)
        {
            var sw = new Stopwatch();
            sw.Start();

            var gridSingles = s.GridContainer.GetGridSingles();
            s.Singles.Clear();

            foreach (var p in gridSingles)
            {
                var neighbors = s.GridContainer.GetGridNeighborContent(p);
                var add = neighbors
                    .Select(p.Distance)
                    .All(dist => dist > s.Rect.MaxDistance);

                if (add) s.Singles.Add(p);
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // O(n * m) where m is grid cells
        public override long UpdateKnn(ISingleDetectAlgorithm s, P p, int k)
        {
            var sw = new Stopwatch();
            sw.Start();
            int max = Math.Max(s.Rect.XGrid, s.Rect.YGrid);

            s.Knn.Clear();
            s.Knn.Origin = p;
            s.Knn.K = k;
            s.GridContainer.UpdateKnnGridStrategy(s.Knn, s.Rect.Square, max);

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
    }
}
