using System;
using System.Runtime.Serialization;
using Kunukn.SingleDetectLibrary.Code.Data;

namespace Kunukn.SingleDetectLibrary.Code.Contract
{    
    public interface IP : ISerializable
    {
        int Uid { get; }   
        double X { get; set; }
        double Y { get; set; }        
        double Distance(double x, double y);             
        GridIndex GridIndex { get; set; }
        //Categories Categories { get; set; }
        int Type { get; set; }

        object Data { get; set; } // Data container for anything, use it as you like
    }
}
