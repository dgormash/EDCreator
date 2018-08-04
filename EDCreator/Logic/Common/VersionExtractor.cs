namespace FDCreator.Logic.Common
{
    public static class VersionExtractor
    {
        public static string GetVersion(string line)
        {
            return line.Substring(29, 8);
        }
    }
}