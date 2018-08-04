using FDCreator.Logic.Interfaces;

namespace FDCreator.Logic.Implementations
{
    public class ArcExcelProcessorNpoiVersionCreator:ISmartToolExcelProcessorCreator
    {
        public ISmartToolExcelProcessor GetProcessor()
        {
            return new ArcExcelProcessorNpoiVersion();
        }
    }
}