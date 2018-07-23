using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace FDCreator.Logic
{
    public static class XlsxCombiner
    {
        private static readonly Application Excel;
        public static string SessionStartTime { get; set; }

       static XlsxCombiner()
        {
            Excel = new Application
            {
                Visible = false,
                DisplayAlerts = false
            };
            
        }

        public static void CombineXlsxFilesFromWorkDir(string [] files)
        {
            try
            {
                foreach (var file in files)
                {
                    Excel.Workbooks.Add(file);
                }

                for (var i = 2; i <= Excel.Workbooks.Count; i++)
                {
                    for (var j = 1; j <= Excel.Workbooks[i].Worksheets.Count; j++)
                    {
                        Worksheet ws = Excel.Workbooks[i].Worksheets[j];
                        
                        ws.Copy(Excel.Workbooks[1].Worksheets[1]);
                    }
                }

                Excel.Workbooks[1].SaveCopyAs(
                    $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                        }\out\Field_Pad_Well_FishingDiagram_{
                        SessionStartTime}.xlsx");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error opening excel application: {e.Message}", "I have a bad feeling about this",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Excel.Quit();
            }

            Excel.Quit();
        }
    }
}