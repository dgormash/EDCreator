namespace FDCreator.Misc
{
    public interface IParsedData
    {
        HeaderData Header { get; set; }
        string Name { get; set; }
        string SerialNumber { get; set; }
        string Length { get; set; }
        Connection ConnectionOne { get; set; }
        Connection ConnectionTwo { get; set; }
        string Version { get; set; }
}
}