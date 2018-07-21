using System.IO;

namespace FDCreator.Logic
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