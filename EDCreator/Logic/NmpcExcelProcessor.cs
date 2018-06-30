using System.IO;
using System.Reflection;
using EDCreator.Misc;
using NPOI.XSSF.UserModel;

namespace EDCreator.Logic
{
    public class NmpcExcelProcessor:ExcelProcessor
    {
        public NmpcExcelProcessor()
        {
            TemplateFileName = "NMPC Diagram.xlsx";
        }

        //Класс наследует от ExcelProcessor. Методы PassDataToExcel и SetCellValue здесь незримо присутствуют и выполнняются так же, как и в 
        //базовом классе (т.е. ExcelProcessor)
    }
}