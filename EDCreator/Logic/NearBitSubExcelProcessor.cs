using System;
using System.IO;
using System.Reflection;
using System.Windows;
using FDCreator.Misc;
using NPOI.XSSF.UserModel;

namespace FDCreator.Logic
{
    internal class NearBitSubExcelProcessor : ExcelProcessor
    {
        public NearBitSubExcelProcessor(string sessionStartTime) : base(sessionStartTime)
        {
            TemplateFileName = "Near Bit Sub Diagram.xlsx";
        }

        public override void PassDataToExcel(IParsedData data)
        {
            if (string.IsNullOrEmpty(TemplateFileName)) return;

            var filePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\misc\{TemplateFileName}";
            try
            {
                using (
                    var file =
                        new FileStream(filePath,
                            FileMode.Open, FileAccess.Read))
                {
                    Book = new XSSFWorkbook(file);
                }

                Sheet = Book.GetSheetAt(0);
                Book.SetSheetName(Book.GetSheetIndex(Sheet), $"{data.Name}_{data.SerialNumber}");

                //Заполнение заголовка (метод прописан в ExcelProcessor.cs)
                FillHeader(data);

                //Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные (нумерация ячеек в коде начинается с 0)
                //Поэтому от номера строки и столбца нужно отнимать 1
                var cellNum = 5;


                //SerialNumber
                SetCellValue(14, cellNum, data.SerialNumber); //соответствует 22 строке в шаблоне и т.д.
                //L
                var inches = InchesValueRetriever.GetInchesValue(data.Length);

                SetCellValue(19, cellNum, LengthConverter.InchesToMeters(inches).ToString("0.000"));
                //OD1
                SetCellValue(20, cellNum, data.ConnectionOne.Od);
                //ID
                SetCellValue(21, cellNum, data.ConnectionTwo.Od);
                //Top
                SetCellValue(16, cellNum, data.ConnectionOne.TreadSize);
                //Bottom
                SetCellValue(17, cellNum, data.ConnectionTwo.TreadSize); //соответствует 27 строке в шаблоне

                //var totalFishingDiagram =
                //    TotalFishingDiagramFile.GetTotalFishingDiagramFileStream(
                //        $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\out\Field_Pad_Well_FishingDiagram_{
                //            SessionStartTime}.xlsx");
                //var totalBook = new XSSFWorkbook(totalFishingDiagram);

                string fileName = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\work\{
                    data.Name}_{data.SerialNumber}_FishingDiagram_{DateTime.Now.ToString("yy-MM-dd-HH-mm-ss")}.xlsx";
                //Сохранение изменённого файла
                using (
                    var file =
                        new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
                {
                    Book.Write(file);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"I have a bad feeling about this: {e.Message}", "Viva La Resistance!!!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }
    }
}