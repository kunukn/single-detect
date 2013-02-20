namespace Kunukn.SingleDetectLibrary.Code.Data
{
    public class KnnConfiguration
    {
        public int K { get; set; }
        public bool SameTypeOnly { get; set; }
        public double? MaxDistance { get; set; }

        public KnnConfiguration()
        {
            K = 1;
            SameTypeOnly = false;
            MaxDistance = null;            
        }
    }
}
