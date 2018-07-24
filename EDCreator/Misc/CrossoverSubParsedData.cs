using System.Windows;
using FDCreator.Logic;

namespace FDCreator.Misc
{
    public class CrossoverSubParsedData : ICrossoverSubParsedData
    {

        private CrossoverType _type;

        public CrossoverType Type
        {
            get
            {
                var od1 = InchesValueRetriever.GetInchesValue(ConnectionOne.Od);
                var od2 = InchesValueRetriever.GetInchesValue(ConnectionTwo.Od);

                if (ConnectionOne.ConnectionType.ToUpper() == "BOX" && ConnectionTwo.ConnectionType.ToUpper() == "PIN")
                {

                    if (od1.Equals(od2))
                    {
                        _type = CrossoverType.Type1;
                    }
                    else
                    {
                        _type = od1 < od2 ? CrossoverType.Type3 : CrossoverType.Type4;
                    }
                }
                else if (ConnectionOne.ConnectionType.ToUpper() == "PIN" && ConnectionTwo.ConnectionType.ToUpper() == "PIN")
                {
                    if (od1.Equals(od2))
                        _type = CrossoverType.Type2;
                }
                else
                {
                   _type = CrossoverType.NotDefined;
                }

                return _type;
            }
        }

        public string FishingNeck { get; set; }
        public HeaderData Header { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Length { get; set; }
        public Connection ConnectionOne { get; set; }
        public Connection ConnectionTwo { get; set; }
        public string Version { get; set; }
    }
}