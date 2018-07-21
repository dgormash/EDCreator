using System.Windows;
using System.Windows.Input;

namespace FDCreator.Logic
{
    public static class ApplicationPropetries
    {
        public static string GetApplicationSessionStratTime()
        {
            var mw = (MainWindow)Application.Current.MainWindow;
            return mw?.SessionStartTime;
        }
    }
}