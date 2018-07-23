using FDCreator.Misc;

namespace FDCreator.Logic
{
    //PdfProcessor - ЭТО БАЗОВЫЙ КЛАСС ДЛЯ ВСЕХ ВИДОВ PDF-ПРОЦЕССОРОВ. ЗДЕСЬ ЕСТЬ 2 ВИРТУАЛЬНЫХ (ИХ МОЖНО ПЕРЕОПРЕДЕЛИТЬ В НАСЛЕДНИКАХ) МЕТОДА:
    //GetPdfData И FillConnectionInfo. ЕСЛИ, СОЗДАВАЯ НОВУЮ ВЕРСИЮ PDF-ПРОЦЕССОРА ВЫ ЭТИ МЕТОДЫ НЕ ПЕРЕОПРЕДЕЛЯЕТЕ, ОНИ ВЫПОЛНЯЮТСЯ КАК ЗДЕСЬ
    //ПЕРЕОПРЕДЕЛЯТЬ НАДО, ЕСЛИ: PDF-ФАЙЛ ОТЛИЧАЕТСЯ ПО ФОРМЕ ИЛИ ПО ЗАДАННЫМ ЗДЕСЬ КООРДИНАТАМ НУЖНЫЕ ДАННЫЕ НЕ ВОЗВРАЩАЮТСЯ; НЕОБХОДИМО ВЫТАЩИТЬ
    //ИНФОРМАЦИЮ, КОТОРАЯ ЗДЕСЬ НЕ ВЫТАСКИВАЕТСЯ.


    public class PdfProcessor
    {
        protected readonly IPdfParser Parser;
        public string File { set; protected get; }
        protected ParsedData TransferingData = new ParsedData();

        public PdfProcessor()
        {
            Parser = new PdfParser();
        }
        public virtual ParsedData GetPdfData()
        {
            //Координаты задаются просто: первая пара - это нижняя правая точка по x и y, вторая - верхняя левая точка
            //Получается прямоугольник, он задаётся в качестве фильтра методу-парсеру, и он выбирает все данные, которые попадают
            //в этот прямоугольник. (ps: туда может попасть что-то лишнее)

            //Чтобы узнать координаты нужной информации см. проект PdfReader

            //SerialNumber
            var  rect = new iTextSharp.text.Rectangle(437, 722, 460, 728);
            TransferingData.SerialNumber = Parser.GetStringValueFromRegion(File, rect);

            //Length shoulder to shoulder
            rect = new iTextSharp.text.Rectangle(148, 640, 165, 647);
            TransferingData.Length = Parser.GetStringValueFromRegion(File, rect);

            //Connection 1 Type, TreadSize
            rect = new iTextSharp.text.Rectangle(29, 584, 126, 614);
            var connectionColumn = Parser.GetStringValueFromRegion(File, rect).Split('\n');
            TransferingData.ConnectionOne = FillConnectionInfo(connectionColumn);
            //Connection 1 Outer diameter
            rect = new iTextSharp.text.Rectangle(109, 564, 130, 578);
            TransferingData.ConnectionOne.Od = Parser.GetStringValueFromRegion(File, rect);
            //Connection 1 Internal diameter
            rect = new iTextSharp.text.Rectangle(111, 548, 129, 554);
            TransferingData.ConnectionOne.Id = Parser.GetStringValueFromRegion(File, rect);

            //Connection 2 Type, TreadSize
            rect = new iTextSharp.text.Rectangle(259, 584, 362, 614);
            connectionColumn = Parser.GetStringValueFromRegion(File, rect).Split('\n');
            TransferingData.ConnectionTwo = FillConnectionInfo(connectionColumn);
            //Connection 2 Outer diameter
            rect = new iTextSharp.text.Rectangle(347, 564, 368, 578);
            TransferingData.ConnectionTwo.Od = Parser.GetStringValueFromRegion(File, rect);
            //Connection 2 Internal diameter
            rect = new iTextSharp.text.Rectangle(347, 548, 368, 562);
            TransferingData.ConnectionTwo.Id = Parser.GetStringValueFromRegion(File, rect);

            //Version 
            rect = new iTextSharp.text.Rectangle(174, 34, 284, 40);
            TransferingData.Version = VersionExtractor.GetVersion(Parser.GetStringValueFromRegion(File, rect));

            return TransferingData;
        }

        protected virtual Connection FillConnectionInfo(string[] stringArray)
        {
            var connectionInfo = new Connection();
            switch (stringArray.Length)
            {
                case 3:
                    connectionInfo.ConnectionType = stringArray[0].Substring(5);
                    connectionInfo.TreadSize = stringArray[1].Substring(10);
                    break;
                case 5:
                    connectionInfo.ConnectionType = stringArray[0].Substring(5);
                    connectionInfo.TreadSize = $"{stringArray[1]} {stringArray[3]}";
                    break;
            }
            return connectionInfo;
        }
    }
}