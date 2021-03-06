﻿using System;
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

namespace FDCreator.Pages
{
    /// <summary>
    /// Interaction logic for ToolsSelector.xaml
    /// </summary>
    public partial class ToolsSelector : Page
    {
        public ToolsSelector()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var navigationService = NavigationService.GetNavigationService(this);
            if (Convert.ToBoolean(RbDumbIron.IsChecked))
            {
                navigationService?.Navigate(new DumbIron());
            }

            if (Convert.ToBoolean(RbTelescope.IsChecked))
            {
                navigationService?.Navigate(new TeleScope());
            }

            if (Convert.ToBoolean(RbGdis.IsChecked))
            {
                navigationService?.Navigate(new Gdis());
            }

            if (Convert.ToBoolean(RbArc.IsChecked))
            {
                navigationService?.Navigate(new Arc());
            }
        }
    }
}
