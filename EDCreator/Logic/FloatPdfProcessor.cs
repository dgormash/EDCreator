using EDCreator.Misc;

namespace EDCreator.Logic
{
    public class FloatPdfProcessor:PdfProcessor
    {
        protected override Connection FillConnectionInfo(string[] stringArray)
        {
            var connectionInfo = new Connection();
            if (stringArray.Length == 3)
            {
                connectionInfo.TreadSize = stringArray[0];
                connectionInfo.Od = stringArray[2];
            }
            else
            {
                connectionInfo.TreadSize = stringArray[0];
                connectionInfo.Od = stringArray[2];
                connectionInfo.Id = stringArray[3];
            }
            return connectionInfo;
        }

        
    }
}