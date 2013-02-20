using System;
using System.Collections.Generic;
using System.Linq;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;

namespace Kunukn.SingleDetectLibrary.Code.Util
{    
    public static class PointUtil
    {
        public static IList<IP> Exclusive(IList<IP> source, IList<IP> candidate)
        {
            var set = new HashSet<IP>();
            foreach (var p in source) set.Add(p);
            IList<IP> exclusive = candidate.Where(p => !set.Contains(p)).ToList();
            return exclusive;
        }
    }
}
