using FDCreator.Misc;

namespace FDCreator.Logic.SmartTools
{
    public class SmartToolPdfProcessor:PdfProcessor
    {
        public override IParsedData GetPdfData()
        {

            //mdc id 1    4 8/32 at lx: 111,72; ly: 555,9988; rx: 129,0547; ry: 562,2388
            
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

                //Treadsize 1 
                TransferingData.ConnectionOne = new Connection();
                rect = new iTextSharp.text.Rectangle(103, 527, 118, 533);
                TransferingData.ConnectionOne.TreadSize = Parser.GetStringValueFromRegion(File, rect);

                //Connection 1 type
                // 103,32; ly: 535,2388; rx: 116,4802; ry: 541,4788
                rect = new iTextSharp.text.Rectangle(103, 535, 116, 541);
                TransferingData.ConnectionOne.ConnectionType = Parser.GetStringValueFromRegion(File, rect);

                //Connection 1 Outer diameter
                rect = new iTextSharp.text.Rectangle(109, 498, 130, 505);
                TransferingData.ConnectionOne.Od = Parser.GetStringValueFromRegion(File, rect);
                
                //Connection 2 Internal diameter
                TransferingData.ConnectionTwo = new Connection();
                rect = new iTextSharp.text.Rectangle(347, 483, 368, 489);
                TransferingData.ConnectionTwo.Id = Parser.GetStringValueFromRegion(File, rect);

                //Connection 2 type
                //339,84; ly: 535,2388; rx: 350,2296; ry: 541,4788
                rect = new iTextSharp.text.Rectangle(339, 535, 350, 541);
                TransferingData.ConnectionTwo.ConnectionType = Parser.GetStringValueFromRegion(File, rect);

                //Treadsize 2 
                rect = new iTextSharp.text.Rectangle(339, 527, 355, 533);
                TransferingData.ConnectionTwo.TreadSize = Parser.GetStringValueFromRegion(File, rect);

                //Connection 2 Outer diameter
                rect = new iTextSharp.text.Rectangle(347, 498, 368, 505);
                TransferingData.ConnectionTwo.Od = Parser.GetStringValueFromRegion(File, rect);
            }
            else
            {
                //Length shoulder to shoulder
                rect = new iTextSharp.text.Rectangle(146, 640, 167, 648);
                TransferingData.Length = Parser.GetStringValueFromRegion(File, rect);

                //Treadsize 1 
                TransferingData.ConnectionOne = new Connection();
                rect = new iTextSharp.text.Rectangle(103, 600, 118, 606);
                TransferingData.ConnectionOne.TreadSize = Parser.GetStringValueFromRegion(File, rect);

                //Connection 1 type
                //lx: 103,32; ly: 608,1987; rx: 113,7096; ry: 614,4387
                rect = new iTextSharp.text.Rectangle(103, 608, 113, 614);
                TransferingData.ConnectionOne.ConnectionType = Parser.GetStringValueFromRegion(File, rect);

                //Connection 1 Outer diameter
                rect = new iTextSharp.text.Rectangle(109, 571, 130, 578);
                TransferingData.ConnectionOne.Od = Parser.GetStringValueFromRegion(File, rect);

                //Connection 1 Internal diameter
                rect = new iTextSharp.text.Rectangle(111, 555, 129, 562);
                TransferingData.ConnectionOne.Id = Parser.GetStringValueFromRegion(File, rect);

                
                //Treadsize 2 
                TransferingData.ConnectionTwo = new Connection();
                rect = new iTextSharp.text.Rectangle(339, 600, 355, 606);
                TransferingData.ConnectionTwo.TreadSize = Parser.GetStringValueFromRegion(File, rect);

                //Connection 2 type
                //lx: 339,84; ly: 608,1987; rx: 350,2296; ry: 614,4387
                rect = new iTextSharp.text.Rectangle(339, 608, 350, 614);
                TransferingData.ConnectionTwo.Id = Parser.GetStringValueFromRegion(File, rect);

                //Connection 2 Outer diameter
                rect = new iTextSharp.text.Rectangle(347, 571, 368, 578);
                TransferingData.ConnectionTwo.Od = Parser.GetStringValueFromRegion(File, rect);


                //mdc id 2    4 8/32 at lx: 349,56; ly: 555,9988; rx: 366,8947; ry: 562,2388
                //Connection 2 Internal diameter
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