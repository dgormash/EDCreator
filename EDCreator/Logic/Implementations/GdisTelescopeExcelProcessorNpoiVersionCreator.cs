using FDCreator.Logic.Interfaces;

namespace FDCreator.Logic.Implementations
{
    public class GdisTelescopeExcelProcessorNpoiVersionCreator:ISmartToolExcelProcessorCreator
    {
        public ISmartToolExcelProcessor GetProcessor()
        {
            return new GdisTelescopeExcelProcessorNpoiVersion();
        }
    }
}