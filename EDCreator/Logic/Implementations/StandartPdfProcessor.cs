using System.IO;
using FDCreator.Logic.Interfaces;
using FDCreator.Misc;

namespace FDCreator.Logic.Implementations
{
    public class StandartPdfProcessor:IPdfProcessor
    {
        private readonly IConnectionInfoAssembler _connectionInfoAssembler = new ConnectionInfoAssembler();
        private readonly IPdfParser _parser = new PdfParser();
        private readonly IParsedData _transferingData = new ParsedData();
        public IParsedData GetParsedDataFromPdf(string file)
        {
            //SerialNumber
            //lx: 411.12; ly: 727.0865; rx: 436.2746; ry: 732.9665
            var rect = new iTextSharp.text.Rectangle(437, 722, 460, 728);
            _transferingData.SerialNumber = _parser.GetStringValueFromRegion(file, rect);

            //Length shoulder to shoulder
            //lx: 137.52; ly: 649.9265; rx: 160.3932; ry: 655.8065
            rect = new iTextSharp.text.Rectangle(148, 640, 165, 647);
            _transferingData.Length = _parser.GetStringValueFromRegion(file, rect);

            //Connection 1 Type, TreadSize
            //Type at lx: 29.64; ly: 610.9265; rx: 42.70536; ry: 616.8065
            //BOX at lx: 98.64; ly: 610.9265; rx: 111.0409; ry: 616.8065
            //Treadsize at lx: 29.64; ly: 603.1265; rx: 55.44732; ry: 609.0065
            //6 5/8  at lx: 98.64; ly: 603.1265; rx: 113.3341; ry: 609.0065
            //Reg at lx: 113.64; ly: 603.1265; rx: 124.4239; ry: 609.0065
            //Condition at lx: 29.64; ly: 595.3265; rx: 54.47124; ry: 601.2065
            //NDF at lx: 98.64; ly: 595.3265; rx: 110.7175; ry: 601.2065
            rect = new iTextSharp.text.Rectangle(29, 584, 126, 614);
            var connectionColumn = _parser.GetStringValueFromRegion(file, rect).Split('\n');
            _transferingData.ConnectionOne = _connectionInfoAssembler.FillConnectionInfo(connectionColumn);
            //Connection 1 Outer diameter
            //lx: 106.32; ly: 576.2465; rx: 122.6546; ry: 582.1265
            rect = new iTextSharp.text.Rectangle(109, 564, 130, 578);
            _transferingData.ConnectionOne.Od = _parser.GetStringValueFromRegion(file, rect);
            //Connection 1 Internal diameter
            //Internal diameter at lx: 29.64; ly: 561.4865; rx: 73.73412; ry: 567.3665
            //???
            rect = new iTextSharp.text.Rectangle(111, 548, 129, 562);
            _transferingData.ConnectionOne.Id = _parser.GetStringValueFromRegion(file, rect);

            //Connection 2 Type, TreadSize
            //Type at lx: 245.16; ly: 610.9265; rx: 258.2254; ry: 616.8065
            //PIN at lx: 320.04; ly: 610.9265; rx: 329.8302; ry: 616.8065
            //Treadsize at lx: 245.16; ly: 603.1265; rx: 270.9673; ry: 609.0065
            //6 5/8  at lx: 320.04; ly: 603.1265; rx: 334.7341; ry: 609.0065
            //Reg at lx: 335.04; ly: 603.1265; rx: 345.8239; ry: 609.0065
            //Condition at lx: 245.16; ly: 595.3265; rx: 269.9912; ry: 601.2065
            //NDF at lx: 320.04; ly: 595.3265; rx: 332.1175; ry: 601.2065
            rect = new iTextSharp.text.Rectangle(259, 584, 362, 614);
            connectionColumn = _parser.GetStringValueFromRegion(file, rect).Split('\n');
            _transferingData.ConnectionTwo = _connectionInfoAssembler.FillConnectionInfo(connectionColumn);
            //Connection 2 Outer diameter
            //lx: 328.92; ly: 576.2465; rx: 345.2546; ry: 582.1265
            rect = new iTextSharp.text.Rectangle(347, 564, 368, 578);
            _transferingData.ConnectionTwo.Od = _parser.GetStringValueFromRegion(file, rect);
            //Connection 2 Internal diameter
            //lx: 335.4; ly: 561.4865; rx: 338.6693; ry: 567.3665
            rect = new iTextSharp.text.Rectangle(347, 548, 368, 562);
            _transferingData.ConnectionTwo.Id = _parser.GetStringValueFromRegion(file, rect);

            //Version
            //lx: 164.04; ly: 30.8465; rx: 267.2399; ry: 36.7265 
            rect = new iTextSharp.text.Rectangle(174, 34, 284, 40);
            _transferingData.Version = VersionExtractor.GetVersion(_parser.GetStringValueFromRegion(file, rect));

            return _transferingData;
        }
    }
}