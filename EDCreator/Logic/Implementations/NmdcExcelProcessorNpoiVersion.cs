using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using FDCreator.Logic.Interfaces;
using FDCreator.Misc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace FDCreator.Logic.Implementations
{
    public class NmdcExcelProcessorNpoiVersion:IExcelProcessor
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

                _sheet =_book.GetSheetAt(0);
                _book.SetSheetName(_book.GetSheetIndex(_sheet), $"{data.Name}_{data.SerialNumber}");

                var nmdcData = new NmdcFData();

                if (!nmdcData.Tools.ContainsKey(data.SerialNumber))
                {
                    MessageBox.Show("No such NMDC-F in library", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                    return;
                }

                var nmdcTool = nmdcData.Tools[data.SerialNumber];
                var inspectionLength = LengthConverter.InchesToMeters(InchesValueRetriever.GetInchesValue(data.Length));
                var arrayLength = Convert.ToSingle(nmdcTool.L, CultureInfo.InvariantCulture);
                var difference = Math.Abs(inspectionLength - arrayLength);
                if (difference > 0.025f)
                {
                    MessageBox.Show($"Collar length {inspectionLength} doesn't match. Should be about {arrayLength}. Difference is {difference}. Prepare fishing diagram manually.", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                    return;
                }

                _cellWriter.Sheet = _sheet;
                //Заполнение заголовка (сам метод внизу)
                _headerFiller.FillHeader(data, _cellWriter);

                //Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные (нумерация ячеек в коде начинается с 0)
                //Поэтому от номера строки и столбца нужно отнимать 1
                var cellNum = 4;

                //L
                _cellWriter.SetCellValue(12, cellNum, inspectionLength.ToString("0.000"));
                //L12
                _cellWriter.SetCellValue(15, cellNum, nmdcTool.L12);
                //L11
                _cellWriter.SetCellValue(16, cellNum, nmdcTool.L11);
                //L10
                _cellWriter.SetCellValue(20, cellNum, nmdcTool.L10);
                //L9
                _cellWriter.SetCellValue(21, cellNum, nmdcTool.L9);
                //L8
                _cellWriter.SetCellValue(24, cellNum, nmdcTool.L8);
                //L7
                _cellWriter.SetCellValue(25, cellNum, nmdcTool.L7);
                //L6
                _cellWriter.SetCellValue(29, cellNum, nmdcTool.L6);
                //L5
                _cellWriter.SetCellValue(30, cellNum, nmdcTool.L5);
                //L4
                _cellWriter.SetCellValue(33, cellNum, nmdcTool.L4);
                //L3
                _cellWriter.SetCellValue(34, cellNum, nmdcTool.L3);
                //L2
                _cellWriter.SetCellValue(38, cellNum, nmdcTool.L2);
                //L1
                _cellWriter.SetCellValue(39, cellNum, nmdcTool.L1);

                cellNum = 8;
                //SerialNumber
                _cellWriter.SetCellValue(12, cellNum, data.SerialNumber); //соответствует 22 строке в шаблоне и т.д.
                //OD
                _cellWriter.SetCellValue(13, cellNum, data.ConnectionOne.Od);
                //ID
                _cellWriter.SetCellValue(14, cellNum, data.ConnectionTwo.Id);

                //Od6
                _cellWriter.SetCellValue(19, cellNum, nmdcTool.Od6);
                //Od5
                _cellWriter.SetCellValue(20, cellNum, nmdcTool.Od5);
                //Od4
                _cellWriter.SetCellValue(21, cellNum, nmdcTool.Od4);
                //Od3
                _cellWriter.SetCellValue(22, cellNum, nmdcTool.Od3);
                //Od2
                _cellWriter.SetCellValue(23, cellNum, nmdcTool.Od2);
                //Od1
                _cellWriter.SetCellValue(24, cellNum, nmdcTool.Od1);

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
                MessageBox.Show(e.Message, "I have a bad feeling about this", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}