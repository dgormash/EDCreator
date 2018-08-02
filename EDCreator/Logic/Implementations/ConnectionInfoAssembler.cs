using FDCreator.Logic.Interfaces;
using FDCreator.Misc;

namespace FDCreator.Logic.Implementations
{
    public class ConnectionInfoAssembler:IConnectionInfoAssembler
    {
        public Connection FillConnectionInfo(string[] stringArray)
        {
            var connectionInfo = new Connection();
            switch (stringArray.Length)
            {
                case 3:
                    connectionInfo.ConnectionType = stringArray[0].Substring(5);
                    connectionInfo.TreadSize = stringArray[1].Substring(10);
                    break;
                case 5:
                    connectionInfo.ConnectionType = stringArray[0].Substring(5);
                    connectionInfo.TreadSize = $"{stringArray[1]} {stringArray[3]}";
                    break;
            }
            return connectionInfo;
        }
    }
}