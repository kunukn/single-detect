using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SingleDetectLibrary.Code.Contract;
using SingleDetectLibrary.Code.Data;
using SingleDetectLibrary.Code.Grid;

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
            var max = Math.Max(s.Rect.XGrid, s.Rect.YGrid);

            s.Knn.Clear();
            s.Knn.Origin = p;
            s.Knn.K = k;            
            UpdateKnnGridStrategy(s.GridContainer,s.Knn,s.Rect.Square,max);

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // K nearest neighbor
        protected void UpdateKnnGridStrategy(GridContainer g, NearestNeighbor nn, double square, int max)
        {            
            var currRing = new List<PDist>();
            var nextRing = new List<PDist>();

            for (var i = 1; i <= max; i++)
            {
                var temp = new List<PDist>();
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

                foreach (var p in list)
                {
                    var dist = nn.Origin.Distance(p);
                    if (dist < i * square) currRing.Add(new PDist { Point = p, Distance = dist });
                    else nextRing.Add(new PDist { Point = p, Distance = dist });
                }

                if (currRing.Count >= nn.K) break;
            }


            if (currRing.Count < nn.K)
            {
                currRing.AddRange(nextRing);
            }

            currRing.Sort();
            nn.NNs = currRing.Count > nn.K ? currRing.Take(nn.K).ToList() : currRing.ToList();
        }
    }
}
