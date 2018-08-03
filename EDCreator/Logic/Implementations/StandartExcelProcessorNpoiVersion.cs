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
    public class StandartExcelProcessorNpoiVersion:IExcelProcessor
    {
        private readonly ICellValueWriter _cellWriter = new CellValueWriter();
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
                //Заполнение заголовка (сам метод внизу)
                //todo 
                //FillHeader(data);

                //Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные (нумерация ячеек в коде начинается с 0)
                //Поэтому от номера строки и столбца нужно отнимать 1
                var cellNum = 2;


                //SerialNumber
                _cellWriter.SetCellValue(21, cellNum, data.SerialNumber); //соответствует 22 строке в шаблоне и т.д.
                //L
                var inches = InchesValueRetriever.GetInchesValue(data.Length);

                _cellWriter.SetCellValue(22, cellNum, LengthConverter.InchesToMeters(inches).ToString("0.000"));
                //OD
                _cellWriter.SetCellValue(23, cellNum, data.ConnectionOne.Od);
                //ID
                _cellWriter.SetCellValue(24, cellNum, data.ConnectionTwo.Id);
                //BOX
                _cellWriter.SetCellValue(25, cellNum, data.ConnectionOne.TreadSize);
                //PIN
                _cellWriter.SetCellValue(26, cellNum, data.ConnectionTwo.TreadSize); //соответствует 27 строке в шаблоне

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