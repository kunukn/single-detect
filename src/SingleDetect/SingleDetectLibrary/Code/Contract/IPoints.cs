using System;
using System.Collections.Generic;
using SingleDetectLibrary.Code.Data;

namespace SingleDetectLibrary.Code.Contract
{
    public interface IPoints
    {
        List<P> Data { get; set; }
        void Round(int decimals);
    }
}
