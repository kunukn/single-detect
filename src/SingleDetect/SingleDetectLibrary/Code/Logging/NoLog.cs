using System;
using System.Reflection;
using Kunukn.SingleDetectLibrary.Code.Contract;

namespace Kunukn.SingleDetectLibrary.Code.Logging
{
    public class NoLog : ILog2
    {
        public void Error(MethodBase m, string s){}
        public void Error(MethodBase m, Exception e){}
        public void Info(MethodBase m, string s){}        
    }
}