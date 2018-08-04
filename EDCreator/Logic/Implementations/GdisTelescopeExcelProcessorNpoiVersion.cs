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
    public class GdisTelescopeExcelProcessorNpoiVersion:ISmartToolExcelProcessor
    {
        private readonly ICellValueWriter _cellWriter = new CellValueWriter();
        private XSSFWorkbook _book;
        private ISheet _sheet;
        public string TemplateFileName { get; set; }
        public void CreateFishingDiagram(ISmartTool smartTool)
        {
            switch (smartTool.Type)
            {
                case SmartToolType.Telescope:
                    TemplateFileName = "TeleScope Diagram.xlsx";
                    break;
                case SmartToolType.Gdis:
                    TemplateFileName = "GDIS Diagram.xlsx";
                    break;
            }

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

                //Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные (нумерация ячеек в коде начинается с 0)
                //Поэтому от номера строки и столбца нужно отнимать 1
                var cellNum = 2;


                //MDC SerialNumber
                _cellWriter.SetCellValue(7, cellNum, smartTool.Middle.SerialNumber);
                //Top Sub SerialNumber
                _cellWriter.SetCellValue(9, cellNum, smartTool.Top.SerialNumber);
                //Bottom Sub SerialNumber
                _cellWriter.SetCellValue(11, cellNum, smartTool.Bottom.SerialNumber);

                cellNum = 4;
                //Top Sub OD
                _cellWriter.SetCellValue(14, cellNum, smartTool.Top.ConnectionOne.Od);
                //MDC OD
                _cellWriter.SetCellValue(20, cellNum, smartTool.Middle.ConnectionOne.Od);
                //Bottom Sub OD
                _cellWriter.SetCellValue(64, cellNum, smartTool.Bottom.ConnectionOne.Od);

                //Bottom Sub ID
                _cellWriter.SetCellValue(75, 7, smartTool.Bottom.ConnectionTwo.Id);

                cellNum = 9;
                //Top Sub Treadsize
                _cellWriter.SetCellValue(7, cellNum, smartTool.Top.ConnectionOne.TreadSize);
                //Bottom Sub Treadsize
                _cellWriter.SetCellValue(75, cellNum, smartTool.Bottom.ConnectionTwo.TreadSize);

                cellNum = 10;

                //L
                var lMdc = LengthConverter.InchesToMeters(InchesValueRetriever.GetInchesValue(smartTool.Middle.Length));
                var lBotSub = LengthConverter.InchesToMeters(InchesValueRetriever.GetInchesValue(smartTool.Bottom.Length));
                var lTopSub = LengthConverter.InchesToMeters(InchesValueRetriever.GetInchesValue(smartTool.Top.Length));
                //L MDC + Bottom Sub
                _cellWriter.SetCellValue(17, cellNum, (lMdc + lBotSub).ToString("0.000", CultureInfo.InvariantCulture));
                //L MDC + Bottom Sub
                _cellWriter.SetCellValue(61, cellNum, lBotSub.ToString("0.000", CultureInfo.InvariantCulture));
                //L MDC + Bottom Sub + Top Sub
                _cellWriter.SetCellValue(40, 13, (lMdc + lBotSub + lTopSub).ToString("0.000", CultureInfo.InvariantCulture));

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