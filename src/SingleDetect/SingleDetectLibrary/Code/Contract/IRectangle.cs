namespace SingleDetectLibrary.Code.Contract
{
    public interface IRectangle
    {
        double XMin { get; set; }
        double XMax { get; set; }
        double YMin { get; set; }
        double YMax { get; set; }

        double Width { get; }
        double Height { get; }

        double MaxDistance { get; set; }
        double Square { get; }
        int XGrid { get; }
        int YGrid { get; }

        // Only draw on positive space, offset
        double XO { get; }
        // Only draw on positive space, offset
        double YO { get; }

        void Validate();
    }
}
