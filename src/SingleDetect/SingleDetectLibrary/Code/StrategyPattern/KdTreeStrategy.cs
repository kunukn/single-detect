using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Kunukn.SingleDetectLibrary.Code.Logging;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;
using KdTree;

namespace Kunukn.SingleDetectLibrary.Code.StrategyPattern
{
    public class KdTreeStrategy : IAlgorithmStrategy
    {
        private readonly ILog2 _log;
        public KdTree<Vector<double>, double> Tree { get; set; }

        public string Name
        {
            get { return "KdTree Strategy"; }
        }
        
        public KdTreeStrategy(IEnumerable<IP> points, ILog2 log = null)
        {
            _log = log ?? new NoLog();
            var vectors = points.Select(p => new DenseVector(new[] { p.X, p.Y })).ToList();
            Tree = KdTree<Vector<double>, double>.Construct(2, vectors.ToArray());
        }
                        
        public long UpdateSingles(IAlgorithm s)
        {
            var sw = new Stopwatch();
            sw.Start();

            var gridSingles = s.GridContainer.GetGridSingles();
            s.Singles.Clear();

            foreach (var p in gridSingles)
            {
                var vector = new DenseVector(new[] { p.X, p.Y });
                var nn = Tree.FindNearestNNeighbors(vector, 1).ToList();
                foreach (var i in nn)
                {
                    var x = i[0];
                    var y = i[1];
                    var dist = p.Distance(x, y);
                    if (dist > s.Rectangle.MaxDistance) s.Singles.Add(p);
                }
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
      
        /// <summary>
        /// Missing mapping to P objects
        /// KdTree could be refactored to use P object instead of Math.Net
        /// 
        /// O(k * log n) 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="origin"></param>
        /// <param name="k"></param>
        /// <param name="conf"></param>        
        /// <returns></returns>
        public long UpdateKnn(IAlgorithm s, IP origin, KnnConfiguration conf)
        {
            if (conf == null) conf = new KnnConfiguration();
            if (conf.SameTypeOnly) throw new NotImplementedException();
            if (conf.MaxDistance.HasValue) throw new NotImplementedException();

            var sw = new Stopwatch();
            sw.Start();

            var vector = new DenseVector(new[] { origin.X, origin.Y });
            var nn = Tree.FindNearestNNeighbors(vector, conf.K).ToList();

            s.Knn.Clear();
            s.Knn.Origin = origin;
            s.Knn.K = conf.K;
            foreach (var i in nn)
            {
                var p = new P { X = i[0], Y = i[1] };
                var dist = origin.Distance(p.X,p.Y);
                s.Knn.NNs.Add(new PDist {Point = p, Distance = dist});
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
    }
}
