using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EDCreator.Logic;
using EDCreator.Misc;
using Microsoft.Win32;

namespace EDCreator
{
    // В ЭТОМ ФАЙЛЕ МЕНЯТЬ ЧТО-ТО НА ВАШЕ УСМОТРЕНИЕ, ЗДЕСЬ ТОЛЬКО ОБРАБОТЧИКИ ИНТЕРФЕЙСА И ЗАПУСК ЛОГИКИ ПРИЛОЖЕНИЯ ЧЕРЕЗ
    // КЛИЕНТСКИЙ КЛАСС



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly OpenFileDialog _opener;
        private readonly List<string> _files;
        public MainWindow()
        {
            InitializeComponent();
            _opener = new OpenFileDialog
            {
                Multiselect = true, //выбор нескольких файлов
                Filter = "PDF files (*.pdf)|*.pdf", //задаётся маска файла поумолчанию
                InitialDirectory = $@"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\in" //Задаётся каталог, который будет
                //каталогом поумолчанию при показе диалога
            };

            _files = new List<string>();
        }
        private void Choose_Click(object sender, RoutedEventArgs e)
        {
            //Показываем диалог выбора файлов
            if (_opener.ShowDialog() != true) return;
            foreach (var fileName in _opener.FileNames)
            {
                _files.Add(fileName);
                FileList.Text += $"{System.IO.Path.GetFileName(fileName)}\n";
            }

            if (_files.Count != 0)
            {
                PathString.Text = System.IO.Path.GetDirectoryName(_files.Last());
            }
        }

        //Обработчик нажатия на кнопку [Обработать]
        private void Proceed_Click(object sender, RoutedEventArgs e)
        {
            //Если не выбран ни один файл, показываем сообщение и выходим из процедуры
            if (_files.Count == 0)
            {
                MessageBox.Show("Не выбрано ни одного файла.", "Предупреждение", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return;
            }

            //Если не вышли на предыдущем шаге, то заполняем данные из полей на форме для формирования заголовка
            var headerData = new HeaderData
            {
                ClientField = Client.Text,
                DateField = Date.Text,
                DdEngineerField = DdEngineer.Text,
                FieldPadWellField = FieldPadWell.Text,
                LocationField = Location.Text
            };

            //Экземпляр класса Client запускает всю логику приложения, дальнейшая работа проходит в нём, можно смело открывать
            //файл Client.cs

            var client = new Client {Header = headerData};

            foreach (var file in _files)
            {
                client.Run(file);
            }
            MessageBox.Show("Файлы обработаны", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void FileList_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var dropedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (dropedFiles == null) return;

            foreach (var file in  dropedFiles)
            {
                if ( CheckFileExtention(file))
                    FileList.Text += $"{System.IO.Path.GetFileName(file)}\n";
                _files.Add(file);
            }
        }

        private void FileList_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var dropedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (dropedFiles == null) return;
            foreach (var file in dropedFiles)
            {
                e.Effects = CheckFileExtention(file) ? DragDropEffects.Copy : DragDropEffects.None;
            }
        }

        private static bool CheckFileExtention(string file)
        {
            var ext = System.IO.Path.GetExtension(file);

            return ext != null && ext.ToUpper() == ".PDF";
        }

        void Container_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var tb = e.Source as TextBox;
            if (tb == null) return;
            switch (e.Key)
            {
                case Key.Enter:
                    tb.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    break;
                default:
                    break;
            }
        }
    }
}
