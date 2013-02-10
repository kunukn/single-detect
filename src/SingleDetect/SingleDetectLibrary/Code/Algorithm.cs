using System;
using System.Collections.Generic;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;
using Kunukn.SingleDetectLibrary.Code.Grid;
using Kunukn.SingleDetectLibrary.Code.Logging;
using Kunukn.SingleDetectLibrary.Code.StrategyPattern;

namespace Kunukn.SingleDetectLibrary.Code
{
    /// <summary>
    /// Author: Kunuk Nykjaer    
    /// MIT license        
    /// </summary>
    public class Algorithm : IAlgorithm
    {        
        public Rectangle Rect_ { get; private set; }
        private readonly ILog2 _log;
        public List<IP> Points { get; private set; }
        public List<IP> Singles { get; private set; }
        public NearestNeighbor Knn { get; private set; }               
        public GridContainer GridContainer { get; private set; }
        public AlgorithmStrategy Strategy { get; private set; }
        public bool KnnSameTypeOnly { get; set; }

        public Algorithm(
            IPoints points, Rectangle rect, StrategyType type = StrategyType.Grid, ILog2 log = null)
        {
            _log = log ?? new NoLog();
            Rect_ = rect;           
            Points = points.Data;          
            Singles = new List<IP>();
            Knn = new NearestNeighbor();

            GridContainer = new GridContainer(Rect_, Points);

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
       
        public long UpdateKnn(IP p, int k)
        {
            UpdateIndex(p);
            return Strategy.UpdateKnn(this, p, k);            
        }

        // Used when p position has been updated
        public void UpdatePosition(IP p)
        {
            this.GridContainer.UpdatePosition(p);
        }

        public void UpdateIndex(IP p)
        {
            this.GridContainer.UpdateIndex(p);
        }
    }
}