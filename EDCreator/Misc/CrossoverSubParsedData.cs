using System.Windows;
using FDCreator.Logic;

namespace FDCreator.Misc
{
    public class CrossoverSubParsedData:ParsedData
    {
     
        public CrossoverType Type { get; protected set; }

        public virtual void SetOwnerType()
        {

            var od1 = InchesValueRetriever.GetInchesValue(ConnectionOne.Od);
            var od2 = InchesValueRetriever.GetInchesValue(ConnectionTwo.Od);

            if (ConnectionOne.ConnectionType.ToUpper() == "BOX" && ConnectionTwo.ConnectionType.ToUpper() == "PIN")
            {
                
                if (od1.Equals(od2))
                {
                    Type = CrossoverType.Type1;
                }

                Type = od1 < od2 ? CrossoverType.Type3 : CrossoverType.Type4;
            }
            else if (ConnectionOne.ConnectionType.ToUpper() == "PIN" && ConnectionTwo.ConnectionType.ToUpper() == "PIN")
            {
                if(od1.Equals(od2))
                    Type = CrossoverType.Type2;
            }
            else
            {
                MessageBox.Show("Crossover Sub Type Not Defined", "Information message", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                Type = CrossoverType.NotDefined;
            }
        }
    }
}