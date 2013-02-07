using System.Collections.Generic;
using SingleDetectLibrary.Code.Data;
using SingleDetectLibrary.Code.Grid;
using SingleDetectLibrary.Code.StrategyPattern;

namespace SingleDetectLibrary.Code.Contract
{
    public interface ISingleDetectAlgorithm
    {
        // Shared
        List<P> Points { get; }
        Rectangle Rect_ { get; }        
        void UpdateGrid(P p);
        GridContainer GridContainer { get; }
        AlgorithmStrategy Strategy { get; }
        void SetAlgorithmStrategy(AlgorithmStrategy algorithmStrategy);

        // Single select
        long UpdateSingles();
        List<P> Singles { get; }

        // KNN
        long UpdateKnn(P p, int k);                        
        NearestNeighbor Knn { get;}        
    }
}
