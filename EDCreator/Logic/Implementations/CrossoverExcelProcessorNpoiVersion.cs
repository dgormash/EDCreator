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
    public class CrossoverExcelProcessorNpoiVersion:IExcelProcessor
    {
        private readonly ICellValueWriter _cellWriter = new CellValueWriter();
        private readonly IHeaderFiller _headerFiller = new DumbIronHeaderFiller();
        private XSSFWorkbook _book;
        private ISheet _sheet;
        public string TemplateFileName { get; set; }

        public void CreateFishingDiagram(IParsedData data)
        {
            var crossoverData = (CrossoverSubParsedData)data;
            var crossoverType = crossoverData.Type;
            switch (crossoverType)
            {
                case CrossoverType.Type1:
                    TemplateFileName = "Crossover Sub Type 1 Diagram.xlsx";
                    break;
                case CrossoverType.Type2:
                    TemplateFileName = "Crossover Sub Type 2 Diagram.xlsx";
                    break;
                case CrossoverType.Type3:
                    TemplateFileName = "Crossover Sub Type 3 Diagram.xlsx";
                    break;
                case CrossoverType.Type4:
                    TemplateFileName = "Crossover Sub Type 4 Diagram.xlsx";
                    break;
                case CrossoverType.NotDefined:
                    MessageBox.Show("Crossover Sub Type Not Defined", "Information message", MessageBoxButton.OK,
                       MessageBoxImage.Asterisk);
                    return;
            }

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
                _headerFiller.FillHeader(crossoverData, _cellWriter);

                //cellNum - Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные
                var cellNum = 5;

                //SerialNumber
                _cellWriter.SetCellValue(14, cellNum, crossoverData.SerialNumber);
                //TOP
                _cellWriter.SetCellValue(16, cellNum, crossoverData.ConnectionOne.TreadSize);
                //BOT
                _cellWriter.SetCellValue(17, cellNum, crossoverData.ConnectionTwo.TreadSize);
                //L
                var inches = InchesValueRetriever.GetInchesValue(crossoverData.Length);
                _cellWriter.SetCellValue(19, cellNum,
                    inches.Equals(0) ? string.Empty : LengthConverter.InchesToMeters(inches).ToString("0.000"));
                switch (crossoverType)
                {
                    case CrossoverType.Type1:
                        //OD
                        _cellWriter.SetCellValue(20, cellNum, crossoverData.ConnectionOne.Od);
                        //ID
                        _cellWriter.SetCellValue(21, cellNum, crossoverData.ConnectionTwo.Id);
                        break;
                    case CrossoverType.Type2:
                        //ID1
                        _cellWriter.SetCellValue(20, cellNum, crossoverData.ConnectionOne.Id);
                        //ID2
                        _cellWriter.SetCellValue(21, cellNum, crossoverData.ConnectionTwo.Id);
                        break;
                    case CrossoverType.Type3:
                    case CrossoverType.Type4:
                        //FishingNeck
                        _cellWriter.SetCellValue(20, cellNum, crossoverData.FishingNeck);
                        //Od1
                        _cellWriter.SetCellValue(21, cellNum, crossoverData.ConnectionOne.Od);
                        //ID2
                        _cellWriter.SetCellValue(23, cellNum, crossoverData.ConnectionTwo.Id);
                        //Od2
                        _cellWriter.SetCellValue(24, cellNum, crossoverData.ConnectionTwo.Od);
                        break;
                }

                string fileName = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\work\{
                    crossoverData.Name}_{crossoverData.SerialNumber}_FishingDiagram_{DateTime.Now.ToString("yy-MM-dd-HH-mm-s")}.xlsx";
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