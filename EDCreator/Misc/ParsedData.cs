namespace FDCreator.Misc
{
    public class ParsedData : IParsedData
    {
        //Прочие данные, которые берутся из pdf.
        public HeaderData Header { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Length { get; set; }
        public Connection ConnectionOne { get; set; }
        public Connection ConnectionTwo { get; set; }
        public string Version { get; set; }
    }
}
