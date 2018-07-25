using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using FDCreator.Misc;

namespace FDCreator.Logic.SmartTools
{
    public class SmartToolClient
    {
        private Dictionary<string, PartFile> _partsFiles;
        private readonly SmartToolType _toolType;
        private ISmartTool _tool;

        public SmartToolClient(Dictionary<string, PartFile> partsFiles, SmartToolType toolType)
        {
            _partsFiles = partsFiles;
            _toolType = toolType;
        }

        private ISmartTool CreateTool()
        {
            ISmartTool tool;
            switch (_toolType)
            {
                case SmartToolType.Telescope:
                    tool = new Telescope();
                    break;
                case SmartToolType.Gdis:
                    tool = new Gdis();
                    break;
                case SmartToolType.Arc:
                    tool = new Arc();
                    break;
                default:
                    return null;
            }
            return tool;
        }

        public void Run()
        {
            var createdTool = CreateTool();
            IParsedData parsedData;
            PdfProcessor processor;
            var partsData = new Dictionary<string, IParsedData>();

            foreach (var partFile in _partsFiles)
            {
                //Определили тулкод инспекции
                var toolCode = GetToolCode(partFile.Value.File);
                
                //По коду определяем версию парсера
                switch (GetFirstLettersOfToolCode(toolCode))
                {
                    case "MSSB":
                        break;
                    case "MDC":
                        //parsedData = parser for MDC
                        break;
                    case "ARC":
                        //parsedData = parser for ARC
                        break;
                    default:
                        MessageBox.Show(
                            "A nonstandart name was received while reading the file. Perhaps there is no handler for the file, or the file is not an inspection file",
                            "Warining", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                }

                processor = new PdfProcessor { File = partFile.Value.File };
                parsedData = processor.GetPdfData();
                partsData.Add(nameof(partFile.Value.Type), parsedData);
               
            }

            IParsedData data;
            switch (_toolType)
            {
                case SmartToolType.Telescope:
                    var telescope = (Telescope)CreateTool();
                    partsData.TryGetValue("Top", out data);
                    telescope.Top = data;
                    partsData.TryGetValue("Middle", out data);
                    telescope.Middle = data;
                    partsData.TryGetValue("Bottom", out data);
                    telescope.Bottom = data;
                    //Pass data to excel-processor
                    break;

                case SmartToolType.Gdis:
                    var gdis = (Gdis)CreateTool();
                    partsData.TryGetValue("Top", out data);
                    gdis.Top = data;
                    partsData.TryGetValue("Middle", out data);
                    gdis.Middle = data;
                    partsData.TryGetValue("Bottom", out data);
                    gdis.Bottom = data;
                    //Pass data to excel-processor
                    break;

                case SmartToolType.Arc:
                    var arc = (Arc)CreateTool();
                    partsData.TryGetValue("Top", out data);
                    arc.Top = data;
                    partsData.TryGetValue("Bottom", out data);
                    arc.Bottom = data;
                    //Pass data to excel-processor
                    break;
            }
            
        }

        private string GetToolCode(string file)
        {
            IPdfParser parser = new PdfParser();
            var rect = new iTextSharp.text.Rectangle(296, 722, 323, 728);
            return parser.GetStringValueFromRegion(file, rect);
        }

        private static string GetFirstLettersOfToolCode(string name)
        {
            var substringableValue = name.ToUpper();
            if (substringableValue.StartsWith("MSSB"))
            {
                return substringableValue.Substring(0, 4);
            }

            return substringableValue.Substring(0, 3);
        }
    }
}