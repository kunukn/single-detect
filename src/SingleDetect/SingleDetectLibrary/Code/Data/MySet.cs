using System;
using System.Collections.Generic;

namespace Kunukn.SingleDetectLibrary.Code.Data
{
    public class MySet<T> : HashSet<T>
    {
        public new bool Add(T t)
        {
            if(base.Contains(t))
            {
                return false;

                throw new ApplicationException(
                    string.Format("Set already contains: {0}",t));
            }
            return base.Add(t);
        }
    }
}
