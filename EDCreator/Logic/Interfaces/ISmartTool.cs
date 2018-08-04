using FDCreator.Misc;

namespace FDCreator.Logic.Interfaces
{
    public interface ISmartTool:ITopPart, IMiddlePart, IBottomPart
    {
        SmartToolType Type { get; }
    }
}