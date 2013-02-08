using System;
using System.Reflection;

namespace Kunukn.SingleDetectLibrary.Code.Contract
{
    public interface ILog2
    {
        void Error(MethodBase m, string s);
        void Error(MethodBase m, Exception e);
        void Info(MethodBase m, string s);        
    }
}
