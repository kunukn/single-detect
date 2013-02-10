using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;
using Kunukn.SingleDetectLibrary.Code.Grid;

namespace Kunukn.SingleDetectLibrary.Code.StrategyPattern
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
        public override long UpdateSingles(IAlgorithm s)
        {
            var sw = new Stopwatch();
            sw.Start();

            var gridSingles = s.GridContainer.GetGridSingles();
            s.Singles.Clear();

            foreach (IP p in gridSingles)
            {
                var neighbors = s.GridContainer.GetGridNeighborContent(p);
                var add = neighbors
                    .Select(i => i.Distance(p.X, p.Y))
                    .All(dist => dist > s.Rect_.MaxDistance);

                if (add) s.Singles.Add(p);
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // O(n * m) where m is grid cells
        public override long UpdateKnn(IAlgorithm s, IP p, int k)
        {
            var sw = new Stopwatch();
            sw.Start();
            var max = Math.Max(s.Rect_.XGrid, s.Rect_.YGrid);

            s.Knn.Clear();
            s.Knn.Origin = p;
            s.Knn.K = k;
            UpdateKnnGridStrategy(s, max);

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // K nearest neighbor
        protected void UpdateKnnGridStrategy(IAlgorithm s, int max)
        {
            var g = s.GridContainer;
            var nn = s.Knn;
            var square = s.Rect_.Square;

            var currRing = new List<IPDist>();
            var nextRing = new List<IPDist>();

            for (var i = 1; i <= max; i++)
            {
                var temp = new List<IPDist>();
                foreach (var p in nextRing)
                {
                    if (p.Distance < i * square) currRing.Add(p);
                    else temp.Add(p);
                }

                nextRing.Clear();
                nextRing.AddRange(temp);

                var list = g.GetRing(nn.Origin, i);

                // First 9 squares, dont include origin
                if (i == 1) list.AddRange(g.GetSet(nn.Origin).Where(a => !a.Equals(nn.Origin)).ToList());

                // Only NN on same type if set
                if (s.KnnSameTypeOnly) list = list.Where(a => a.Type == nn.Origin.Type).ToList();

                foreach (var p in list)
                {
                    var dist = nn.Origin.Distance(p.X, p.Y);
                    if (dist < i * square) currRing.Add(new PDist { Point = p, Distance = dist });
                    else nextRing.Add(new PDist { Point = p, Distance = dist });
                }

                if (currRing.Count >= nn.K) break;
            }


            if (currRing.Count < nn.K)
            {
                // Only NN on same type if set
                currRing.AddRange(s.KnnSameTypeOnly
                                      ? nextRing.Where(a => a.Point.Type == nn.Origin.Type).ToList()
                                      : nextRing);
            }

            currRing.Sort();
            nn.NNs = currRing.Count > nn.K ? currRing.Take(nn.K).ToList() : currRing.ToList();
        }
    }
}
