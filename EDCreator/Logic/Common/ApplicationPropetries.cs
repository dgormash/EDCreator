using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace FDCreator.Logic
{
    public static class ApplicationPropetries
    {
        public static string GetApplicationSessionStratTime()
        {
            var mw = GetMainWindow();
            return mw?.SessionStartTime;
        }

        public static string GetTotalFishingDiagramPath()
        {
            return $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                        }\out\Field_Pad_Well_FishingDiagram_{
                        GetMainWindow().SessionStartTime}.xlsx";
        }

        private static MainWindow GetMainWindow()
        {
            return (MainWindow)Application.Current.MainWindow;
        }
    }
}