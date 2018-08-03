using NPOI.SS.UserModel;

namespace FDCreator.Logic.Interfaces
{
    public interface ICellValueWriter
    {
        ISheet Sheet { get; set; }
        void SetCellValue(int rowNum, int cellNum, string value);
    }
}