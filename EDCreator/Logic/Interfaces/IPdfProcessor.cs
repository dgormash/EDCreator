using FDCreator.Misc;

namespace FDCreator.Logic.Interfaces
{
    public interface IPdfProcessor
    {
        IParsedData GetParsedDataFromPdf(string file);
    }
}