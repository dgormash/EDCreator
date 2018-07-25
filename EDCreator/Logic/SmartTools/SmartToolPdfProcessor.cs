using FDCreator.Misc;

namespace FDCreator.Logic.SmartTools
{
    public class SmartToolPdfProcessor:PdfProcessor
    {
        public override IParsedData GetPdfData()
        {
            //Cheking for DPI position
            var rect = new iTextSharp.text.Rectangle(214, 681, 284, 687);
            var checkResult = Parser.GetStringValueFromRegion(File, rect);

            //SerialNumber
            rect = new iTextSharp.text.Rectangle(437, 722, 484, 728);
            TransferingData.SerialNumber = Parser.GetStringValueFromRegion(File, rect);

            if (checkResult.ToUpper().Contains("DPI"))
            {
                //Length shoulder to shoulder
                rect = new iTextSharp.text.Rectangle(145, 567, 169, 574);
                TransferingData.Length = Parser.GetStringValueFromRegion(File, rect);

                //Connection 1 Outer diameter
                TransferingData.ConnectionOne = new Connection();
                rect = new iTextSharp.text.Rectangle(109, 498, 130, 505);
                TransferingData.ConnectionOne.Od = Parser.GetStringValueFromRegion(File, rect);

                //Connection 2 Internal diameter
                TransferingData.ConnectionTwo = new Connection();
                rect = new iTextSharp.text.Rectangle(347, 483, 368, 489);
                TransferingData.ConnectionTwo.Id = Parser.GetStringValueFromRegion(File, rect);
            }
            else
            {
                //Length shoulder to shoulder
                rect = new iTextSharp.text.Rectangle(146, 640, 167, 648);
                TransferingData.Length = Parser.GetStringValueFromRegion(File, rect);

                //Connection 1 Outer diameter
                TransferingData.ConnectionOne = new Connection();
                rect = new iTextSharp.text.Rectangle(109, 571, 130, 578);
                TransferingData.ConnectionOne.Od = Parser.GetStringValueFromRegion(File, rect);

                //Connection 2 Internal diameter
                TransferingData.ConnectionTwo = new Connection();
                rect = new iTextSharp.text.Rectangle(347, 555, 368, 562);
                TransferingData.ConnectionTwo.Id = Parser.GetStringValueFromRegion(File, rect);
            }
            //Version 
            rect = new iTextSharp.text.Rectangle(174, 30, 284, 43);
            TransferingData.Version = VersionExtractor.GetVersion(Parser.GetStringValueFromRegion(File, rect));

            return TransferingData;
        }
    }
}