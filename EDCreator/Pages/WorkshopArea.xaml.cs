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
        public WorkshopArea()
        {
            InitializeComponent();
        }

        private void Choose_Click(object sender, RoutedEventArgs e)
        {
            var openFileDlg = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "PDF files (*.pdf)|*.pdf",
                InitialDirectory = $"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\in"
            };

            if (openFileDlg.ShowDialog() == true)
            {
                var pdfReader = new PdfReader(openFileDlg.FileName);
                var sb = new StringBuilder();
                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    sb.Append(PdfTextExtractor.GetTextFromPage(pdfReader, i));
                }
                FileList.Text = sb.ToString();
                pdfReader.Close();
            }
        }
    }
}
