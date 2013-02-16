using System.Collections.Generic;
using Kunukn.SingleDetectLibrary.Code.Data;
using Kunukn.SingleDetectLibrary.Code.Grid;
using Kunukn.SingleDetectLibrary.Code.StrategyPattern;

namespace Kunukn.SingleDetectLibrary.Code.Contract
{
    public interface IAlgorithm
    {
        // Shared
        IList<IP> Points { get; }    // all the points
        IRectangle Rect_ { get; }    // the world boundary      
        void UpdatePosition(IP p);  // update grid and gridbox ref for p
        void UpdateIndex(IP p);     // update gridbox ref for p
        GridContainer GridContainer { get; }
        AlgorithmStrategy Strategy { get; }
        void SetAlgorithmStrategy(AlgorithmStrategy algorithmStrategy); // set the algo type used for calc        

        // Single select
        long UpdateSingles();
        IList<IP> Singles { get; }

        // KNN
        long UpdateKnn(IP p, int k, bool knnSameTypeOnly = false);
        NearestNeighbor Knn { get;}
    }
}
