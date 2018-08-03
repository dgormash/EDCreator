using FDCreator.Logic.Interfaces;

namespace FDCreator.Logic.Implementations
{
    public class NearBitSubExcelProcessorNpoiVersionCreator:IExcelProcessorCreator
    {
        public IExcelProcessor GetProcessor()
        {
            return new NearBitSubExcelProcessorNpoiVersion();
        }
    }
}