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
        public override long UpdateSingles(ISingleDetectAlgorithm s)
        {
            var sw = new Stopwatch();
            sw.Start();

            var gridSingles = s.GridContainer.GetGridSingles();
            s.Singles.Clear();

            foreach (IP p in gridSingles)
            {
                var neighbors = s.GridContainer.GetGridNeighborContent(p);
                var add = neighbors
                    .Select(i => i.Distance(p.X,p.Y))
                    .All(dist => dist > s.Rect_.MaxDistance);

                if (add) s.Singles.Add(p);
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // O(n * m) where m is grid cells
        public override long UpdateKnn(ISingleDetectAlgorithm s, IP p, int k)
        {
            var sw = new Stopwatch();
            sw.Start();
            var max = Math.Max(s.Rect_.XGrid, s.Rect_.YGrid);

            s.Knn.Clear();
            s.Knn.Origin = p;
            s.Knn.K = k;            
            UpdateKnnGridStrategy(s.GridContainer,s.Knn,s.Rect_.Square,max);

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // K nearest neighbor
        protected void UpdateKnnGridStrategy(GridContainer g, NearestNeighbor nn, double square, int max)
        {            
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
                currRing.AddRange(nextRing);
            }

            currRing.Sort();            
            nn.NNs = new DistPoints { Data = currRing.Count > nn.K ? currRing.Take(nn.K).ToList() : currRing.ToList() };
        }
    }
}
