using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using FDCreator.Logic;
using FDCreator.Logic.Common;
using FDCreator.Logic.RunableClients;
using FDCreator.Misc;
using Microsoft.Win32;

namespace FDCreator.Pages
{
    /// <summary>
    /// Interaction logic for Arc.xaml
    /// </summary>
    public partial class Arc : Page
    {
        private readonly OpenFileDialog _opener;
        private string[] _top,  _bottom;
        public Arc()
        {
            InitializeComponent();
            _opener = new OpenFileDialog
            {
                Multiselect = false, //выбор нескольких файлов
                Filter = "PDF files (*.pdf)|*.pdf", //задаётся маска файла поумолчанию
                InitialDirectory = $@"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\in"
                //Задаётся каталог, который будет
                //каталогом поумолчанию при показе диалога
            };
            _top = new string[1];
            _bottom = new string[1];
        }


        private void Top_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            _top = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (_top == null) return;

            if (CheckFileExtention(_top[0]))
            {
                Top.Text = $"{System.IO.Path.GetFileName(_top[0])}";
                Top.FontSize = 14;
                Top.Foreground = Brushes.MediumTurquoise;
            }


        }

       
        private void Bottom_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            _bottom = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (_bottom == null) return;


            if (CheckFileExtention(_bottom[0]))
            {
                Bottom.Text = $"{System.IO.Path.GetFileName(_bottom[0])}";
                Top.FontSize = 14;
                Top.Foreground = Brushes.MediumTurquoise;
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
            var bc = new BrushConverter();
            Top.Text = "ARC Collar";
            Top.FontSize = 24;
            Top.Foreground = (Brush)bc.ConvertFrom("#FFB3D0C7");

            Bottom.Text = "Saver Sub Bot";
            Bottom.FontSize = 24;
            Bottom.Foreground = (Brush)bc.ConvertFrom("#FFB3D0C7");

            _top[0] = string.Empty;
            _bottom[0] = string.Empty;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var navigationService = NavigationService.GetNavigationService(this);
            navigationService?.Navigate(new ToolsSelector());
        }

        private void ChooseTop_Click(object sender, RoutedEventArgs e)
        {
            if (_opener.ShowDialog() != true) return;

            Top.Text = System.IO.Path.GetFileName(_opener.FileName);
            _top[0] = _opener.FileName;
            Top.FontSize = 14;
            Top.Foreground = Brushes.MediumTurquoise;
        }

        private void ChooseBottom_Click(object sender, RoutedEventArgs e)
        {
            if (_opener.ShowDialog() != true) return;

            Bottom.Text = System.IO.Path.GetFileName(_opener.FileName);
            _bottom[0] = _opener.FileName;
            Bottom.FontSize = 14;
            Bottom.Foreground = Brushes.MediumTurquoise;
        }
        private void Proceed_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_top[0]) || string.IsNullOrWhiteSpace(_bottom[0]))
            {
                MessageBox.Show("Please, add all parts of the Telescope tool", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

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
                    "Bottom", new PartFile
                    {
                        File = _bottom[0],
                        Type = SmartToolPartType.Bottom
                    }
                }
            };



            var client = new SmartToolClient(partsFiles, SmartToolType.Arc);
            client.Run();

            var files = Directory.GetFiles($@"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\work", "*.xlsx");

            if (files.Length != 0)
            {
                XlsxCombiner.SessionStartTime = ApplicationPropetries.GetApplicationSessionStratTime();
                XlsxCombiner.CombineXlsxFilesFromWorkDir(files);
                MessageBox.Show("Task completed", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                XlsxTotalFishingDiagramOpener.ShowTotalDiagram(XlsxCombiner.CombinedFile);
            }
            else
            {
                MessageBox.Show("Finished. No files were processed", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

    }
}
