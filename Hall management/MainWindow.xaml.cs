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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Hall_mngmt01Entities db = new Hall_mngmt01Entities();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Test ver. 00.01", "FAQ", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginBox.Text;
            string password = PassBox.Password;

            // Проверка аутентификации пользователя
            if (db.AuthenticateUser(username, password))
            {
                // Пользователь успешно аутентифицирован, выполняем нужные действия
                MessageBox.Show("Вы успешно вошли в систему!");
                // Создаем новое окно A1_Scenario и открываем его
                A1_Scenario a1_Scenario = new A1_Scenario();
                a1_Scenario.Show();

                // Закрываем текущее окно
                this.Close();
            }
            else
            {
                // Пользователь не аутентифицирован, выводим сообщение об ошибке
                MessageBox.Show("Ошибка аутентификации. Проверьте свои учетные данные.");
            }
        }
    }
}
