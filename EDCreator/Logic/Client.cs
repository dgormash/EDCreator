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
            switch (name)
            {
                case "MFS6-AB":
                    processor = GetPdfProcessor(PdfProcessorType.FilterSub);
                    break;
                case "SFS8N":
                    processor = GetPdfProcessor(PdfProcessorType.FloatSub);
                    break;
                case "NMPC8":
                    processor = GetPdfProcessor(PdfProcessorType.Nmpc);
                    break;
                case "SZS9N-IBS":
                    processor = GetPdfProcessor(PdfProcessorType.Stabilizer);
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
            //todo Передача в Excel
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
                    //todo StabilizerPdfProcessor
                    return null;
             }

            return null;
        }
    }
}