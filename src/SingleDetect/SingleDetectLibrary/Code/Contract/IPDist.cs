using System;
using SingleDetectLibrary.Code.Data;

namespace SingleDetectLibrary.Code.Contract
{
    public interface IPDist : IComparable
    {
        IP Point { get; set; }
        double Distance { get; set; }
    }
}
