using FDCreator.Misc;

namespace FDCreator.Logic.SmartTools
{
    public class Gdis : ISmartTool, ITopPart, IMiddlePart, IBottomPart
    {
        public IParsedData Top { get; set; }
        public IParsedData Middle { get; set; }
        public IParsedData Bottom { get; set; }
        public SmartToolType Type => SmartToolType.Gdis;
    }
}