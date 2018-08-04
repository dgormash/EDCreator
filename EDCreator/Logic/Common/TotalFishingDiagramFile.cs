using System.IO;

namespace FDCreator.Logic.Common
{
    public  static class TotalFishingDiagramFile
    {
        public static FileStream GetTotalFishingDiagramFileStream(string filePath)
        {
            return new FileStream(filePath,
                FileMode.OpenOrCreate, FileAccess.Write);
        }
    }
}