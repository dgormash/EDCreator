using System;
using System.IO;
using System.Reflection;
using System.Windows;
using FDCreator.Misc;
using NPOI.XSSF.UserModel;

namespace FDCreator.Logic
{
    public class StabilizerExcelProcessor : ExcelProcessor
    {
        //С шаблоном Stabilizer Diagram.xlsx возникла проблема при пересохранении, решил переводом оригинала шаблона в формат
        //.xls, поэтому здесь используется тип HSSFWorkbook, тогда как в других excel-процессорах используется XSSFWorkbook
        //Если файлы после записи не будут открываться, пересохраняете их в .xls, создаёте потомка ExcelProcessor и переопределяете методы
        //Здесь видно, что я поменял
        private XSSFWorkbook _xlsBook; //Это я поменял
        public StabilizerExcelProcessor()
        {
            TemplateFileName = "Stabilizer Diagram.xlsx";
        }

        //В шаблоне Stabilizer Diagram.xls прям совсем другая структура, поэтому методе PassDataToExcel необходимо переопределить

        public override void PassDataToExcel(ParsedData data)
        {
            var stabilizerData = (StabilizerParsedData) data; //... (прдолжение) чтобы использовать поля из StabilizerParsedData, необходимо вот так вот
            //как здесь выполнить приведение к призводному типу. Просто небольшой нюанс, если вы будете делать свои версии класса ParsedData
            string fileName;
            if (string.IsNullOrEmpty(TemplateFileName)) return;

            var filePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\misc\{TemplateFileName}";

            try
            {
                using (
                    var file =
                        new FileStream(filePath,
                            FileMode.Open, FileAccess.Read))
                {
                    //Book = new XSSFWorkbook(file);
                    _xlsBook = new XSSFWorkbook(file); //И это я поменял
                }
                _xlsBook.SetForceFormulaRecalculation(true);
                Sheet = _xlsBook.GetSheetAt(0);

                //Запись заголовка
                FillHeader(stabilizerData);

                //cellNum - Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные
                var cellNum = 6;

                //SerialNumber
                SetCellValue(14, cellNum, stabilizerData.SerialNumber);
                //TOP
                SetCellValue(17, cellNum, stabilizerData.ConnectionOne.TreadSize);
                //BOT
                SetCellValue(18, cellNum, stabilizerData.ConnectionTwo.TreadSize);
                //L
                var inches = InchesValueRetriever.GetInchesValue(stabilizerData.Length);
                SetCellValue(22, cellNum, LengthConverter.InchesToMeters(inches).ToString("0.000"));
                //L1
                inches = InchesValueRetriever.GetInchesValue(stabilizerData.FishingNeckTongSpace);
                SetCellValue(23, cellNum, LengthConverter.InchesToMeters(inches).ToString("0.000"));
                //OD
                SetCellValue(29, cellNum, stabilizerData.ConnectionOne.Od);
                //ID
                SetCellValue(30, cellNum, stabilizerData.ConnectionTwo.Id);
                //MaxOD
                SetCellValue(31, cellNum, stabilizerData.StabilizerOd);
                //BladeLength
                inches = InchesValueRetriever.GetInchesValue(stabilizerData.LobeLength);
                SetCellValue(32, cellNum, LengthConverter.InchesToMeters(inches).ToString("0.000"));
                //BladeWidth
                SetCellValue(34, cellNum, stabilizerData.LobeWidth);

                //L4
                //SetCellValue(26, cellNum, "=G24+G33");
                //Row = Sheet.GetRow(26);
                //Cell = Row.GetCell(cellNum);
                //Cell.SetCellFormula("G24+G33");

                fileName = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\out\{
                    stabilizerData.Name}_{stabilizerData.SerialNumber}_FishingDiagram_{DateTime.Now.ToString("yy-MM-dd-HH-mm-s")}.xlsx";
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

            OpenExcelApp(fileName);
      }
   }
}