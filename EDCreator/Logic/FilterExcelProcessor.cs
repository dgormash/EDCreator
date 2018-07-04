namespace FDCreator.Logic
{
    public class FilterExcelProcessor:ExcelProcessor
    {
        public FilterExcelProcessor()
        {
            //Я специально создаю потомков от ExcelProcessor, чтобы менять имя шаблона.
            //В каждом новом потомке меняйте эту строку на нужное значение
            TemplateFileName = "FilterSub Diagram.xlsx";
        }

        //Класс наследует от ExcelProcessor.Методы PassDataToExcel и SetCellValue, FillHeader и OpenExcelApp здесь незримо присутствуют и выполнняются так же, как и в
        //базовом классе (т.е. ExcelProcessor)
    }
}