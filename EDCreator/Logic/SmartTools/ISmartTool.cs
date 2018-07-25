namespace FDCreator.Logic.SmartTools
{
    public interface ISmartTool:ITopPart, IMiddlePart, IBottomPart
    {
        SmartToolType Type { get; }
    }
}