namespace EDCreator.Misc
{
    public class ParsedData
    {
        internal LoginData Header { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Length { get; set; }
        public Connection ConnectionOne { get; set; }
        public Connection ConnectionTwo { get; set; }
    }
}
