using FDCreator.Misc;

namespace FDCreator.Logic.SmartTools
{
    public class Telescope:ISmartTool, ITopPart, IMiddlePart, IBottomPart
    {
        public IParsedData Top { get; set; }
        public IParsedData Middle { get; set; }
        public IParsedData Bottom { get; set; }
    }
}