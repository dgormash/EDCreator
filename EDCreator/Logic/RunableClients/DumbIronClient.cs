using System.Windows;
using FDCreator.Logic.Implementations;
using FDCreator.Logic.Interfaces;
using FDCreator.Misc;

namespace FDCreator.Logic.RunableClients
{
    public class DumbIronClient
    {
        private HeaderData _header;
        private string _toolCode;
        private IPdfProcessor _pdfProcessor;
        private IExcelProcessor _excelProcessor;
        private IPdfProcessorCreator _pdfProcessorCreator;
        private IExcelProcessorCreator _excelProcessorCreator;

        public HeaderData Header
        {
            set { _header = value; }
        }

        public void Run(string file)
        {
            //1. Определяем тулкод изделия
            var preparser = new PdfPreparser();
            _toolCode = preparser.GetParsedDataFromPdf(file);

            var firstLetters = GetFirstLettersOfToolCode(_toolCode);

            switch (firstLetters) 
            {                       
                case "MFS":
                    _pdfProcessorCreator = new StandartPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new StandartExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    _excelProcessor.TemplateFileName = "FilterSub Diagram.xlsx";
                    break;
                case "SFS":
                    _pdfProcessorCreator = new StandartPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new StandartExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    _excelProcessor.TemplateFileName = "FloatSub Diagram.xlsx";
                    break;
                case "NMPC":
                    _pdfProcessorCreator = new StandartPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new StandartExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    _excelProcessor.TemplateFileName = "NMPC Diagram.xlsx";
                    break;
                case "SZS":
                    _pdfProcessorCreator = new StabilizerPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new StablizerExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    _excelProcessor.TemplateFileName = "Stabilizer Diagram.xlsx";
                    break;
                case "SBS":
                    _pdfProcessorCreator = new StandartPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new NearBitSubExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    _excelProcessor.TemplateFileName = "Near Bit Sub Diagram.xlsx";
                    break;
                case "SZB":
                    _pdfProcessorCreator = new StabilizerPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new NearBitSubStablizerExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    _excelProcessor.TemplateFileName = "Near Bit Stabilizer Diagram.xlsx";
                    break;
                case "NMDC":
                    _pdfProcessorCreator = new StandartPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new NmdcExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    _excelProcessor.TemplateFileName = "NMDC-F Diagram.xlsx";
                    break;
                case "SXO":
                    _pdfProcessorCreator = new StandartPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new CrossoverExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    _excelProcessor.TemplateFileName = "";//Здесь пусто, потому, что имя шаблона для кроссовера определяется в excel-процессоре
                    break;
                case "MSSB":
                    _pdfProcessorCreator = new StandartPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new CrossoverExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    _excelProcessor.TemplateFileName = "";//Здесь пусто, потому, что имя шаблона для кроссовера определяется в excel-процессоре
                    break;
                default:
                    MessageBox.Show(
                        "A nonstandart name was received while reading the file. Perhaps there is no handler for the file, or the file is not an inspection file",
                        "Warining", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
            }

            var parsedData = _pdfProcessor.GetParsedDataFromPdf(file);

            if (parsedData.Version != "1.0.11.0")
            {
                var proceedWhenVersionNotEqual = MessageBox.Show($"Version of the inspection file is \"{parsedData.Version}\", but we expected \"1.0.11.0\". Do you want to continue?",
                    "Information message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (proceedWhenVersionNotEqual == MessageBoxResult.No)
                    return;
            }
            parsedData.Name = _toolCode;
            parsedData.Header = _header;

            _excelProcessor.CreateFishingDiagram(parsedData);
        }

        private static string GetFirstLettersOfToolCode(string toolCode)
        {
            var substringableValue = toolCode.ToUpper();
            if (substringableValue.StartsWith("NMPC") || substringableValue.StartsWith("NMDC") || substringableValue.StartsWith("MSSB"))
            {
                return substringableValue.Substring(0, 4);
            }

            return substringableValue.Substring(0, 3);
        }
    }
}