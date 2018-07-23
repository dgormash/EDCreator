using FDCreator.Misc;

namespace FDCreator.Logic
{
    internal class CrossoverSubPdfProcessor : PdfProcessor
    {
        protected CrossoverSubParsedData TransferedData = new CrossoverSubParsedData();

        public override ParsedData GetPdfData()
        {
            //SerialNumber
            var rect = new iTextSharp.text.Rectangle(437, 722, 460, 728);
            TransferingData.SerialNumber = Parser.GetStringValueFromRegion(File, rect);

            //Length shoulder to shoulder
            rect = new iTextSharp.text.Rectangle(148, 640, 165, 647);
            TransferingData.Length = Parser.GetStringValueFromRegion(File, rect);

            //Connection Type, 1 TreadSize, Outer diameter

            rect = new iTextSharp.text.Rectangle(103, 555, 130, 614);
            var connectionColumn = Parser.GetStringValueFromRegion(File, rect).Split('\n');
            TransferingData.ConnectionOne = FillConnectionInfo(connectionColumn);

            //Connection 2 Type, TreadSize, Outer diameter
            rect = new iTextSharp.text.Rectangle(339, 555, 368, 614);
            connectionColumn = Parser.GetStringValueFromRegion(File, rect).Split('\n');
            TransferingData.ConnectionTwo = FillConnectionInfo(connectionColumn);

            //Version 
            rect = new iTextSharp.text.Rectangle(174, 34, 284, 40);
            TransferingData.Version = VersionExtractor.GetVersion(Parser.GetStringValueFromRegion(File, rect));

            return TransferingData;
        }

        protected override Connection FillConnectionInfo(string[] stringArray)
        {
            var connectionInfo = new Connection();
            if (stringArray.Length == 4)
            {
                connectionInfo.ConnectionType = stringArray[0];
                connectionInfo.TreadSize = stringArray[1];
                connectionInfo.Od = stringArray[3];
            }
            else
            {
                connectionInfo.ConnectionType = stringArray[0];
                connectionInfo.TreadSize = $"{stringArray[1]} {stringArray[2]}";
                connectionInfo.Od = stringArray[4];
                connectionInfo.Id = stringArray[5];
            }
            return connectionInfo;
        }
    }
}