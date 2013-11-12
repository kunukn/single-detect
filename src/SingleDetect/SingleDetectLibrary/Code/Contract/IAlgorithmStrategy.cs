using Kunukn.SingleDetectLibrary.Code.Data;

namespace Kunukn.SingleDetectLibrary.Code.Contract
{
    public interface IAlgorithmStrategy
    {
        string Name { get; }
        long UpdateSingles(IAlgorithm s);
        long UpdateKnn(IAlgorithm s, IP p, KnnConfiguration configuration);
    }
}
