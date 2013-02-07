using SingleDetectLibrary.Code.Data;

namespace SingleDetectLibrary.Code.Contract
{
    public interface IP
    {
        double Distance(P p);
        double Distance(double x, double y);
        void Round(int decimals);
        int Uid { get; }
        bool Visible { get; set; }
        GridIndex GridIndex { get; set; }
        object Data { get; set; } // Data container for anything, use it as you like
    }
}
