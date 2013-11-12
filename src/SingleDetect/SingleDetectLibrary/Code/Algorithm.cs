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
        private readonly ILog2 _log;
        public IRectangle Rectangle { get; private set; }        
        public IList<IP> Points { get; private set; }
        public IList<IP> Singles { get; private set; }
        public NearestNeighbor Knn { get; private set; }               
        public GridContainer GridContainer { get; private set; }
        public IAlgorithmStrategy Strategy { get; private set; }
        
        public Algorithm(
            IPoints points, IRectangle rect, StrategyType type = StrategyType.Grid, ILog2 log = null)
        {
            _log = log ?? new NoLog();
            Rectangle = rect;           
            Points = points.Data;          
            Singles = new List<IP>();
            Knn = new NearestNeighbor();

            GridContainer = new GridContainer(Rectangle, Points);

            switch (type)
            {
                case StrategyType.Naive:
                    Strategy = new NaiveStrategy(_log);
                    break;
                case StrategyType.Grid:
                    Strategy = new GridStrategy(_log);
                    break;
                case StrategyType.KdTree:
                    Strategy = new KdTreeStrategy(Points);
                    break;
                default:
                    throw new NotImplementedException("Unknown strategy");
            }
                            
            //_log.Info(MethodBase.GetCurrentMethod(), "object init");
        }

        public void SetAlgorithmStrategy(IAlgorithmStrategy algorithmStrategy)
        {
            Strategy = algorithmStrategy;
        }     

        public long UpdateSingles()
        {
            return Strategy.UpdateSingles(this);            
        }
       
        public long UpdateKnn(IP p, KnnConfiguration configuration)
        {
            UpdateIndex(p);
            return Strategy.UpdateKnn(this, p, configuration);
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