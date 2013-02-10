using System;

namespace Kunukn.SingleDetectLibrary.Code
{    
    public enum StrategyType
    {
        Naive, Grid, KdTree
    };

    [Flags]
    public enum Categories
    {
        None = 0,
        Category01 = 1,
        Category02 = 2,
        Category03 = 4,
        Category04 = 8,
        Category05 = 16,
        Category06 = 32,
        Category07 = 64,
        Category08 = 128,
        Category09 = 256,
        Category10 = 512,
        Category11 = 1024,
        Category12 = 2048,
        Category13 = 4096,
        Category14 = 8192,
        Category15 = 16384,
        Category16 = 32768,
        Category17 = 65536,
        Category18 = 131072,
        Category19 = 262144,
        Category20 = 524288,
        Category21 = 1048576,
        Category22 = 2097152,
        Category23 = 4194304,
        Category24 = 8388608,
        Category25 = 16777216,
        Category26 = 33554432,
        Category27 = 67108864,
        Category28 = 134217728,
        Category29 = 268435456,
        Category30 = 536870912,        
    }
}
