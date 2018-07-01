﻿using System;
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
using EDCreator.Logic;
using EDCreator.Misc;
using EDCreator.Pages;
using Microsoft.Win32;

namespace EDCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly OpenFileDialog _opener;
        public MainWindow()
        {
            InitializeComponent();
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
            var headerData = new HeaderData
            {
                ClientField = Client.Text,
                DateField = Date.Text,
                DdEngineerField = DdEngineer.Text,
                FieldPadWellField = FieldPadWell.Text,
                LocationField = Location.Text
            };

            //Если были выбраны файлы (или один файл) - здесь под Length понимается размер массива, который содержит имена выбранных файлов
            if (_opener.FileNames.Length != 0)
            {
                foreach (var fileName in _opener.FileNames)
                {
                    var client = new Client(headerData);
                    client.Run(fileName);
                }
            }
            MessageBox.Show("Всё!!!");
        }
    }
}
