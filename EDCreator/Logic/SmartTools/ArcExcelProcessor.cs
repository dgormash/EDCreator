using System;
using System.IO;
using System.Reflection;
using System.Windows;
using NPOI.XSSF.UserModel;

namespace FDCreator.Logic.SmartTools
{
    internal class ArcExcelProcessor : SmartToolExcelProcessor
    {
        public override void PassDataToExcel(ISmartTool tool)
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
                    Book = new XSSFWorkbook(file);
                }

                Sheet = Book.GetSheetAt(0);
                Book.SetSheetName(Book.GetSheetIndex(Sheet), $"{tool.Top.Name}_{tool.Top.SerialNumber}");

                var arcData = new ArcData();
                if (!arcData.Tools.ContainsKey(tool.Top.SerialNumber))
                {
                    MessageBox.Show("No such ARC in library", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                    return;
                }

                var arcTool = arcData.Tools[tool.Top.SerialNumber];

                var comparableLength = InchesValueRetriever.GetInchesValue(tool.Top.Length);
                if (Math.Abs(LengthConverter.InchesToMeters(comparableLength) - Convert.ToSingle(arcTool.L)) > 0.025f)
                {
                    MessageBox.Show("Collar length doesn't match. Prepare fishing diagram manually.", "Information", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                    return;
                }
                //Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные (нумерация ячеек в коде начинается с 0)
                //Поэтому от номера строки и столбца нужно отнимать 1
                var cellNum = 2;


                //ARC SerialNumber
                SetCellValue(7, cellNum, tool.Top.SerialNumber);
                
                //Bottom Sub SerialNumber
                SetCellValue(11, cellNum, tool.Bottom.SerialNumber);

                cellNum = 5;
                //OD8 from ARC inspection
                SetCellValue(13, cellNum, tool.Top.ConnectionOne.Od);
                //OD7
                SetCellValue(19, cellNum, arcTool.Od7);
                //OD6
                SetCellValue(22, cellNum, arcTool.Od6);
                //OD5
                SetCellValue(25, cellNum, arcTool.Od5);
                //OD4
                SetCellValue(36, cellNum, arcTool.Od4);
                //OD3
                SetCellValue(48, cellNum, arcTool.Od3);
                //OD2
                SetCellValue(60, cellNum, arcTool.Od2);
                //OD1
                SetCellValue(64, cellNum, arcTool.Od1);
                //OD Saver Sub
                SetCellValue(68, cellNum, tool.Bottom.ConnectionOne.Od);
                //ID Saver Sub
                SetCellValue(75, 7, tool.Bottom.ConnectionTwo.Id);

                cellNum = 9;
                //Top connection type
                SetCellValue(7, cellNum, tool.Top.ConnectionOne.ConnectionType);
                //Saver sub connection type
                SetCellValue(75, cellNum, tool.Bottom.ConnectionTwo.ConnectionType);

                cellNum = 10;
                var saverSubLength = LengthConverter.InchesToMeters(InchesValueRetriever.GetInchesValue(tool.Bottom.Length));

                //L10
                SetCellValue(18, cellNum, (Convert.ToSingle(arcTool.L10) + saverSubLength).ToString("0.000"));
                //L9
                SetCellValue(21, cellNum, (Convert.ToSingle(arcTool.L9) + saverSubLength).ToString("0.000"));
                //L8
                SetCellValue(23, cellNum, (Convert.ToSingle(arcTool.L8) + saverSubLength).ToString("0.000"));
                //L7
                SetCellValue(25, cellNum, (Convert.ToSingle(arcTool.L7) + saverSubLength).ToString("0.000"));
                //L6
                SetCellValue(35, cellNum, (Convert.ToSingle(arcTool.L6) + saverSubLength).ToString("0.000"));
                //L5
                SetCellValue(38, cellNum, (Convert.ToSingle(arcTool.L5) + saverSubLength).ToString("0.000"));
                //L4
                SetCellValue(47, cellNum, (Convert.ToSingle(arcTool.L4) + saverSubLength).ToString("0.000"));
                //L3
                SetCellValue(50, cellNum, (Convert.ToSingle(arcTool.L3) + saverSubLength).ToString("0.000"));
                //L2
                SetCellValue(59, cellNum, (Convert.ToSingle(arcTool.L2) + saverSubLength).ToString("0.000"));
                //L1
                SetCellValue(62, cellNum, (Convert.ToSingle(arcTool.L1) + saverSubLength).ToString("0.000"));

                //L total
                SetCellValue(40, 13, LengthConverter.InchesToMeters(InchesValueRetriever.GetInchesValue(tool.Top.Length)).ToString("0.000"));
                

                string fileName = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\work\{
                    tool.Top.Name}_{tool.Top.SerialNumber}_FishingDiagram_{DateTime.Now.ToString("yy-MM-dd-HH-mm-ss")}.xlsx";
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