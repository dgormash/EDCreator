using System;
using System.Globalization;
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
    public class ArcExcelProcessorNpoiVersion:ISmartToolExcelProcessor
    {
        private readonly ICellValueWriter _cellWriter = new CellValueWriter();
        private XSSFWorkbook _book;
        private ISheet _sheet;
        public string TemplateFileName { get; set; }
        public void CreateFishingDiagram(ISmartTool smartTool)
        {
            TemplateFileName = "ARC Diagram.xlsx";
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
                _book.SetSheetName(_book.GetSheetIndex(_sheet), $"{smartTool.Top.Name}_{smartTool.Top.SerialNumber}");
                _cellWriter.Sheet = _sheet;
                var arcData = new ArcData();
                if (!arcData.Tools.ContainsKey(smartTool.Top.SerialNumber))
                {
                    MessageBox.Show("No such ARC in library", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                    return;
                }

                var arcTool = arcData.Tools[smartTool.Top.SerialNumber];

                var inspectionLength = LengthConverter.InchesToMeters(InchesValueRetriever.GetInchesValue(smartTool.Top.Length));
                var arrayLength = Convert.ToSingle(arcTool.L, CultureInfo.InvariantCulture);
                var difference = Math.Abs(inspectionLength - arrayLength);
                if (difference > 0.025f)
                {
                    MessageBox.Show($"Collar length {inspectionLength} doesn't match. Should be about {arrayLength}. Difference is {difference}. Prepare fishing diagram manually.", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                    return;
                }
                //Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные (нумерация ячеек в коде начинается с 0)
                //Поэтому от номера строки и столбца нужно отнимать 1
                var cellNum = 2;


                //ARC SerialNumber
                _cellWriter.SetCellValue(7, cellNum, smartTool.Top.SerialNumber);

                //Bottom Sub SerialNumber
                _cellWriter.SetCellValue(11, cellNum, smartTool.Bottom.SerialNumber);

                cellNum = 5;
                //OD8 from ARC inspection
                _cellWriter.SetCellValue(13, cellNum, smartTool.Top.ConnectionOne.Od);
                //OD7
                _cellWriter.SetCellValue(19, cellNum, arcTool.Od7);
                //OD6
                _cellWriter.SetCellValue(22, cellNum, arcTool.Od6);
                //OD5
                _cellWriter.SetCellValue(25, cellNum, arcTool.Od5);
                //OD4
                _cellWriter.SetCellValue(36, cellNum, arcTool.Od4);
                //OD3
                _cellWriter.SetCellValue(48, cellNum, arcTool.Od3);
                //OD2
                _cellWriter.SetCellValue(60, cellNum, arcTool.Od2);
                //OD1
                _cellWriter.SetCellValue(64, cellNum, arcTool.Od1);
                //OD Saver Sub
                _cellWriter.SetCellValue(68, cellNum, smartTool.Bottom.ConnectionOne.Od);
                //ID Saver Sub
                _cellWriter.SetCellValue(75, 7, smartTool.Bottom.ConnectionTwo.Id);

                cellNum = 9;
                //Top connection type
                _cellWriter.SetCellValue(7, cellNum, smartTool.Top.ConnectionOne.ConnectionType);
                //Saver sub connection type
                _cellWriter.SetCellValue(75, cellNum, smartTool.Bottom.ConnectionTwo.ConnectionType);

                cellNum = 10;
                var saverSubLength = LengthConverter.InchesToMeters(InchesValueRetriever.GetInchesValue(smartTool.Bottom.Length));

                //L10
                _cellWriter.SetCellValue(18, cellNum, (Convert.ToSingle(arcTool.L10, CultureInfo.InvariantCulture) + saverSubLength).ToString("0.000"));
                //L9
                _cellWriter.SetCellValue(21, cellNum, (Convert.ToSingle(arcTool.L9, CultureInfo.InvariantCulture) + saverSubLength).ToString("0.000"));
                //L8
                _cellWriter.SetCellValue(23, cellNum, (Convert.ToSingle(arcTool.L8, CultureInfo.InvariantCulture) + saverSubLength).ToString("0.000"));
                //L7
                _cellWriter.SetCellValue(25, cellNum, (Convert.ToSingle(arcTool.L7, CultureInfo.InvariantCulture) + saverSubLength).ToString("0.000"));
                //L6
                _cellWriter.SetCellValue(35, cellNum, (Convert.ToSingle(arcTool.L6, CultureInfo.InvariantCulture) + saverSubLength).ToString("0.000"));
                //L5
                _cellWriter.SetCellValue(38, cellNum, (Convert.ToSingle(arcTool.L5, CultureInfo.InvariantCulture) + saverSubLength).ToString("0.000"));
                //L4
                _cellWriter.SetCellValue(47, cellNum, (Convert.ToSingle(arcTool.L4, CultureInfo.InvariantCulture) + saverSubLength).ToString("0.000"));
                //L3
                _cellWriter.SetCellValue(50, cellNum, (Convert.ToSingle(arcTool.L3, CultureInfo.InvariantCulture) + saverSubLength).ToString("0.000"));
                //L2
                _cellWriter.SetCellValue(59, cellNum, (Convert.ToSingle(arcTool.L2, CultureInfo.InvariantCulture) + saverSubLength).ToString("0.000"));
                //L1
                _cellWriter.SetCellValue(62, cellNum, (Convert.ToSingle(arcTool.L1, CultureInfo.InvariantCulture) + saverSubLength).ToString("0.000"));

                //L total
                _cellWriter.SetCellValue(40, 13, (inspectionLength + saverSubLength).ToString("0.000"));


                string fileName = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\work\{
                    smartTool.Top.Name}_{smartTool.Top.SerialNumber}_FishingDiagram_{DateTime.Now.ToString("yy-MM-dd-HH-mm-ss")}.xlsx";
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