using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kunukn.SingleDetectLibrary.Code.Util
{
    /// <summary>
    /// http://stackoverflow.com/questions/3261451/using-a-bitmask-in-c-sharp
    /// </summary>
    public static class CategoryUtil
    {
        public static bool IsSet<T>(T flags, T flag) where T : struct
        {
            var flagsValue = (int)(object)flags;
            var flagValue = (int)(object)flag;

            return (flagsValue & flagValue) != 0;
        }

        public static void Set<T>(ref T flags, T flag) where T : struct
        {
            var flagsValue = (int)(object)flags;
            var flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue | flagValue);
        }

        public static void Unset<T>(ref T flags, T flag) where T : struct
        {
            var flagsValue = (int)(object)flags;
            var flagValue = (int)(object)flag;

            flags = (T)(object)(flagsValue & (~flagValue));
        }
    }
}
