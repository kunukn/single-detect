using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;
using Kunukn.SingleDetectLibrary.Code.Logging;

namespace Kunukn.SingleDetectLibrary.Code.StrategyPattern
{
    public class GridStrategy : IAlgorithmStrategy
    {        
        private readonly ILog2 _log;

        public string Name
        {
            get { return "Grid Strategy"; }
        }

        public GridStrategy(ILog2 log = null)
        {
            _log = log ?? new NoLog();
        }


        /// <summary>
        /// O(n * m)  where m is grid cells 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public long UpdateSingles(IAlgorithm s)
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
                    .All(dist => dist > s.Rectangle.MaxDistance);

                if (add) s.Singles.Add(p);
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // O(n * m) where m is grid cells
        public long UpdateKnn(IAlgorithm s, IP p, KnnConfiguration conf)
        {
            if (conf == null) conf = new KnnConfiguration();
            
            var sw = new Stopwatch();
            sw.Start();
            var max = Math.Max(s.Rectangle.XGrid, s.Rectangle.YGrid);

            s.Knn.Clear();
            s.Knn.Origin = p;
            s.Knn.K = conf.K;
            UpdateKnnGridStrategy(s, max, conf);

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        // K nearest neighbor
        protected void UpdateKnnGridStrategy(IAlgorithm s, int max, KnnConfiguration conf)
        {
            var g = s.GridContainer;
            var nn = s.Knn;
            var square = s.Rectangle.Square;

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
                if (conf.SameTypeOnly) list = list.Where(a => a.Type == nn.Origin.Type).ToList();

                var dataWasAdded = false;
                foreach (var p in list)
                {
                    var dist = nn.Origin.Distance(p.X, p.Y);
                    if(dist >= conf.MaxDistance) continue; // not within max distance

                    if (dist < i * square) currRing.Add(new PDist { Point = p, Distance = dist });
                    else nextRing.Add(new PDist { Point = p, Distance = dist });
                    dataWasAdded = true;
                }

                if(conf.MaxDistance.HasValue && !dataWasAdded) break; // max distance used and no new data was added, then we are done
                if (currRing.Count >= nn.K) break; // enough neighbors? then done
            }


            if (currRing.Count < nn.K)
            {
                // Only NN on same type if set
                currRing.AddRange(conf.SameTypeOnly
                                      ? nextRing.Where(a => a.Point.Type == nn.Origin.Type).ToList()
                                      : nextRing);

                if (conf.MaxDistance.HasValue) currRing = currRing.Where(i => i.Distance < conf.MaxDistance.Value).ToList();
            }
            
            currRing.Sort();
            nn.NNs = currRing.Count > nn.K ? currRing.Take(nn.K).ToList() : currRing.ToList();
        }
    }
}
