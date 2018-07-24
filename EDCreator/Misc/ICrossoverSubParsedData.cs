namespace FDCreator.Misc
{
    public interface ICrossoverSubParsedData : IParsedData
    {
        CrossoverType Type { get; }
        string FishingNeck { get; set; }
    }
}