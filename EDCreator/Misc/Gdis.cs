using FDCreator.Logic.Interfaces;

namespace FDCreator.Misc
{
    public class Gdis : ISmartTool
    {
        public IParsedData Top { get; set; }
        public IParsedData Middle { get; set; }
        public IParsedData Bottom { get; set; }
        public SmartToolType Type => SmartToolType.Gdis;
    }
}