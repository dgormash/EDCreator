using FDCreator.Logic.Interfaces;

namespace FDCreator.Logic.Implementations
{
    public class NmdcExcelProcessorNpoiVersionCreator:IExcelProcessorCreator
    {
        public IExcelProcessor GetProcessor()
        {
            return new NmdcExcelProcessorNpoiVersion();
        }
    }
}