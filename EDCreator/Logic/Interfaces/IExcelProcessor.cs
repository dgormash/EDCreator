using FDCreator.Misc;

namespace FDCreator.Logic.Interfaces
{
    public interface IExcelProcessor
    {
        string TemplateFileName { get; set; }
        void CreateFishingDiagram(IParsedData data);
    }
}