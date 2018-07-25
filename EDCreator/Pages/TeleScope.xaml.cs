using System;
using System.Collections.Generic;
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
using FDCreator.Logic;
using FDCreator.Logic.SmartTools;
using Microsoft.Win32;

namespace FDCreator.Pages
{
    /// <summary>
    /// Interaction logic for TeleScope.xaml
    /// </summary>
    public partial class TeleScope : Page
    {
        private readonly OpenFileDialog _opener;
        private readonly List<string> _files;
        private string[] _top, _middle, _bottom;

        public TeleScope()
        {
            InitializeComponent();
            _opener = new OpenFileDialog
            {
                Multiselect = true, //выбор нескольких файлов
                Filter = "PDF files (*.pdf)|*.pdf", //задаётся маска файла поумолчанию
                InitialDirectory = $@"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\in"
                //Задаётся каталог, который будет
                //каталогом поумолчанию при показе диалога
            };

            _files = new List<string>();
        }

        private void Top_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            _top = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (_top == null) return;

            if (CheckFileExtention(_top[0]))
                    Top.Text = _top[0];
            
        }

        private void Middle_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            _middle = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (_middle == null) return;

           
                if (CheckFileExtention(_middle[0]))
                    Middle.Text = _middle[0];
        }

        private void Bottom_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            _bottom = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (_bottom == null) return;

            
                if (CheckFileExtention(_bottom[0]))
                    Bottom.Text = _bottom[0];
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
            //Client.Text = string.Empty;
            //FieldPadWell.Text = string.Empty;
            //Location.Text = string.Empty;
            //DdEngineer.Text = string.Empty;
            //Date.Text = string.Empty;
            //PathString.Text = string.Empty;
            //FileList.Text = string.Empty;
            //_files.Clear();
            //PathString.Text = "[empty...]";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var navigationService = NavigationService.GetNavigationService(this);
            navigationService?.Navigate(new ToolsSelector());
        }

        private void Proceed_Click(object sender, RoutedEventArgs e)
        {
            var partsFiles = new Dictionary<string, PartFile>
            {
                {
                    "Top", new PartFile
                    {
                        File = _top[0],
                        Type = SmartToolPartType.Top
                    }
                },
                {
                    "Middle", new PartFile
                    {
                        File = _middle[0],
                        Type = SmartToolPartType.Top
                    }
                },
                {
                    "Bottom", new PartFile
                    {
                        File = _bottom[0],
                        Type = SmartToolPartType.Top
                    }
                }
            };



            var client = new SmartToolClient(partsFiles, SmartToolType.Telescope);
            client.Run();
        }

        private void Choose_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
