using System.Collections.Generic;
using SingleDetectLibrary.Code.Data;
using SingleDetectLibrary.Code.Grid;
using SingleDetectLibrary.Code.StrategyPattern;

namespace SingleDetectLibrary.Code.Contract
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
