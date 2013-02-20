using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Kunukn.SingleDetectLibrary.Code.Contract
{
    public interface IPoints : ISerializable
    {
        List<IP> Data { get; set; }
        void Round(int decimals);
        int Count { get; }        
        IP this[int i] { get; set; }
        void Add(IP p);
    }
}
