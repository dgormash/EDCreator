using System;
using System.Collections.Generic;
using System.Linq;
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
using EDCreator.Misc;

namespace EDCreator.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var loginData = new LoginData
            {
                ClientField = Client.Text,
                DateField = Date.Text,
                DdEngineerField = DdEngineer.Text,
                FieldPadWellField = FieldPadWell.Text,
                LocationField = Location.Text
            };

            this.NavigationService.Navigate(new WorkshopArea());
        }
    }
}
