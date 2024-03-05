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

namespace Hall_management
{
    /// <summary>
    /// Логика взаимодействия для A1_Scenario.xaml
    /// </summary>
    public partial class A1_Scenario : Window
    {
        public A1_Scenario()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Устанавливаем размер масштаба в процентах
            double scalePercentage = 100; 
            SetWebBrowserZoom(webBrowser1, scalePercentage);
            SetWebBrowserZoom(webBrowser2, scalePercentage);
        }

        private void SetWebBrowserZoom(WebBrowser webBrowser, double scalePercentage)
        {
            // Формируем скрипт для изменения масштаба страницы
            string script = $"document.body.style.zoom = '{scalePercentage}%';";

            // Выполняем скрипт внутри WebBrowser
            webBrowser.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }
    }
}
    

