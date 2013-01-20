using System.Diagnostics;
using System.Linq;
using SingleDetectLibrary.Code.Contract;
using SingleDetectLibrary.Code.Data;

namespace SingleDetectLibrary.Code.StrategyPattern
{
    public class NaiveStrategy : AlgorithmStrategy
    {
        public override string Name
        {
            get { return "Naive Strategy"; }
        }
        
        /// <summary>
        ///  O(n^2)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public override long UpdateSingles(ISingleDetectAlgorithm s)
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
                    var dist = p1.Distance(p2);
                    if (!(dist > s.Rect.MaxDistance))
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
        /// O(n log n)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="p"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public override long UpdateKnn(ISingleDetectAlgorithm s, P p, int k)
        {
            var sw = new Stopwatch();
            sw.Start();

            s.Knn.Clear();
            s.Knn.Origin = p;
            s.Knn.K = k;

            var n = s.Points.Count;
            for (var i = 0; i < n; i++)
            {
                var p1 = s.Points[i];
                if (p.Equals(p1)) continue;

                var dist = p.Distance(p1);
                s.Knn.NNs.Add(new PDist { Point = p1, Distance = dist });
            }

            s.Knn.NNs.Sort();
            k = k > n ? n : k;
            s.Knn.NNs = s.Knn.NNs.Take(k).ToList();

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
    }
}
