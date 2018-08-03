using FDCreator.Logic.Interfaces;

namespace FDCreator.Logic.Implementations
{
    public class StablizerExcelProcessorNpoiVersionCreator:IExcelProcessorCreator
    {
        public IExcelProcessor GetProcessor()
        {
            return new StablizerExcelProcessorNpoiVersion();
        }
    }
}