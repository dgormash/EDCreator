namespace EDCreator.Logic
{
    public class FilterExcelProcessor:ExcelProcessor
    {
        public FilterExcelProcessor()
        {

            TemplateFileName = "FilterSub Diagram.xlsx";
        }

        //Класс наследует от ExcelProcessor.Методы PassDataToExcel и SetCellValue, FillHeader и OpenExcelApp здесь незримо присутствуют и выполнняются так же, как и в
        //базовом классе (т.е. ExcelProcessor)
    }
}