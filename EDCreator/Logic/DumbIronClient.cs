using System.Windows;
using FDCreator.Logic.Implementations;
using FDCreator.Logic.Interfaces;
using FDCreator.Misc;

namespace FDCreator.Logic
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
                case "SFS":
                case "NMPC":
                    _pdfProcessorCreator = new StandartPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new StandartExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    break;
                case "SZS":
                    _pdfProcessorCreator = new StabilizerPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new StablizerExcelProcessorNpoiVersion();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    break;
                case "SBS":
                    _pdfProcessorCreator = new StandartPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new NearBitSubExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    break;
                case "SZB":
                    _pdfProcessorCreator = new StabilizerPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new NearBitSubStablizerExcelProcessorNpoiVersion();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    break;
                case "NMDC":
                    _pdfProcessorCreator = new StandartPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new NmdcExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    break;
                case "SXO":
                    _pdfProcessorCreator = new StandartPdfProcessorCreator();
                    _pdfProcessor = _pdfProcessorCreator.GetProcessor();
                    _excelProcessorCreator = new CrossoverExcelProcessorNpoiVersionCreator();
                    _excelProcessor = _excelProcessorCreator.GetProcessor();
                    break;
                default:
                    MessageBox.Show(
                        "A nonstandart name was received while reading the file. Perhaps there is no handler for the file, or the file is not an inspection file",
                        "Warining", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
            }
        }

        private static string GetFirstLettersOfToolCode(string toolCode)
        {
            var substringableValue = toolCode.ToUpper();
            if (substringableValue.StartsWith("NMPC") || substringableValue.StartsWith("NMDC"))
            {
                return substringableValue.Substring(0, 4);
            }

            return substringableValue.Substring(0, 3);
        }
    }
}