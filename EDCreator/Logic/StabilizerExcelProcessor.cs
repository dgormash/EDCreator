using System.IO;
using System.Reflection;
using EDCreator.Misc;
using NPOI.XSSF.UserModel;

namespace EDCreator.Logic
{
    public class StabilizerExcelProcessor : ExcelProcessor
    {
        public StabilizerExcelProcessor()
        {
            TemplateFileName = "Stabilizer Diagram.xlsx";
        }

        //В шаблоне Stabilizer Diagram.xlsx прям совсем другая структура, поэтому методе PassDataToExcel необходимо переопределить

        public override void PassDataToExcel(ParsedData data)
        {
            var stabilizerData = (StabilizerParsedData) data;

            if (string.IsNullOrEmpty(TemplateFileName)) return;

            var filePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\misc\{TemplateFileName}";

            using (
                var file =
                    new FileStream(filePath,
                        FileMode.Open, FileAccess.Read))
            {
                Book = new XSSFWorkbook(file);
            }

            Sheet = Book.GetSheetAt(0);

            //Запись заголовка
            FillHeader(stabilizerData);

            //cellNum - Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные, соответстует столбцу G в шаблоне
            var cellNum = 7;

            //SerialNumber
            SetCellValue(15, cellNum, stabilizerData.SerialNumber);
            //TOP
            SetCellValue(17, cellNum, stabilizerData.ConnectionOne.TreadSize);
            //BOT
            SetCellValue(18, cellNum, stabilizerData.ConnectionTwo.TreadSize);
            //L
            SetCellValue(23, cellNum, stabilizerData.Length);
            //L1
            SetCellValue(24, cellNum, stabilizerData.FishingNeckTongSpace);
            //OD
            SetCellValue(30, cellNum, stabilizerData.ConnectionOne.Od);
            //ID
            SetCellValue(31, cellNum, stabilizerData.ConnectionTwo.Id);
            //MaxOD
            SetCellValue(32, cellNum, stabilizerData.StabilizerOd);
            //BladeLength
            SetCellValue(33, cellNum, stabilizerData.LobeLength);
            //BladeWidth
            SetCellValue(35, cellNum, stabilizerData.LobeWidth);

            //Сохранение изменённого файла
            using (
                var file =
                    new FileStream(
                        $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\out\{
                            stabilizerData.Name}_{stabilizerData.SerialNumber}_FinishedDiagram.xlsx",
                        FileMode.Create, FileAccess.Write))
            {
                Book.Write(file);
            }
        }
    }
}