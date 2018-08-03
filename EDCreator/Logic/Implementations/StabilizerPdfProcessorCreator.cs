using FDCreator.Logic.Interfaces;

namespace FDCreator.Logic.Implementations
{
    public class StabilizerPdfProcessorCreator:IPdfProcessorCreator
    {
        public IPdfProcessor GetProcessor()
        {
            return new StabilizerPdfProcessor();
        }
    }
}