using System;
using System.IO;
using System.Reflection;
using System.Windows;
using FDCreator.Misc;
using NPOI.XSSF.UserModel;

namespace FDCreator.Logic
{
    internal class NmdcExcelProcessor : ExcelProcessor
    {
        public NmdcExcelProcessor(string sessionStartTime) : base(sessionStartTime)
        {
            TemplateFileName = "NMDC-F Diagram.xlsx";
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

                var nmdcData = new NmdcFData();

                if (!nmdcData.Tools.ContainsKey(data.SerialNumber))
                {
                    MessageBox.Show("No such NMDC-F in library", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                    return;
                }

                var nmdcTool = nmdcData.Tools[data.SerialNumber];
                var comparableLength = InchesValueRetriever.GetInchesValue(data.Length);
                if (Math.Abs(LengthConverter.InchesToMeters(comparableLength) - Convert.ToSingle(nmdcTool.L)) > 0.025f)
                {
                    MessageBox.Show($"Collar length {LengthConverter.InchesToMeters(comparableLength)} doesn't match. Should be {nmdcTool.L}. Difference is {Math.Abs(LengthConverter.InchesToMeters(comparableLength) - Convert.ToSingle(nmdcTool.L))}. Prepare fishing diagram manually.", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                    return;
                }

                //Заполнение заголовка (сам метод внизу)
                FillHeader(data);

                //Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные (нумерация ячеек в коде начинается с 0)
                //Поэтому от номера строки и столбца нужно отнимать 1
                var cellNum = 4;
               
                //L
                SetCellValue(12, cellNum, LengthConverter.InchesToMeters(comparableLength).ToString("0.000"));
                //L12
                SetCellValue(15, cellNum, nmdcTool.L12);
                //L11
                SetCellValue(16, cellNum, nmdcTool.L11);
                //L10
                SetCellValue(20, cellNum, nmdcTool.L10);
                //L9
                SetCellValue(21, cellNum, nmdcTool.L9);
                //L8
                SetCellValue(24, cellNum, nmdcTool.L8);
                //L7
                SetCellValue(25, cellNum, nmdcTool.L7);
                //L6
                SetCellValue(29, cellNum, nmdcTool.L6);
                //L5
                SetCellValue(30, cellNum, nmdcTool.L5);
                //L4
                SetCellValue(33, cellNum, nmdcTool.L4);
                //L3
                SetCellValue(34, cellNum, nmdcTool.L3);
                //L2
                SetCellValue(38, cellNum, nmdcTool.L2);
                //L1
                SetCellValue(39, cellNum, nmdcTool.L1);

                cellNum = 8;
                //SerialNumber
                SetCellValue(12, cellNum, data.SerialNumber); //соответствует 22 строке в шаблоне и т.д.
                //OD
                SetCellValue(13, cellNum, data.ConnectionOne.Od);
                //ID
                SetCellValue(14, cellNum, data.ConnectionTwo.Id);

                //Od6
                SetCellValue(19, cellNum, nmdcTool.Od6);
                //Od5
                SetCellValue(20, cellNum, nmdcTool.Od5);
                //Od4
                SetCellValue(21, cellNum, nmdcTool.Od4);
                //Od3
                SetCellValue(22, cellNum, nmdcTool.Od3);
                //Od2
                SetCellValue(23, cellNum, nmdcTool.Od2);
                //Od1
                SetCellValue(24, cellNum, nmdcTool.Od1);

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
                MessageBox.Show(e.Message, "I have a bad feeling about this", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }
    }
}