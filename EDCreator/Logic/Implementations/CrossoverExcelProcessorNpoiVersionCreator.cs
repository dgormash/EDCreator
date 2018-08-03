using FDCreator.Logic.Interfaces;

namespace FDCreator.Logic.Implementations
{
    public class CrossoverExcelProcessorNpoiVersionCreator:IExcelProcessorCreator
    {
        public IExcelProcessor GetProcessor()
        {
            return new CrossoverExcelProcessorNpoiVersion();
        }
    }
}