using FDCreator.Misc;

namespace FDCreator.Logic
{
    internal class NearBitStabilizerPdfProcessor : StabilizerPdfProcessor
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
                connectionInfo.TreadSize = stringArray[0] + stringArray [1];
                connectionInfo.Od = stringArray[3];
            }
            return connectionInfo;
        }
    }
}
