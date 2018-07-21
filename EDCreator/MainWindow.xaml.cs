using System;
using System.Runtime.InteropServices;
using FDCreator.Pages;

namespace FDCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public string SessionStartTime { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MainWindowFrame.Content = new ToolsSelector();
            SessionStartTime = DateTime.Now.ToString("yy-MM-dd-HH-mm-ss");
        }

        //todo delete *.xlsx from work directory after finish combining
    }
}
