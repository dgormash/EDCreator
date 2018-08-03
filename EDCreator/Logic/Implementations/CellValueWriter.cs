using System;
using FDCreator.Logic.Interfaces;
using NPOI.SS.UserModel;

namespace FDCreator.Logic.Implementations
{
    public class CellValueWriter:ICellValueWriter
    {
        public ISheet Sheet { get; set; }
        private IRow _row;
        private ICell _cell;
        public void SetCellValue(int rowNum, int cellNum, string value)
        {
            if (Sheet == null)
            {
                throw new ArgumentNullException();
            }
            _row = Sheet.GetRow(rowNum);
            _cell = _row.GetCell(cellNum);
            _cell.SetCellValue(value);
        }
    }
}