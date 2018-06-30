namespace EDCreator.Logic
{
    public class FloatExcelProcessor:ExcelProcessor
    {
        public FloatExcelProcessor()
        {
            TemplateFileName = "FloatSub Diagram.xlsx";
        }

        //Класс наследует от ExcelProcessor. Методы PassDataToExcel и SetCellValue здесь незримо присутствуют и выполнняются так же, как и в 
        //базовом классе (т.е. ExcelProcessor)
    }
}