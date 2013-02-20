using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;

namespace Kunukn.SingleDetectLibrary.Code.StrategyPattern
{
    public abstract class AlgorithmStrategy
    {
        public abstract string Name { get; }
        public abstract long UpdateSingles(IAlgorithm s);
        public abstract long UpdateKnn(IAlgorithm s, IP p, KnnConfiguration configuration);
    }
}
