using FDCreator.Logic.Interfaces;

namespace FDCreator.Logic.Implementations
{
    public class NearBitSubStablizerExcelProcessorNpoiVersionCreator:IExcelProcessorCreator
    {
        public IExcelProcessor GetProcessor()
        {
            return new NearBitSubStablizerExcelProcessorNpoiVersion();
        }
    }
}