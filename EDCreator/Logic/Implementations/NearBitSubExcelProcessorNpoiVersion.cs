using System;
using System.IO;
using System.Reflection;
using System.Windows;
using FDCreator.Logic.Interfaces;
using FDCreator.Misc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace FDCreator.Logic.Implementations
{
    public class NearBitSubExcelProcessorNpoiVersion:IExcelProcessor
    {
        private readonly ICellValueWriter _cellWriter = new CellValueWriter();
        private readonly IHeaderFiller _headerFiller = new DumbIronHeaderFiller();
        private XSSFWorkbook _book;
        private ISheet _sheet;
        public string TemplateFileName { get; set; }
        public void CreateFishingDiagram(IParsedData data)
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
                    _book = new XSSFWorkbook(file);
                }

                _sheet = _book.GetSheetAt(0);
                _book.SetSheetName(_book.GetSheetIndex(_sheet), $"{data.Name}_{data.SerialNumber}");
                _cellWriter.Sheet = _sheet;
                //Заполнение заголовка (метод прописан в ExcelProcessor.cs)
                _headerFiller.FillHeader(data,_cellWriter);
               

                //Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные (нумерация ячеек в коде начинается с 0)
                //Поэтому от номера строки и столбца нужно отнимать 1
                var cellNum = 5;


                //SerialNumber
                _cellWriter.SetCellValue(14, cellNum, data.SerialNumber); //соответствует 22 строке в шаблоне и т.д.
                //L
                var inches = InchesValueRetriever.GetInchesValue(data.Length);

                _cellWriter.SetCellValue(19, cellNum, LengthConverter.InchesToMeters(inches).ToString("0.000"));
                //Od1
                _cellWriter.SetCellValue(20, cellNum, data.ConnectionOne.Od);
                //ID
                _cellWriter.SetCellValue(21, cellNum, data.ConnectionTwo.Od);
                //Top
                _cellWriter.SetCellValue(16, cellNum, data.ConnectionOne.TreadSize);
                //Bottom
                _cellWriter.SetCellValue(17, cellNum, data.ConnectionTwo.TreadSize); //соответствует 27 строке в шаблоне

                //var totalFishingDiagram =
                //    TotalFishingDiagramFile.GetTotalFishingDiagramFileStream(
                //        $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\out\Field_Pad_Well_FishingDiagram_{
                //            SessionStartTime}.xlsx");
                //var total_book = new XSSFWorkbook(totalFishingDiagram);

                string fileName = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\work\{
                    data.Name}_{data.SerialNumber}_FishingDiagram_{DateTime.Now.ToString("yy-MM-dd-HH-mm-ss")}.xlsx";
                //Сохранение изменённого файла
                using (
                    var file =
                        new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
                {
                    _book.Write(file);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"I have a bad feeling about this: {e.Message}", "Viva La Resistance!!!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}