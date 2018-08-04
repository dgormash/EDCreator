using System.Collections.Generic;
using System.Windows;
using FDCreator.Logic.Implementations;
using FDCreator.Logic.Interfaces;
using FDCreator.Logic.SmartTools;
using FDCreator.Misc;

namespace FDCreator.Logic.RunableClients
{
    public class SmartToolClient
    {
        private readonly Dictionary<string, PartFile> _partsFiles;
        private readonly SmartToolType _toolType;
        private ISmartTool _tool;
        private IPdfProcessorCreator _pdfProcessorCreator;
        private ISmartToolExcelProcessorCreator _excelProcessorCreator;
        private IPdfProcessor _pdfProcessor;
        private ISmartToolExcelProcessor _excelProcessor;

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
            _tool = CreateTool();

            var partsData = new Dictionary<string, IParsedData>();

            foreach (var partFile in _partsFiles)
            {
                //Определили тулкод инспекции
                var toolCode = GetToolCode(partFile.Value.File);
                
                //Защита от файлов, не являющихся файлами инспекций или файлами инспекций, для которых нет обработчиков
                switch (GetFirstLettersOfToolCode(toolCode))
                {
                    case "MSSB":
                    case "MDC":
                    case "ARC":
                        _pdfProcessorCreator = new StandartPdfProcessorCreator();
                        _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                        _excelProcessorCreator = new GdisTelescopeExcelProcessorNpoiVersion();
                        _excelProcessor = _excelProcessorCreator.GetProcessor();
                        break;
                    default:
                        MessageBox.Show(
                            "A nonstandart name was received while reading the file. Perhaps there is no handler for the file, or the file is not an inspection file",
                            "Warining", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                }

                _pdfProcessor = new StandartPdfProcessor(); 
                var parsedData = _pdfProcessor.GetParsedDataFromPdf(partFile.Value.File);
                partsData.Add(partFile.Key, parsedData);
               
            }

            IParsedData data;
            partsData.TryGetValue("Top", out data);
            _tool.Top = data;
            if(_toolType == SmartToolType.Telescope || _toolType == SmartToolType.Gdis)
            {
                partsData.TryGetValue("Middle", out data);
                _tool.Middle = data;}

            partsData.TryGetValue("Bottom", out data);
            _tool.Bottom = data;

            SmartToolExcelProcessor excel;
            if (_toolType == SmartToolType.Arc)
            {
                excel = new ArcExcelProcessor();
            }
            else
            {
                excel = new SmartToolExcelProcessor();
            }

            excel.PassDataToExcel(_tool);

        }

        private static string GetToolCode(string file)
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