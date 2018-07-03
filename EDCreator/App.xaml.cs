using System.Windows;
using System.Windows.Controls;

namespace EDCreator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    //Код позволяет: а) выделять весь текст в текстовом поле при переходе по Tab; б) позволяет выделеть весь текст в текстовом поле при 
    //клике мышкой
    //http://madprops.org/blog/wpf-textbox-selectall-on-focus/
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            EventManager.RegisterClassHandler(typeof (TextBox),
                UIElement.GotFocusEvent,
                new RoutedEventHandler(TextBox_GotFocus));

            EventManager.RegisterClassHandler(typeof (TextBox),
                UIElement.PreviewMouseDownEvent,
                new RoutedEventHandler(TextBox_PreviewMouseDown));
            base.OnStartup(e);
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox?.SelectAll();
        }

        private static void TextBox_PreviewMouseDown(object sender, RoutedEventArgs e)

        {

            var textBox = sender as TextBox;

            if (textBox == null || textBox.IsFocused) return;
            textBox.Focus();

            textBox.SelectAll();

            e.Handled = true;
        }
    }
}
