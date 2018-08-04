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

namespace FDCreator.Logic.SmartTools
{
    public class SmartToolExcelProcessor
    {
        protected string TemplateFileName;
        protected XSSFWorkbook Book;
        protected ISheet Sheet;
        protected IRow Row;
        protected ICell Cell;

        protected Microsoft.Office.Interop.Excel.Application ExcelApp;
        protected Microsoft.Office.Interop.Excel.Window ExcelWindow;
        protected string SessionStartTime;

        public virtual void PassDataToExcel(ISmartTool tool)
        {
            
            switch (tool.Type)
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
                    Book = new XSSFWorkbook(file);
                }

                Sheet = Book.GetSheetAt(0);
                Book.SetSheetName(Book.GetSheetIndex(Sheet), $"{tool.Top.Name}_{tool.Top.SerialNumber}");

                
                //Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные (нумерация ячеек в коде начинается с 0)
                //Поэтому от номера строки и столбца нужно отнимать 1
                var cellNum = 2;


                //MDC SerialNumber
                SetCellValue(7, cellNum, tool.Middle.SerialNumber);
                //Top Sub SerialNumber
                SetCellValue(9, cellNum, tool.Top.SerialNumber);
                //Bottom Sub SerialNumber
                SetCellValue(11, cellNum, tool.Bottom.SerialNumber);

                cellNum = 4;
                //Top Sub OD
                SetCellValue(14, cellNum, tool.Top.ConnectionOne.Od);
                //MDC OD
                SetCellValue(20, cellNum, tool.Middle.ConnectionOne.Od);
                //Bottom Sub OD
                SetCellValue(64, cellNum, tool.Bottom.ConnectionOne.Od);

                //Bottom Sub ID
                SetCellValue(75, 7, tool.Bottom.ConnectionTwo.Id);

                cellNum = 9;
                //Top Sub Treadsize
                SetCellValue(7, cellNum, tool.Top.ConnectionOne.TreadSize);
                //Bottom Sub Treadsize
                SetCellValue(75, cellNum, tool.Bottom.ConnectionTwo.TreadSize);

                cellNum = 10;

                //L
                var lMdc = LengthConverter.InchesToMeters(InchesValueRetriever.GetInchesValue(tool.Middle.Length));
                var lBotSub = LengthConverter.InchesToMeters(InchesValueRetriever.GetInchesValue(tool.Bottom.Length));
                var lTopSub = LengthConverter.InchesToMeters(InchesValueRetriever.GetInchesValue(tool.Top.Length));
                //L MDC + Bottom Sub
                SetCellValue(17, cellNum, (lMdc+ lBotSub).ToString("0.000"));
                //L MDC + Bottom Sub
                SetCellValue(61, cellNum, lBotSub.ToString("0.000"));
                //L MDC + Bottom Sub + Top Sub
                SetCellValue(40, 13, (lMdc + lBotSub + lTopSub).ToString("0.000"));

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

        protected void SetCellValue(int rowNum, int cellNum, string value)
        {
            Row = Sheet.GetRow(rowNum);
            Cell = Row.GetCell(cellNum);
            Cell.SetCellValue(value);
        }
    }
}