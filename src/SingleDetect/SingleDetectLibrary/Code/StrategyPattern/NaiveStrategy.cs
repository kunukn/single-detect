using System.Diagnostics;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;
using Kunukn.SingleDetectLibrary.Code.Logging;

namespace Kunukn.SingleDetectLibrary.Code.StrategyPattern
{
    public class NaiveStrategy : IAlgorithmStrategy
    {
        private readonly ILog2 _log;

        public string Name
        {
            get { return "Naive Strategy"; }
        }

        public NaiveStrategy(ILog2 log = null)
        {
            _log = log ?? new NoLog();
        }

        /// <summary>
        ///  O(n^2)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public long UpdateSingles(IAlgorithm s)
        {
            var sw = new Stopwatch();
            sw.Start();

            s.Singles.Clear();

            var n = s.Points.Count;
            for (var i = 0; i < n; i++)
            {
                var p1 = s.Points[i];
                var add = true;
                for (var j = 0; j < n; j++)
                {
                    if (i == j) continue;

                    var p2 = s.Points[j];
                    var dist = p1.Distance(p2.X, p2.Y);
                    if (!(dist > s.Rectangle.MaxDistance))
                    {
                        add = false;
                        break;
                    }
                }
                if (add) s.Singles.Add(p1);
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        /// <summary>        
        /// O(n * k * logk)  // much faster than O(n logn) for k << n
        /// </summary>
        /// <param name="s"></param>
        /// <param name="p"></param>       
        /// <param name="conf"></param>
        /// <returns></returns>
        public long UpdateKnn(IAlgorithm s, IP p, KnnConfiguration conf)
        {
            conf = conf ?? new KnnConfiguration();

            var sw = new Stopwatch();
            sw.Start();

            s.Knn.Clear();
            s.Knn.Origin = p;
            s.Knn.K = conf.K;

            //var all = new List<IPDist>(); // naive version
            var sortedList2 = new SortedList2();  // better naive version

            var n = s.Points.Count;
            for (var i = 0; i < n; i++)
            {
                var p1 = s.Points[i];
                if (p.Equals(p1)) continue; // don't include origin
                if (conf.SameTypeOnly && p.Type != p1.Type) continue; // only same type used

                var dist = p.Distance(p1.X, p1.Y);
                if (dist >= conf.MaxDistance) continue;

                var pdist = new PDist { Point = p1, Distance = dist };

                //all.Add(pdist); // naive version
                sortedList2.Add(pdist, conf.K);  // better naive version
            }

            //s.Knn.NNs = all.OrderBy(i => i.Distance).Take(conf.K).ToList(); // O(n logn)
            s.Knn.NNs = sortedList2.GetAll(); // O(n * k * logk) // better naive version
            
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }        
    }
}
