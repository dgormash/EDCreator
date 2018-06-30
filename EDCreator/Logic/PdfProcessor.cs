using EDCreator.Misc;

namespace EDCreator.Logic
{
    public class PdfProcessor
    {
        protected readonly IPdfParser Parser;
        public string File { set; protected get; }

        public PdfProcessor()
        {
            Parser = new PdfParser();
        }
        public virtual ParsedData GetPdfData()
        {
            var transferingData = new ParsedData();

            //SerialNumber
            var  rect = new iTextSharp.text.Rectangle(437, 722, 460, 728);
            transferingData.SerialNumber = Parser.GetStringValueFromRegion(File, rect);

            //Length shoulder to shoulder
            rect = new iTextSharp.text.Rectangle(148, 640, 165, 647);
            transferingData.Length = Parser.GetStringValueFromRegion(File, rect);

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

        protected virtual Connection FillConnectionInfo(string[] stringArray)
        {
            var connectionInfo = new Connection();
            if (stringArray.Length == 4)
            {
                connectionInfo.TreadSize = $"{stringArray[0]} {stringArray[1]}";
                connectionInfo.Od = stringArray[3];
            }
            else
            {
                connectionInfo.TreadSize = $"{stringArray[0]} {stringArray[1]}";
                connectionInfo.Od = stringArray[3];
                connectionInfo.Id = stringArray[4];
            }
            return connectionInfo;
        }
    }
}