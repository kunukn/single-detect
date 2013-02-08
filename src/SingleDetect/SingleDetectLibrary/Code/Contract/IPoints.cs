using System;
using System.Collections.Generic;

namespace SingleDetectLibrary.Code.Contract
{
    public interface IPoints
    {
        List<IP> Data { get; set; }
        void Round(int decimals);
        int Count { get; }        
        IP this[int i] { get; set; }
        void Add(IP p);
    }
}
