namespace Kunukn.SingleDetectLibrary.Code.Data
{
    public class GridIndex
    {
        public int X { get; set; }
        public int Y { get; set; }
        public override string ToString() { return X + ";" + Y; }
        public override int GetHashCode() { return ToString().GetHashCode(); }
        public override bool Equals(object obj)
        {
            var o = obj as GridIndex;
            if (o == null) { return false; }
            return GetHashCode() == o.GetHashCode();
        }
    }
}
