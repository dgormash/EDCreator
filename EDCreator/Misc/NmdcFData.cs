using System.Collections.Generic;

namespace FDCreator.Misc
{
    public class NmdcFData
    {
        public Dictionary<string, NmdcFTool> Tools { get; }

        public NmdcFData()
        {
            Tools = new Dictionary<string, NmdcFTool>
            {
                {
                    "OSS14-00453E3",
                    new NmdcFTool
                    {
                        L = "9.29",
                        L1 = "1",
                        L2 = "1.04",
                        L3 = "3.12",
                        L4 = "3.14",
                        L5 = "3.62",
                        L6 = "3.65",
                        L7 = "5.61",
                        L8 = "5.63",
                        L9 = "10.12",
                        L10 = "10.15",
                        L11 = "8.1",
                        L12 = "8.13",
                        Id = "3",
                        Od = "8",
                        Od1 = "8 1/16",
                        Od2 = "6 11/16",
                        Od3 = "7 7/8",
                        Od4 = "6 11/16",
                        Od5 = "7 13/16",
                        Od6 = "6 11/16"
                    }
                },
                {"OSS14-00453E1",
                    new NmdcFTool
                    {
                        L = "8.94",
                        L1 = "0.78",
                        L2 = "0.8",
                        L3 = "2.98",
                        L4 = "2.92",
                        L5 = "3.38",
                        L6 = "3.41",
                        L7 = "5.39",
                        L8 = "5.4",
                        L9 = "5.87",
                        L10 = "5.92",
                        L11 = "7.87",
                        //L12 = "7.9",
                        Id = "2 13/16",
                        Od = "7 5/8",
                        Od1 = "7 13/16",
                        Od2 = "6 11/16",
                        Od3 = "7 11/16",
                        Od4 = "6 11/16",
                        Od5 = "7 11/16",
                        Od6 = "6 11/16"
                    }
                }
            };
       }
   }
}
