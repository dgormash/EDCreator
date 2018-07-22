using System.Windows;
using FDCreator.Misc;

namespace FDCreator.Logic
{
    public class Client
    {
        private readonly IPdfParser _pdfParser;
        private  HeaderData _header;

        public HeaderData Header
        {
            set { _header = value; }
        }
        public  string SessionStartTime { get; set; }

        public Client()
        {
            _pdfParser = new PdfParser();
        }

        public void Run(string file)
        {
            //Здесь мы задаём координаты, по которым ищется имя в файле инспекции и получаем имя в переменную name
            //Во всех .pdf, которые вы предоставили, имя находится в одном месте, вероятно они формируются программно
            //поэтому бояться, что в координатах что-то поменяется, не приходится (если, конечно, не изменится что-то
            //в процессе формирования .pdf
            var rect = new iTextSharp.text.Rectangle(296, 722, 323, 728);
            var name = GetInspectionNameFromPdf(file, rect);

            PdfProcessor processor;
            ExcelProcessor excel;

            //В блоке switch мы проверяем полученное имя на соответсвие с заранее определёнными вариантами
            //Когда появятся новые файлы инспекций в это блок необходимо будет добавить по аналогии новые варианты
            //Если есть необходимость, надо создать новые версии PdfProcessor и ExcelProcessor
            //Необходимость возникнет, если pdf - файлы или excel-файлы будут отличаться от тех, которые вы предоставляли.
            var comparableName = GetFirstLettersOfToolCode(name);
            switch (comparableName) //ToUpper на всякий случай, это перевод в верхний регистр символов. Сравнение строк в верхнем регистре
            {                       //лучше оптимизировано, ну и это позволяет избежать нежелательного поведения, если вдруг в .pdf случайно 
                                    //имя будет в нижнем регистре
                case "MFS":
                    //Для каждого случая вызываем свою версию PdfProcessor и ExcelProcessor потому как файлы могут обрабатываться по-разному
                    processor = GetPdfProcessor(PdfProcessorType.FilterSub); //Перечисление PdfProcessorType находится в файле PdfProcessorType.cs
                    excel = GetExcelProcessor(ExcelProcessorType.FilterExcelProcessor); //Перечисление ExcelProcessorType находится в ExcelProcessorType.cs
                    //diagramType = ExcelDiagramType.FilterSubDiagram;
                    break;
                case "SFS":
                    processor = GetPdfProcessor(PdfProcessorType.FloatSub);
                    excel = GetExcelProcessor(ExcelProcessorType.FloatExcelProcessor);
                    //diagramType = ExcelDiagramType.FloatSubDiagram;
                    break;
                case "NMPC":
                    processor = GetPdfProcessor(PdfProcessorType.Nmpc);
                    excel = GetExcelProcessor(ExcelProcessorType.NmpcExcelProcessor);
                    //diagramType = ExcelDiagramType.NmpcDiagram;
                    break;
                case "SZS":
                    processor = GetPdfProcessor(PdfProcessorType.Stabilizer);
                    excel = GetExcelProcessor(ExcelProcessorType.StabilizerExcelProcessor);
                    break;
                case "SBS":
                    processor = GetPdfProcessor(PdfProcessorType.NearBitSub);
                    excel = GetExcelProcessor(ExcelProcessorType.NearBitSubExcelProcessor);
                    break;
                case "SZB":
                    processor = GetPdfProcessor(PdfProcessorType.NearBitStabilizer);
                    excel = GetExcelProcessor(ExcelProcessorType.NearBitStabilizerExcelProcessor);
                    break;
                case "NDMC":
                    processor = GetPdfProcessor(PdfProcessorType.FlexNmdc);
                    excel = GetExcelProcessor(ExcelProcessorType.FlexNmdcExcelProcessor);
                    break;
                case "SXO":
                    processor = GetPdfProcessor(PdfProcessorType.Crossover);
                    excel = GetExcelProcessor(ExcelProcessorType.CrossoverExcelProcessor);
                    break;
                default:
                    MessageBox.Show(
                        "A nonstandart name was received while reading the file. Perhaps there is no handler for the file, or the file is not an inspection file",
                        "Warining", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
            }

            processor.File = file;
            var parsedData = processor.GetPdfData(); //В выбранной версии PdfProcessor запускаем процедуру парсинга
            if (parsedData.Version != "1.0.11.0")
            {
                var proceedWhenVersionNotEqual = MessageBox.Show("Information message",
                    $"Version of inspection is {parsedData.Version}, but we expected 1.0.11.0. Do you want to continue?", MessageBoxButton.YesNo);
                if (proceedWhenVersionNotEqual == MessageBoxResult.No)
                    return;
            }
            parsedData.Name = name;
            parsedData.Header = _header;
            
            excel?.PassDataToExcel(parsedData); //В выбранной версии ExcelProcessor запускаем процедуру записи данных в excel-шаблоны
        }

        private static string GetFirstLettersOfToolCode(string name)
        {
            var substringableValue = name.ToUpper();
            if (substringableValue.StartsWith("NMPC") || substringableValue.StartsWith("NDMC"))
            {
                return substringableValue.Substring(0, 4);
            }

            return substringableValue.Substring(0, 3);
        }

        //Метод возвращает имя инспекции, найденное в переданнов файле
        private string GetInspectionNameFromPdf(string file, iTextSharp.text.Rectangle rectangle)
        {
            return _pdfParser.GetStringValueFromRegion(file, rectangle);
        }


        //Метод возвращает новый PdfProcessor версии, опредлённой в PdfProcessorType (если создаётся новая версия процессора, в PdfProcessorType
        //необходимо добавить, по аналогии с имеющимися там, варианты, и, соответственно, добавить в этот метод обработку новых типов
        private static PdfProcessor GetPdfProcessor(PdfProcessorType type)
        {
            switch (type)
            {
                case PdfProcessorType.FloatSub:
                    return new FloatPdfProcessor(); //Здесь создаётся потомок от базового класса PdfProcessor и в нем переопределяется метод FillConnectionInfo
                case PdfProcessorType.Nmpc:
                    return new FloatPdfProcessor();//Здесь я не стал делать новый класс, потому как он работает так же, как и FloatPdfProcessor
                case PdfProcessorType.FilterSub:
                    return new PdfProcessor(); //Здесь создаётся экземпляр базового класса PdfProcessor
                case PdfProcessorType.Stabilizer:
                    return new StabilizerPdfProcessor();
                case PdfProcessorType.NearBitSub:
                    return new FloatPdfProcessor(); ;
                case PdfProcessorType.NearBitStabilizer:
                    return new FloatPdfProcessor(); ;
                case PdfProcessorType.FlexNmdc:
                    return new FloatPdfProcessor(); ;
                case PdfProcessorType.Crossover:
                    return new FloatPdfProcessor(); ;
                case PdfProcessorType.Empty:
                    return new FloatPdfProcessor(); ;
                default:
                    return null;
            }
        }


        //Здесь всё то же, что и для метода GetPdfProcessor - будет что-то новое, добавляете в ExcelProcessorType новый вариант
        //и в этом методе добавляете его обработку
        private  ExcelProcessor GetExcelProcessor(ExcelProcessorType type)
        {
            switch (type)
            {
                case ExcelProcessorType.FilterExcelProcessor:
                    return new FilterExcelProcessor(SessionStartTime);
                case ExcelProcessorType.StabilizerExcelProcessor:
                    return new StabilizerExcelProcessor(SessionStartTime);
                case ExcelProcessorType.FloatExcelProcessor:
                    return  new FloatExcelProcessor(SessionStartTime);
                case ExcelProcessorType.NmpcExcelProcessor:
                    return new NmpcExcelProcessor(SessionStartTime);
                case ExcelProcessorType.ExcelProcessor:
                    return new FloatExcelProcessor(SessionStartTime);
                case ExcelProcessorType.NearBitSubExcelProcessor:
                    return new FloatExcelProcessor(SessionStartTime);
                case ExcelProcessorType.NearBitStabilizerExcelProcessor:
                    return new FloatExcelProcessor(SessionStartTime);
                case ExcelProcessorType.FlexNmdcExcelProcessor:
                    return new FloatExcelProcessor(SessionStartTime);
                case ExcelProcessorType.CrossoverExcelProcessor:
                    return new FloatExcelProcessor(SessionStartTime);
                default:
                    return null;
            }
        }
    }
}