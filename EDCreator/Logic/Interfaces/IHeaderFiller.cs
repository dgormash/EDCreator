using FDCreator.Misc;

namespace FDCreator.Logic.Interfaces
{
    public interface IHeaderFiller
    {
        void FillHeader(IParsedData data, ICellValueWriter cellWriter);
    }
}