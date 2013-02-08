using System;
using System.Runtime.Serialization;

namespace Kunukn.SingleDetectLibrary.Code.Contract
{
    public interface IPDist : IComparable, ISerializable
    {
        IP Point { get; set; }
        double Distance { get; set; }
    }
}
