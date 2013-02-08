using System;
using Kunukn.SingleDetectLibrary.Code.Data;

namespace Kunukn.SingleDetectLibrary.Code.Contract
{
    public interface IPDist : IComparable
    {
        IP Point { get; set; }
        double Distance { get; set; }
    }
}
