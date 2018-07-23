using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using FDCreator.Logic;
using FDCreator.Misc;
using Microsoft.Win32;

namespace FDCreator.Pages
{
    /// <summary>
    /// Interaction logic for DumbIron.xaml
    /// </summary>
    public partial class DumbIron : Page
    {
        private readonly OpenFileDialog _opener;
        private readonly List<string> _files;
        public DumbIron()
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
                MessageBox.Show("No files have been selected", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return;
            }

            Proceed.IsEnabled = false;
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

            var client = new Client { Header = headerData, SessionStartTime  = ApplicationPropetries.GetApplicationSessionStratTime()};
            foreach (var file in _files)
            {
               client.Run(file);
            }

            var files = Directory.GetFiles($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\work", "*.xlsx");

            if (files.Length != 0)
            {
                XlsxCombiner.SessionStartTime = ApplicationPropetries.GetApplicationSessionStratTime();
                XlsxCombiner.CombineXlsxFilesFromWorkDir(files);
                MessageBox.Show("Task completed", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                XlsxTotalFishingDiagramOpener.ShowTotalDiagram(ApplicationPropetries.GetTotalFishingDiagramPath());
            }
            else
            {
                MessageBox.Show("Finished. No files were processed", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            
            Proceed.IsEnabled = true;
            FileList.Clear();
            _files.Clear();

            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        private void FileList_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var dropedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (dropedFiles == null) return;

            foreach (var file in dropedFiles)
            {
                if (CheckFileExtention(file))
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

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Client.Text = string.Empty;
            FieldPadWell.Text = string.Empty;
            Location.Text = string.Empty;
            DdEngineer.Text = string.Empty;
            Date.Text = string.Empty;
            PathString.Text = string.Empty;
            FileList.Text = string.Empty;
            _files.Clear();
            PathString.Text = "[empty...]";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var navigationService = NavigationService.GetNavigationService(this);
            navigationService?.Navigate(new ToolsSelector());
        }
    }
}
