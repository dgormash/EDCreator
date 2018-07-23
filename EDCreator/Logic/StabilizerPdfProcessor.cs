using FDCreator.Misc;

namespace FDCreator.Logic
{
    public class StabilizerPdfProcessor:PdfProcessor
    {
        //Здесь переопределяются оба метода базового класса PdfProcessor, так как нужно больше данных, чем в других инспекциях
        public override ParsedData GetPdfData()
        {
            var transferingData = new StabilizerParsedData();

            //SerialNumber
            var rect = new iTextSharp.text.Rectangle(437, 722, 476, 728);
            transferingData.SerialNumber = Parser.GetStringValueFromRegion(File, rect);

            //Length shoulder to shoulder
            rect = new iTextSharp.text.Rectangle(148, 640, 167, 647);
            transferingData.Length = Parser.GetStringValueFromRegion(File, rect);

            //Stabilizer OD
            rect = new iTextSharp.text.Rectangle(308, 640, 325, 647);
            transferingData.StabilizerOd = Parser.GetStringValueFromRegion(File, rect);

            //Lobe Length
            rect = new iTextSharp.text.Rectangle(313, 632, 320, 638);
            transferingData.LobeLength = Parser.GetStringValueFromRegion(File, rect);

            //Lobe Width
            rect = new iTextSharp.text.Rectangle(315, 624, 318, 630);
            transferingData.LobeWidth = Parser.GetStringValueFromRegion(File, rect);

            //Fishing neck/Tong space
            rect = new iTextSharp.text.Rectangle(467, 523, 484, 530);
            transferingData.FishingNeckTongSpace = Parser.GetStringValueFromRegion(File, rect);

            //Connection 1 TreadSize, Outer diameter
            rect = new iTextSharp.text.Rectangle(103, 564, 130, 606);
            var connectionColumn = Parser.GetStringValueFromRegion(File, rect).Split('\n');
            transferingData.ConnectionOne = FillConnectionInfo(connectionColumn);

            //Connection 2 TreadSize, Outer diameter
            rect = new iTextSharp.text.Rectangle(339, 548, 368, 606);
            connectionColumn = Parser.GetStringValueFromRegion(File, rect).Split('\n');
            transferingData.ConnectionTwo = FillConnectionInfo(connectionColumn);

            //Version 
            rect = new iTextSharp.text.Rectangle(174, 34, 284, 40);
            transferingData.Version = VersionExtractor.GetVersion(Parser.GetStringValueFromRegion(File, rect));

            return transferingData;
        }


        protected override Connection FillConnectionInfo(string[] stringArray)
        {
            var connectionInfo = new Connection();
            if (stringArray.Length == 3)
            {
                connectionInfo.TreadSize =stringArray[0];
                connectionInfo.Od = stringArray[2];
            }
            else
            {
                connectionInfo.TreadSize =stringArray[0];
                connectionInfo.Id = stringArray[3];
            }
            return connectionInfo;
        }
    }
}