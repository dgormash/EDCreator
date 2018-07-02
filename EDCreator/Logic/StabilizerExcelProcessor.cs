using System;
using System.IO;
using System.Reflection;
using EDCreator.Misc;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace EDCreator.Logic
{
    public class StabilizerExcelProcessor : ExcelProcessor
    {

        private HSSFWorkbook _xlsBook;
        public StabilizerExcelProcessor()
        {
            TemplateFileName = "Stabilizer Diagram.xls";
        }

        //В шаблоне Stabilizer Diagram.xlsx прям совсем другая структура, поэтому методе PassDataToExcel необходимо переопределить

        public override void PassDataToExcel(ParsedData data)
        {
            var stabilizerData = (StabilizerParsedData) data;

            if (string.IsNullOrEmpty(TemplateFileName)) return;

            var filePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\misc\{TemplateFileName}";

            using (
                var file =
                    new FileStream(filePath,
                        FileMode.Open, FileAccess.Read))
            {
                //Book = new XSSFWorkbook(file);
                _xlsBook = new HSSFWorkbook(file);
            }

            Sheet = _xlsBook.GetSheetAt(0);

            //Запись заголовка
            FillHeader(stabilizerData);

            //cellNum - Номер ячейки (в контексте таблицы - столбца), в которую вставляются данные, соответстует столбцу G в шаблоне
            var cellNum = 6;

            //SerialNumber
            SetCellValue(14, cellNum, stabilizerData.SerialNumber);
            //TOP
            SetCellValue(17, cellNum, stabilizerData.ConnectionOne.TreadSize);
            //BOT
            SetCellValue(18, cellNum, stabilizerData.ConnectionTwo.TreadSize);
            //L
            SetCellValue(22, cellNum, stabilizerData.Length);
            //L1
            SetCellValue(23, cellNum, stabilizerData.FishingNeckTongSpace);
            //OD
            SetCellValue(29, cellNum, stabilizerData.ConnectionOne.Od);
            //ID
            SetCellValue(30, cellNum, stabilizerData.ConnectionTwo.Id);
            //MaxOD
            SetCellValue(31, cellNum, stabilizerData.StabilizerOd);
            //BladeLength
            SetCellValue(32, cellNum, stabilizerData.LobeLength);
            //BladeWidth
            SetCellValue(34, cellNum, stabilizerData.LobeWidth);

            var fileName = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\out\{
                stabilizerData.Name}_{stabilizerData.SerialNumber}_FinishedDiagram.xls";
            //Сохранение изменённого файла
            using (
                var file =
                    new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                _xlsBook.Write(file);
            }

            OpenExcelApp(fileName);

            //var zipName = Path.ChangeExtension(name, ".zip");
            //File.Move(name, Path.ChangeExtension(name, ".zip"));
            //ExtractZipFile(zipName, null, $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\out\");
            //PackFile(zipName);
        }

        private void PackFile(string file)
        {
            using (var archiveStream = File.Create($@"{Path.GetFileNameWithoutExtension(file)}.zip"))
            {
                ZipOutputStream zipStream = new ZipOutputStream(archiveStream);
                zipStream.SetLevel(3);
                byte[] buffer = new byte[4096];
                var entry = new ZipEntry(Path.GetFileName(file));
                zipStream.PutNextEntry(entry);
                using (var reader = File.OpenRead(file))
                {
                    StreamUtils.Copy(reader, zipStream, buffer);
                }

                zipStream.CloseEntry();
                zipStream.Close();
                File.Delete(file);
            }

         
        }
        public void ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    zf.Password = password;     // AES encrypted entries are handled automatically
                }
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }
    }
}