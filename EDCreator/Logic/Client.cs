using EDCreator.Misc;

namespace EDCreator.Logic
{
    public class Client
    {
        private readonly IPdfParser _pdfParser;
        private readonly LoginData _header;

        public Client(LoginData header)
        {
            _pdfParser = new PdfParser();
            _header = header;
        }

        public void Run(string file)
        {
            var rect = new iTextSharp.text.Rectangle(296, 722, 323, 728);
            var name = GetInspectionNameFromPdf(file, rect);

            PdfProcessor processor;
            ExcelProcessor excel = null;
            //ExcelDiagramType //diagramType = 0;
            switch (name)
            {
                case "MFS6-AB":
                    processor = GetPdfProcessor(PdfProcessorType.FilterSub);
                    excel = GetExcelProcessor(ExcelProcessorType.ExcelProcessor);
                    //diagramType = ExcelDiagramType.FilterSubDiagram;
                    break;
                case "SFS8N":
                    processor = GetPdfProcessor(PdfProcessorType.FloatSub);
                    excel = GetExcelProcessor(ExcelProcessorType.ExcelProcessor);
                    //diagramType = ExcelDiagramType.FloatSubDiagram;
                    break;
                case "NMPC8":
                    processor = GetPdfProcessor(PdfProcessorType.Nmpc);
                    excel = GetExcelProcessor(ExcelProcessorType.ExcelProcessor);
                    //diagramType = ExcelDiagramType.NmpcDiagram;
                    break;
                case "SZS9N-IBS":
                    processor = GetPdfProcessor(PdfProcessorType.Stabilizer);
                    excel = GetExcelProcessor(ExcelProcessorType.StabilizerExcelProcessor);
                    //diagramType = ExcelDiagramType.StabilizerDiagram;
                    break;
                default:
                    //todo Сделать EmtpyPdfProcessor с переопредлёнными методами, которые просто будут показывать сообщения, что что-то не так
                    processor = GetPdfProcessor(PdfProcessorType.FilterSub);
                    break;
            }

            processor.File = file;
            var parsedData = processor.GetPdfData();
            parsedData.Name = name;
            parsedData.Header = _header;
            excel.PassDataToExcel(parsedData);//todo Передача в Excel
        }

        private string GetInspectionNameFromPdf(string file, iTextSharp.text.Rectangle rectangle)
        {
            return _pdfParser.GetStringValueFromRegion(file, rectangle);
        }

        private PdfProcessor GetPdfProcessor(PdfProcessorType type)
        {
            switch (type)
            {
                case PdfProcessorType.FloatSub:
                    return new FloatPdfProcessor();
                case PdfProcessorType.Nmpc:
                    return new FloatPdfProcessor();
                case PdfProcessorType.FilterSub:
                    return new PdfProcessor();
                case PdfProcessorType.Stabilizer:
                    return new StabilizerPdfProcessor();
                default:
                    return null;
            }
        }

        private ExcelProcessor GetExcelProcessor(ExcelProcessorType type)
        {
            switch (type)
            {
                case ExcelProcessorType.FilterExcelProcessor:
                    return new FilterExcelProcessor();
                case ExcelProcessorType.StabilizerExcelProcessor:
                    return new StabilizerExcelProcessor();
                case ExcelProcessorType.FloatExcelProcessor:
                    return  new FloatExcelProcessor();
                case ExcelProcessorType.NmpcExcelProcessor:
                    return new NmpcExcelProcessor();
                default:
                    return null;
            }
        }
    }
}