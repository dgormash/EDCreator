using System;
using System.Runtime.InteropServices;
using FDCreator.Pages;

namespace FDCreator
{
    // В ЭТОМ ФАЙЛЕ МЕНЯТЬ ЧТО-ТО НА ВАШЕ УСМОТРЕНИЕ, ЗДЕСЬ ТОЛЬКО ОБРАБОТЧИКИ ИНТЕРФЕЙСА И ЗАПУСК ЛОГИКИ ПРИЛОЖЕНИЯ ЧЕРЕЗ
    // КЛИЕНТСКИЙ КЛАСС



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
       
    }
}
