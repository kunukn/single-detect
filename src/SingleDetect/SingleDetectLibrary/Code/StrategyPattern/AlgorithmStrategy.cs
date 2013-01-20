using SingleDetectLibrary.Code.Contract;
using SingleDetectLibrary.Code.Data;

namespace SingleDetectLibrary.Code.StrategyPattern
{
    public abstract class AlgorithmStrategy
    {
        public abstract string Name { get; }
        public abstract long UpdateSingles(ISingleDetectAlgorithm s);
        public abstract long UpdateKnn(ISingleDetectAlgorithm s, P p, int k);
    }
}
