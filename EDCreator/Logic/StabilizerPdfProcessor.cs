using FDCreator.Misc;

namespace FDCreator.Logic
{
    public class StabilizerPdfProcessor:PdfProcessor
    {
        protected new StabilizerParsedData TransferingData = new StabilizerParsedData();
        //Здесь переопределяются оба метода базового класса PdfProcessor, так как нужно больше данных, чем в других инспекциях
        public override ParsedData GetPdfData()
        {
            //SerialNumber
            var rect = new iTextSharp.text.Rectangle(437, 722, 476, 728);
            TransferingData.SerialNumber = Parser.GetStringValueFromRegion(File, rect);

            //Length shoulder to shoulder
            rect = new iTextSharp.text.Rectangle(148, 640, 167, 647);
            TransferingData.Length = Parser.GetStringValueFromRegion(File, rect);

            //Stabilizer OD
            rect = new iTextSharp.text.Rectangle(308, 640, 325, 647);
            TransferingData.StabilizerOd = Parser.GetStringValueFromRegion(File, rect);

            //Lobe Length
            rect = new iTextSharp.text.Rectangle(313, 632, 320, 638);
            TransferingData.LobeLength = Parser.GetStringValueFromRegion(File, rect);

            //Lobe Width
            rect = new iTextSharp.text.Rectangle(315, 624, 318, 630);
            TransferingData.LobeWidth = Parser.GetStringValueFromRegion(File, rect);

            //Fishing neck/Tong space
            rect = new iTextSharp.text.Rectangle(467, 523, 484, 530);
            TransferingData.FishingNeckTongSpace = Parser.GetStringValueFromRegion(File, rect);

            //Connection 1 Type, TreadSize
            rect = new iTextSharp.text.Rectangle(29, 584, 126, 614);
            var connectionColumn = Parser.GetStringValueFromRegion(File, rect).Split('\n');
            TransferingData.ConnectionOne = FillConnectionInfo(connectionColumn);

            //Connection 1 Outer diameter
            rect = new iTextSharp.text.Rectangle(109, 564, 130, 578);
            TransferingData.ConnectionOne.Od = Parser.GetStringValueFromRegion(File, rect);
            //Connection 1 Internal diameter
            rect = new iTextSharp.text.Rectangle(111, 548, 129, 554);
            TransferingData.ConnectionOne.Id = Parser.GetStringValueFromRegion(File, rect);

            //Connection 2 Type, TreadSize
            rect = new iTextSharp.text.Rectangle(259, 584, 362, 614);
            connectionColumn = Parser.GetStringValueFromRegion(File, rect).Split('\n');
            TransferingData.ConnectionTwo = FillConnectionInfo(connectionColumn);

            //Connection 2 Outer diameter
            rect = new iTextSharp.text.Rectangle(347, 564, 368, 578);
            TransferingData.ConnectionTwo.Od = Parser.GetStringValueFromRegion(File, rect);
            //Connection 2 Internal diameter
            rect = new iTextSharp.text.Rectangle(356, 555, 359, 562);
            TransferingData.ConnectionTwo.Id = Parser.GetStringValueFromRegion(File, rect);

            //Version 
            rect = new iTextSharp.text.Rectangle(174, 34, 284, 40);
            TransferingData.Version = VersionExtractor.GetVersion(Parser.GetStringValueFromRegion(File, rect));

            return TransferingData;
        }


        //protected override Connection FillConnectionInfo(string[] stringArray)
        //{
        //    var connectionInfo = new Connection();
        //    if (stringArray.Length == 3)
        //    {
        //        connectionInfo.TreadSize =stringArray[0];
        //        connectionInfo.Od = stringArray[2];
        //    }
        //    else
        //    {
        //        connectionInfo.TreadSize =stringArray[0];
        //        connectionInfo.Id = stringArray[3];
        //    }
        //    return connectionInfo;
        //}
    }
}