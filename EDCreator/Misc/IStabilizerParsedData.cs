namespace FDCreator.Misc
{
    public interface IStabilizerParsedData:IParsedData
    {
         string StabilizerOd { get; set; }
         string LobeLength { get; set; }
         string LobeWidth { get; set; }
         string FishingNeckTongSpace { get; set; }
    }
}