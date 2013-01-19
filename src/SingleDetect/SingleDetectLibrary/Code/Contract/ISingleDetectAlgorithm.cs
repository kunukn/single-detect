using System.Collections.Generic;
using SingleDetectLibrary.Code.Data;

namespace SingleDetectLibrary.Code.Contract
{
    public interface ISingleDetectAlgorithm
    {
        long UpdateSingles();
        void UpdateGrid(P p);
        List<P> Points { get; }
        List<P> Singles { get; }
        int XGrid { get; }
        int YGrid { get; }
        double MaxDistance { get; }
        double Square { get; }
    }
}
