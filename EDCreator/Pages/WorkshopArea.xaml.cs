using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Win32;

namespace EDCreator.Pages
{
    /// <summary>
    /// Interaction logic for WorkshopArea.xaml
    /// </summary>
    public partial class WorkshopArea : Page
    {
        private OpenFileDialog _opener;
        public WorkshopArea()
        {
            InitializeComponent();
            //Настраиваем окно диалога выбора файлов
            _opener = new OpenFileDialog
            {
                Multiselect = true, //выбор нескольких файлов
                Filter = "PDF files (*.pdf)|*.pdf", //задаётся маска файла поумолчанию
                InitialDirectory = $"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\in" //Задаётся каталог, который будет
                //каталогом поумолчанию при показе диалога
            };
        }

        private void Choose_Click(object sender, RoutedEventArgs e)
        {
            //Показываем диалог выбора файлов
            _opener.ShowDialog();
        }

        private void Proceed_Click(object sender, RoutedEventArgs e)
        {
            //Если были выбраны файлы (или один файл) - здесь под Length понимается размер массива, который содержит имена выбранных файлов
            if (_opener.FileNames.Length != 0)
            {
                //вызов парсера и передача ему строк, содержащих путь к выбранным файлам
            }
        }
    }
}
