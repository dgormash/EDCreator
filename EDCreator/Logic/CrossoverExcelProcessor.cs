using System;
using System.IO;
using System.Reflection;
using System.Windows;
using FDCreator.Misc;
using NPOI.XSSF.UserModel;

namespace FDCreator.Logic
{
    internal class CrossoverExcelProcessor : ExcelProcessor
    {

        private XSSFWorkbook _xlsBook;
        public CrossoverExcelProcessor(string sessionStartTime) : base(sessionStartTime)
        {
            TemplateFileName = string.Empty;
        }

        public override void PassDataToExcel(IParsedData data)
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
                    _xlsBook = new XSSFWorkbook(file);
                }
                _xlsBook.SetForceFormulaRecalculation(true);
                Sheet = _xlsBook.GetSheetAt(0);

                Sheet = _xlsBook.GetSheetAt(0);
                _xlsBook.SetSheetName(_xlsBook.GetSheetIndex(Sheet), $"{data.Name}_{data.SerialNumber}");
                //Запись заголовка
                FillHeader(crossoverData);

                //cellNum - Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные
                var cellNum = 5;

                //SerialNumber
                SetCellValue(14, cellNum, crossoverData.SerialNumber);
                //TOP
                SetCellValue(16, cellNum, crossoverData.ConnectionOne.TreadSize);
                //BOT
                SetCellValue(17, cellNum, crossoverData.ConnectionTwo.TreadSize);
                //L
                var inches = InchesValueRetriever.GetInchesValue(crossoverData.Length);
                SetCellValue(19, cellNum,
                    inches.Equals(0) ? string.Empty : LengthConverter.InchesToMeters(inches).ToString("0.000"));
                switch (crossoverType)
                {
                    case CrossoverType.Type1:
                        //OD
                        SetCellValue(20, cellNum, crossoverData.ConnectionOne.Od);
                        //ID
                        SetCellValue(21, cellNum, crossoverData.ConnectionTwo.Id);
                        break;
                    case CrossoverType.Type2:
                        //ID1
                        SetCellValue(20, cellNum, crossoverData.ConnectionOne.Id);
                        //ID2
                        SetCellValue(21, cellNum, crossoverData.ConnectionTwo.Id);
                        break;
                    case CrossoverType.Type3:
                    case CrossoverType.Type4:
                        //FishingNeck
                        SetCellValue(20, cellNum, crossoverData.FishingNeck);
                        //OD1
                        SetCellValue(21, cellNum, crossoverData.ConnectionOne.Od);
                        //ID2
                        SetCellValue(23, cellNum, crossoverData.ConnectionTwo.Id);
                        //OD2
                        SetCellValue(24, cellNum, crossoverData.ConnectionTwo.Od);
                        break;
                  }

                string fileName = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\work\{
                    crossoverData.Name}_{crossoverData.SerialNumber}_FishingDiagram_{DateTime.Now.ToString("yy-MM-dd-HH-mm-s")}.xlsx";
                //Сохранение изменённого файла
                using (
                    var file =
                        new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    _xlsBook.Write(file);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Something is wrong: {e.Message}", "Viva La Resistance!!!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }
    }
}