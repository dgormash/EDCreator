using EDCreator.Misc;

namespace EDCreator.Logic
{
    public class StabilizerPdfProcessor:PdfProcessor
    {
        public override ParsedData GetPdfData()
        {
            var transferingData = new StabilizerParsedData();

            //SerialNumber
            var rect = new iTextSharp.text.Rectangle(437, 722, 476, 728);
            transferingData.SerialNumber = Parser.GetStringValueFromRegion(File, rect);
            //437.16; ly: 722.0787; rx: 476.3347; ry: 728

            //Length shoulder to shoulder
            rect = new iTextSharp.text.Rectangle(148, 640, 167, 647);
            transferingData.Length = Parser.GetStringValueFromRegion(File, rect);
            //146.88; ly: 640.8387; rx: 167.6842; ry: 647

            //308.04; ly: 640.8387; rx: 325.3747; ry: 647
            //Stabilizer OD
            rect = new iTextSharp.text.Rectangle(308, 640, 325, 647);
            transferingData.StabilizerOd = Parser.GetStringValueFromRegion(File, rect);

            //Lobe Length
            //313.2; ly: 632.6788; rx: 320.1389; ry: 638
            rect = new iTextSharp.text.Rectangle(313, 632, 320, 638);
            transferingData.LobeLength = Parser.GetStringValueFromRegion(File, rect);

            //Lobe Width
            //315; ly: 624.5187; rx: 318.4695; ry: 630
            rect = new iTextSharp.text.Rectangle(315, 624, 318, 630);
            transferingData.LobeWidth = Parser.GetStringValueFromRegion(File, rect);

            //Fishing neck/Tong space
            //467.04; ly: 523.8387; rx: 484.3747; ry: 530
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