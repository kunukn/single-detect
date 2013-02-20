using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;

namespace Kunukn.SingleDetectLibrary.Code.StrategyPattern
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
        public override long UpdateSingles(IAlgorithm s)
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
                    if (!(dist > s.Rect_.MaxDistance))
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
        /// Something like O(k * logk * n)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="p"></param>
        /// <param name="k"></param>
        /// <param name="conf"></param>
        /// <returns></returns>
        public override long UpdateKnn(IAlgorithm s, IP p, KnnConfiguration conf)
        {
            if (conf == null) conf = new KnnConfiguration();

            var sw = new Stopwatch();
            sw.Start();

            s.Knn.Clear();
            s.Knn.Origin = p;
            s.Knn.K = conf.K;

            var set = new SortedSet<IPDist>();
            var debug = new List<IPDist>();

            var n = s.Points.Count;
            for (var i = 0; i < n; i++)
            {
                var p1 = s.Points[i];
                if (p.Equals(p1))  continue; // don't include origin
                if (conf.SameTypeOnly && p.Type != p1.Type)  continue; // only same type used

                var dist = p.Distance(p1.X, p1.Y);
                if (dist >= conf.MaxDistance) continue;
                
                var pdist = new PDist {Point = p1, Distance = dist};
                debug.Add(pdist);

                if (set.Count < conf.K) set.Add(pdist);
                else
                {                   
                    var m = set.Max;
                    if (dist < m.Distance)
                    {
                        // replace
                        set.Remove(m);
                        set.Add(pdist);
                    }
                }
            }

            s.Knn.NNs = set.ToList();

            //debug = debug.OrderBy(i => i.Distance).ToList();
            //Validate(s.Knn.NNs, debug);

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }


        void Validate(IList<IPDist> knn, IList<IPDist> all)
        {            
            for (int i = 0; i < knn.Count; i++)
            {
                var a = knn[i];
                var b = all[i];

                if(a.Point.Uid != b.Point.Uid)
                {
                    
                }
            }
        }
    }
}
