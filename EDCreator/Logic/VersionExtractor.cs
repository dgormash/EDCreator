namespace FDCreator.Logic
{
    public static class VersionExtractor
    {
        public static string GetVersion(string line)
        {
            return line.Substring(30, 8);
        }
    }
}