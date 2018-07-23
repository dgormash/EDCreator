using FDCreator.Misc;

namespace FDCreator.Logic
{
    internal class NearBitSubPdfProcessor : PdfProcessor
    {
        //public override ParsedData GetPdfData()
        //{
        //    var transferingData = new ParsedData();

        //    //SerialNumber
        //    var rect = new iTextSharp.text.Rectangle(437, 722, 472, 728);
        //    transferingData.SerialNumber = Parser.GetStringValueFromRegion(File, rect);

        //    //Length shoulder to shoulder
        //    rect = new iTextSharp.text.Rectangle(145, 640, 169, 647);
        //    transferingData.Length = Parser.GetStringValueFromRegion(File, rect);

        //    //Connection 1 TreadSize, Outer diameter
        //    rect = new iTextSharp.text.Rectangle(103, 571, 130, 606);
        //    var connectionColumn = Parser.GetStringValueFromRegion(File, rect).Split('\n');
        //    transferingData.ConnectionOne = FillConnectionInfo(connectionColumn);

        //    //Connection 2 TreadSize, Outer diameter
        //    rect = new iTextSharp.text.Rectangle(339, 571, 368, 606);
        //    connectionColumn = Parser.GetStringValueFromRegion(File, rect).Split('\n');
        //    transferingData.ConnectionTwo = FillConnectionInfo(connectionColumn);

        //    //Version 
        //    rect = new iTextSharp.text.Rectangle(174, 34, 284, 40);
        //    transferingData.Version = VersionExtractor.GetVersion(Parser.GetStringValueFromRegion(File, rect));

        //    return transferingData;
        //}

        protected override Connection FillConnectionInfo(string[] stringArray)
        {
            var connectionInfo = new Connection
            {
                TreadSize = $"{stringArray[0]}",
                Od = stringArray[2]
            };


            return connectionInfo;
        }
    }
}