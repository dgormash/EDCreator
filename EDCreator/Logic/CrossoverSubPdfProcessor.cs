using FDCreator.Misc;

namespace FDCreator.Logic
{
    internal class CrossoverSubPdfProcessor : PdfProcessor
    {
        protected new ICrossoverSubParsedData TransferingData = new CrossoverSubParsedData();

        public override IParsedData GetPdfData()
        {
            //SerialNumber
            var rect = new iTextSharp.text.Rectangle(437, 722, 460, 728);
            TransferingData.SerialNumber = Parser.GetStringValueFromRegion(File, rect);

            //Length shoulder to shoulder
            rect = new iTextSharp.text.Rectangle(148, 640, 165, 647);
            TransferingData.Length = Parser.GetStringValueFromRegion(File, rect);

            //Connection 1 Type, TreadSize
            rect = new iTextSharp.text.Rectangle(29, 584, 126, 614);
            var connectionColumn = Parser.GetStringValueFromRegion(File, rect).Split('\n');
            TransferingData.ConnectionOne = FillConnectionInfo(connectionColumn);
            //Connection 1 Outer diameter
            rect = new iTextSharp.text.Rectangle(109, 564, 130, 578);
            TransferingData.ConnectionOne.Od = Parser.GetStringValueFromRegion(File, rect);
            //Connection 1 Internal diameter
            //2 8/32 at lx: 111,72; ly: 555,9988; rx: 129,0547; ry: 562,2388
            rect = new iTextSharp.text.Rectangle(111, 548, 129, 562);
            TransferingData.ConnectionOne.Id = Parser.GetStringValueFromRegion(File, rect);

            //Connection 2 Type, TreadSize
            rect = new iTextSharp.text.Rectangle(259, 584, 362, 614);
            connectionColumn = Parser.GetStringValueFromRegion(File, rect).Split('\n');
            TransferingData.ConnectionTwo = FillConnectionInfo(connectionColumn);
            //Connection 2 Outer diameter
            rect = new iTextSharp.text.Rectangle(347, 564, 368, 578);
            TransferingData.ConnectionTwo.Od = Parser.GetStringValueFromRegion(File, rect);
            //Connection 2 Internal diameter
            rect = new iTextSharp.text.Rectangle(347, 548, 368, 562);
            TransferingData.ConnectionTwo.Id = Parser.GetStringValueFromRegion(File, rect);

            //FishingNeck
            rect = new iTextSharp.text.Rectangle(236, 523, 243, 530);
            TransferingData.FishingNeck = Parser.GetStringValueFromRegion(File, rect);

            //Version 
            rect = new iTextSharp.text.Rectangle(174, 34, 284, 40);
            TransferingData.Version = VersionExtractor.GetVersion(Parser.GetStringValueFromRegion(File, rect));

            return TransferingData;
        }
    }
}