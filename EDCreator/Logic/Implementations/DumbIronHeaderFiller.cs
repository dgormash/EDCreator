using FDCreator.Logic.Interfaces;
using FDCreator.Misc;

namespace FDCreator.Logic.Implementations
{
    public class DumbIronHeaderFiller:IHeaderFiller
    {
        public void FillHeader(IParsedData data, ICellValueWriter cellWriter)
        {
           var cellNum = 2;

            //Запись заголовка
            cellWriter.SetCellValue(4, cellNum, data.Header.ClientField);
            cellWriter.SetCellValue(5, cellNum, data.Header.FieldPadWellField);
            cellWriter.SetCellValue(6, cellNum, data.Header.LocationField);

            cellNum = 8;
            cellWriter.SetCellValue(4, cellNum, data.Header.DdEngineerField);
            cellWriter.SetCellValue(5, cellNum, data.Header.DateField);
        }
    }
}