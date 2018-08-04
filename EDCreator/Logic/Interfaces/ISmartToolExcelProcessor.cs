namespace FDCreator.Logic.Interfaces
{
    public interface ISmartToolExcelProcessor
    {
        string TemplateFileName { get; set; }
        void CreateFishingDiagram(ISmartTool smartTool);
    }
}