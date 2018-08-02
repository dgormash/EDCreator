using FDCreator.Logic.Interfaces;
using FDCreator.Misc;

namespace FDCreator.Logic.Implementations
{
    public class StandartPdfProcessor:IPdfProcessor
    {
        private IConnectionInfoAssembler _connetionInfoAssembler = new ConnectionInfoAssembler();
        public IParsedData GetParsedDataFromPdf(string file)
        {
            throw new System.NotImplementedException();
        }
    }
}