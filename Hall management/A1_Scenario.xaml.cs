using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
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
        private TcpClient recorderClient;
        private NetworkStream recorderStream;
        private bool isRecording;
        

        public A1_Scenario()
        {
            InitializeComponent();
            ConnectToRecorder();
            

        }

        private async Task SendTelnetCommandAsync(string command)
        {
            try
            {
                using (TcpClient client = new TcpClient("192.168.110.214", 23))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.ASCII.GetBytes(command);
                    await stream.WriteAsync(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке команды по Telnet: {ex.Message}");
            }
        }
        private async void ConnectToRecorder()
        {
            try
            {
                recorderClient = new TcpClient();
                await recorderClient.ConnectAsync("192.168.110.215", 23);
                recorderStream = recorderClient.GetStream();
                StartReading();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подключении к рекордеру: {ex.Message}");
            }
        }

        

        

     
        private async void StartReading()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = await recorderStream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                ProcessRecorderResponse(response);
            }
        }
        private void ProcessRecorderResponse(string response)
        {
            // Обрабатываем полученный ответ от рекордера
            if (response == "1")
            {
                isRecording = true;
                Dispatcher.Invoke(() => InfoButton.Background = Brushes.Red); // Окраска кнопки в красный цвет
                MessageBox.Show("Запись запущена");
            }
            else if (response == "0")
            {
                isRecording = false;
                Dispatcher.Invoke(() => InfoButton.Background = Brushes.Gray); // Окраска кнопки в серый цвет
                MessageBox.Show("Запись остановлена");
            }
        }
        private async Task SendRecorderCommandAsync(string command)
        {
            try
            {
                // Открываем Telnet-соединение
                using (var client = new TcpClient("192.168.110.215", 23))
                using (var stream = client.GetStream())
                {
                    // Отправляем команду
                    byte[] data = Encoding.ASCII.GetBytes(command + "\r\n");
                    await stream.WriteAsync(data, 0, data.Length);

                    // Прочитываем ответ
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    // Обрабатываем ответ
                    ProcessRecorderResponse(response);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке команды на рекордер: {ex.Message}");
            }
        }
        

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (recorderStream != null)
            {
                await SendRecorderCommandAsync("WY1RCDR");
                InfoButton.Background = Brushes.Red;
            }
        }
        
        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (recorderStream != null)
            {
                await SendRecorderCommandAsync("WY0RCDR");
                InfoButton.Background = Brushes.Gray;
            }
        }

        private async void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (recorderStream != null)
            {
                await SendRecorderCommandAsync("WYRCDR");
            }
        }

        private void Micbtn_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://192.168.110.225/chairman/microphonecontrol.cgi#";
            Process.Start(url);
        }

        private async void Prez_button_Click(object sender, RoutedEventArgs e)
        {
            await SendTelnetCommandAsync("2*3%");
        }

        private async void VCS_button_Click(object sender, RoutedEventArgs e)
        {
            await SendTelnetCommandAsync("5*3%");
        }

        private async void Oper_button_Click(object sender, RoutedEventArgs e)
        {
            await SendTelnetCommandAsync("10*3%");
        }
    }
}

    

