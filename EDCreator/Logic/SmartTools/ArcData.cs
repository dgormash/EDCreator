using System.Collections.Generic;

namespace FDCreator.Logic.SmartTools
{
    public class ArcData
    {
        public Dictionary<string, ArcTool> Tools { get; }

        public ArcData()
        {
            Tools = new Dictionary<string, ArcTool>
            {
                {"5883", new ArcTool
                    {
                        L =   "5,54",
                        L1  =   "0,33",
                        L2  =   "0,51",
                        L3  =   "1,63",
                        L4  =   "1,77",
                        L5  =   "2,29",
                        L6  =   "2,44",
                        L7  =   "3,23",
                        L8  =   "3,27",
                        L9  =   "3,9",
                        L10 =   "4,05",
                        Od1 =   "8",
                        Od2 =   "9 3/32",
                        Od3 =   "9 3/32",
                        Od4 =   "9 3/32",
                        Od5 =   "8",
                        Od6 =   "8 3/8",
                        Od7 =   "9 1/16",
                        Od8 =   "8 3/8"
                    }
                }
            };
        }
    }
}