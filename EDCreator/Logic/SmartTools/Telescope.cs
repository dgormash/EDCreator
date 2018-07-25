using FDCreator.Misc;

namespace FDCreator.Logic.SmartTools
{
    public class Telescope:ISmartTool
    {
        public IParsedData Top { get; set; }
        public IParsedData Middle { get; set; }
        public IParsedData Bottom { get; set; }
        public SmartToolType Type { get; set; }
    }
}