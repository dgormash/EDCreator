using FDCreator.Logic.Interfaces;

namespace FDCreator.Logic.Implementations
{
    public class StandartExcelProcessorNpoiVersionCreator:IExcelProcessorCreator
    {
        public IExcelProcessor GetProcessor()
        {
            return new StandartExcelProcessorNpoiVersion();
        }
    }
}