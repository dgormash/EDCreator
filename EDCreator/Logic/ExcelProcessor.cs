using System;
using System.IO;
using System.Reflection;
using System.Windows;
using EDCreator.Misc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EDCreator.Logic
{
    public class ExcelProcessor
    {
        protected string TemplateFileName;
        protected XSSFWorkbook Book;
        protected ISheet Sheet;
        protected IRow Row;
        protected ICell Cell;


        public virtual void PassDataToExcel(ParsedData data)
        {
            if (string.IsNullOrEmpty(TemplateFileName)) return;

            var filePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\misc\{TemplateFileName}";

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

            //Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные
            var cellNum = 3;
            

            //SerialNumber
            SetCellValue(22, cellNum, data.SerialNumber);
            //L
            SetCellValue(23, cellNum, data.Length);
            //OD
            SetCellValue(24, cellNum, data.ConnectionOne.Od);
            //ID
            SetCellValue(25, cellNum, data.ConnectionTwo.Id);
            //BOX
            SetCellValue(26, cellNum, data.ConnectionOne.TreadSize);
            //PIN
            SetCellValue(27, cellNum, data.ConnectionTwo.TreadSize);

            //Сохранение изменённого файла
            using (
                var file =
                    new FileStream(
                        $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\out\{
                            data.Name}_{data.SerialNumber}_FinishedDiagram.xlsx",
                        FileMode.Create, FileAccess.Write))
            {
                Book.Write(file);
            }
        }

        protected void SetCellValue(int rowNum, int cellNum, string value)
        {
            Row = Sheet.GetRow(rowNum);
            Cell = Row.GetCell(cellNum);
            Cell.SetCellValue(value);
        }

        protected void FillHeader(ParsedData data)
        {
            var cellNum = 3;

            //Запись заголовка
            SetCellValue(5, cellNum, data.Header.ClientField);
            SetCellValue(6, cellNum, data.Header.FieldPadWellField);
            SetCellValue(7, cellNum, data.Header.LocationField);

            cellNum = 9;
            SetCellValue(5, cellNum, data.Header.DdEngineerField);
            SetCellValue(6, cellNum, data.Header.DateField);
        }
    }
}