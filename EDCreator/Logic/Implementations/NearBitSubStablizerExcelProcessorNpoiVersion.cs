using System;
using System.IO;
using System.Reflection;
using System.Windows;
using FDCreator.Logic.Common;
using FDCreator.Logic.Interfaces;
using FDCreator.Misc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using LengthConverter = FDCreator.Logic.Common.LengthConverter;

namespace FDCreator.Logic.Implementations
{
    public class NearBitSubStablizerExcelProcessorNpoiVersion:IExcelProcessor
    {
        private readonly ICellValueWriter _cellWriter = new CellValueWriter();
        private readonly IHeaderFiller _headerFiller = new DumbIronHeaderFiller();
        private XSSFWorkbook _book;
        private ISheet _sheet;
        public string TemplateFileName { get; set; }
        public void CreateFishingDiagram(IParsedData data)
        {
            var stabilizerData = (IStabilizerParsedData)data;
            //... (прдолжение) чтобы использовать поля из StabilizerParsedData, необходимо вот так вот
            //как здесь выполнить приведение к призводному типу. Просто небольшой нюанс, если вы будете делать свои версии класса ParsedData
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
                _book.SetForceFormulaRecalculation(true);
                _sheet = _book.GetSheetAt(0);

                _sheet = _book.GetSheetAt(0);
                _book.SetSheetName(_book.GetSheetIndex(_sheet), $"{data.Name}_{data.SerialNumber}");
                _cellWriter.Sheet = _sheet;
                //Запись заголовка
                _headerFiller.FillHeader(stabilizerData, _cellWriter);

                //cellNum - Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные
                var cellNum = 6;

                //SerialNumber
                _cellWriter.SetCellValue(14, cellNum, stabilizerData.SerialNumber);
                //TOP
                _cellWriter.SetCellValue(17, cellNum, stabilizerData.ConnectionOne.TreadSize);
                //BOT
                _cellWriter.SetCellValue(18, cellNum, stabilizerData.ConnectionTwo.TreadSize);
                //L
                var inches = InchesValueRetriever.GetInchesValue(stabilizerData.Length);
                _cellWriter.SetCellValue(22, cellNum, LengthConverter.InchesToMeters(inches).ToString("0.000"));
                //L1
                inches = InchesValueRetriever.GetInchesValue(stabilizerData.FishingNeckTongSpace);
                _cellWriter.SetCellValue(23, cellNum, LengthConverter.InchesToMeters(inches).ToString("0.000"));
                //OD
                _cellWriter.SetCellValue(29, cellNum, stabilizerData.ConnectionOne.Od);
                //ID
                _cellWriter.SetCellValue(30, cellNum, stabilizerData.ConnectionTwo.Od);
                //MaxOD
                _cellWriter.SetCellValue(31, cellNum, stabilizerData.StabilizerOd);
                //BladeLength
                inches = InchesValueRetriever.GetInchesValue(stabilizerData.LobeLength);
                _cellWriter.SetCellValue(32, cellNum, LengthConverter.InchesToMeters(inches).ToString("0.000"));
                //BladeWidth
                _cellWriter.SetCellValue(34, cellNum, stabilizerData.LobeWidth);

                string fileName = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\work\{
                    stabilizerData.Name}_{stabilizerData.SerialNumber}_FishingDiagram_{
                    DateTime.Now.ToString("yy-MM-dd-HH-mm-s")}.xlsx";
                //Сохранение изменённого файла
                using (
                    var file =
                        new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    _book.Write(file);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Something is wrong: {e.Message}", "Viva La Resistance!!!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}