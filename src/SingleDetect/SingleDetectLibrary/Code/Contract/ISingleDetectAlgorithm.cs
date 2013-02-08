using System.Collections.Generic;
using Kunukn.SingleDetectLibrary.Code.Data;
using Kunukn.SingleDetectLibrary.Code.Grid;
using Kunukn.SingleDetectLibrary.Code.StrategyPattern;

namespace Kunukn.SingleDetectLibrary.Code.Contract
{
    public interface ISingleDetectAlgorithm
    {
        // Shared
        List<IP> Points { get; }
        Rectangle Rect_ { get; }        
        void UpdateGrid(IP p);
        GridContainer GridContainer { get; }
        AlgorithmStrategy Strategy { get; }
        void SetAlgorithmStrategy(AlgorithmStrategy algorithmStrategy);

        // Single select
        long UpdateSingles();
        List<IP> Singles { get; }

        // KNN
        long UpdateKnn(IP p, int k);                        
        NearestNeighbor Knn { get;}        
    }
}
