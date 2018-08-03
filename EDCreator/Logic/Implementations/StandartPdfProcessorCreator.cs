using FDCreator.Logic.Interfaces;

namespace FDCreator.Logic.Implementations
{
    public class StandartPdfProcessorCreator:IPdfProcessorCreator
    {
        public IPdfProcessor GetProcessor()
        {
            return new StandartPdfProcessor();
        }
    }
}