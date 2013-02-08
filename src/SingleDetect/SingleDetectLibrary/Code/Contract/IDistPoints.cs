using System;
using System.Collections.Generic;

namespace Kunukn.SingleDetectLibrary.Code.Contract
{
    public interface IDistPoints
    {
        List<IPDist> Data { get; set; }
        int Count { get; }        
        IPDist this[int i] { get; set; }
        void Add(IPDist p);
    }
}
