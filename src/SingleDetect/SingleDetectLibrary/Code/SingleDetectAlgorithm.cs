using System;
using System.Collections.Generic;
using SingleDetectLibrary.Code.Contract;
using SingleDetectLibrary.Code.Data;
using SingleDetectLibrary.Code.Grid;
using SingleDetectLibrary.Code.Logging;
using SingleDetectLibrary.Code.StrategyPattern;

namespace SingleDetectLibrary.Code
{
    /// <summary>
    /// Author: Kunuk Nykjaer    
    /// MIT license        
    /// </summary>
    public class SingleDetectAlgorithm : ISingleDetectAlgorithm
    {        
        public Rectangle Rect { get; private set; }
        private readonly ILog2 _log;
        public List<P> Points { get; private set; }
        public List<P> Singles { get; private set; }
        public NearestNeighbor Knn { get; private set; }               
        public GridContainer GridContainer { get; private set; }
        public AlgorithmStrategy Strategy { get; private set; }

        public SingleDetectAlgorithm(
            List<P> points, Rectangle rect, StrategyType type = StrategyType.Grid, ILog2 log = null)
        {
            _log = log ?? new NoLog();
            Rect = rect;           
            Points = points;          
            Singles = new List<P>();
            Knn = new NearestNeighbor();

            GridContainer = new GridContainer(Rect, Points);

            switch (type)
            {
                case StrategyType.Naive:
                    Strategy = new NaiveStrategy();
                    break;
                case StrategyType.Grid:
                    Strategy = new GridStrategy();
                    break;
                case StrategyType.KdTree:
                    Strategy = new KdTreeStrategy(Points);
                    break;
                default:
                    throw new NotImplementedException("Unknown strategy");
            }
                            
            //_log.Info(MethodBase.GetCurrentMethod(), "object init");
        }

        public void SetAlgorithmStrategy(AlgorithmStrategy algorithmStrategy)
        {
            Strategy = algorithmStrategy;
        }     

        public long UpdateSingles()
        {
            return Strategy.UpdateSingles(this);            
        }
       
        public long UpdateKnn(P p, int k)
        {
            return Strategy.UpdateKnn(this, p, k);            
        }

        // Used when p position has been updated
        public void UpdateGrid(P p)
        {
            GridContainer.Update(p);
        }
    }
}
