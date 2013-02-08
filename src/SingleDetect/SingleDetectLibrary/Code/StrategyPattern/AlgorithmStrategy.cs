using SingleDetectLibrary.Code.Contract;

namespace SingleDetectLibrary.Code.StrategyPattern
{
    public abstract class AlgorithmStrategy
    {
        public abstract string Name { get; }
        public abstract long UpdateSingles(ISingleDetectAlgorithm s);
        public abstract long UpdateKnn(ISingleDetectAlgorithm s, IP p, int k);
    }
}
