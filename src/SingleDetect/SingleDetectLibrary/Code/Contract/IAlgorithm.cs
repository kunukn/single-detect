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
        IRectangle Rectangle { get; }    // the world boundary      
        void UpdatePosition(IP p);  // update grid and gridbox ref for p
        void UpdateIndex(IP p);     // update gridbox ref for p
        GridContainer GridContainer { get; }
        IAlgorithmStrategy Strategy { get; }
        void SetAlgorithmStrategy(IAlgorithmStrategy algorithmStrategy); // set the algo type used for calc        

        // Single select
        long UpdateSingles();
        IList<IP> Singles { get; }

        // KNN
        long UpdateKnn(IP p, KnnConfiguration configuration);
        NearestNeighbor Knn { get;}
    }
}
