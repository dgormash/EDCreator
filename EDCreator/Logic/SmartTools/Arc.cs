using FDCreator.Misc;

namespace FDCreator.Logic.SmartTools
{
    public class Arc : ISmartTool, ITopPart,  IBottomPart
    {
        public IParsedData Top { get; set; }
        public IParsedData Bottom { get; set; }
    }
}