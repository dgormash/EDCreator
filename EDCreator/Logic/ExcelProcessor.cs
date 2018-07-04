using System;
using System.IO;
using System.Reflection;
using System.Windows;
using FDCreator.Misc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace FDCreator.Logic
{
    //В классах ...ExcelProcessor всё по аналогии с PdfProcessor - есть базовый класс, в котором прописаны методы, и есть потомки,
    //в которых эти методы при необходимости переопределяются.
    public class ExcelProcessor
    {
        protected string TemplateFileName;
        protected XSSFWorkbook Book;
        protected ISheet Sheet;
        protected IRow Row;
        protected ICell Cell;

        protected Excel.Application ExcelApp;
        protected Excel.Window ExcelWindow;


        public virtual void PassDataToExcel(ParsedData data)
        {
            if (string.IsNullOrEmpty(TemplateFileName)) return;

            var filePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\misc\{TemplateFileName}";
            string fileName;
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

                //Заполнение заголовка (сам метод внизу)
                FillHeader(data);

                //Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные (нумерация ячеек в коде начинается с 0)
                //Поэтому от номера строки и столбца нужно отнимать 1
                var cellNum = 2;
            

                //SerialNumber
                SetCellValue(21, cellNum, data.SerialNumber); //соответствует 22 строке в шаблоне и т.д.
                //L
                var inches = InchesValueRetriever.GetInchesValue(data.Length);

                SetCellValue(22, cellNum, LengthConverter.InchesToMeters(inches).ToString("0.000"));
                //OD
                SetCellValue(23, cellNum, data.ConnectionOne.Od);
                //ID
                SetCellValue(24, cellNum, data.ConnectionTwo.Id);
                //BOX
                SetCellValue(25, cellNum, data.ConnectionOne.TreadSize);
                //PIN
                SetCellValue(26, cellNum, data.ConnectionTwo.TreadSize); //соответствует 27 строке в шаблоне

                fileName = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\out\{
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
                MessageBox.Show($"I have a bad feeling about this: {e.Message}", "Viva La Resistance!!!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            OpenExcelApp(fileName);
        }

        protected void SetCellValue(int rowNum, int cellNum, string value)
        {
            Row = Sheet.GetRow(rowNum);
            Cell = Row.GetCell(cellNum);
            Cell.SetCellValue(value);
        }

        protected void FillHeader(ParsedData data)
        {
            var cellNum = 2;

            //Запись заголовка
            SetCellValue(4, cellNum, data.Header.ClientField);
            SetCellValue(5, cellNum, data.Header.FieldPadWellField);
            SetCellValue(6, cellNum, data.Header.LocationField);

            cellNum = 8;
            SetCellValue(4, cellNum, data.Header.DdEngineerField);
            SetCellValue(5, cellNum, data.Header.DateField);
        }

        protected void OpenExcelApp(string file)
        {
            try
            {
                ExcelApp = new Excel.Application {Visible = true};
                ExcelApp.Workbooks.Open(file);
            }
            catch (Exception e)
            {
                MessageBox.Show($"I have a bad feeling about this: {e.Message}; opening file: {TemplateFileName}", "Error opening excel application",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ExcelApp.Quit();
            }
            finally
            {
                ExcelApp = null;
            }
            //ExcelWindow.Visible = true;
        }
    }
}