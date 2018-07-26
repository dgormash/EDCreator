using System;
using System.IO;
using System.Windows;
using ExcelApp = Microsoft.Office.Interop.Excel;
namespace FDCreator.Logic
{
    public static class XlsxTotalFishingDiagramOpener
    {
        public static void ShowTotalDiagram(string file)
        {
            var excelApp = new ExcelApp.Application { Visible = true };
            try
                {
                    excelApp.Workbooks.Open(file);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Error opening excel application: {e.Message}; tried to open {Path.GetFileName(file)}", "I have a bad feeling about this",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    excelApp.Quit();
                }
                finally
                {
                    excelApp = null;
                }
            }
          
    }
}