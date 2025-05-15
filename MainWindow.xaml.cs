using DeliveryApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using DelAPI;
using System.Net.Http;
namespace DeliveryApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress = "http://localhost:3333";
        private Client cl;

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            cl = new Client(_baseAddress, _httpClient);
            Load();
        }

        private void Load()
        {
            try
            {
                Deliveries.Items.Clear();
                var list = cl.ShowAllDeliveriesAsync().Result;
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        Deliveries.Items.Add(item);
                    }
                    Deliveries.Items.Refresh();
                }
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Соединение не установлено. Проверьте подключение к серверу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addView = new Add();
            if (addView.ShowDialog() == true)
            {
                var newDelivery = addView.NewDelivery;
                if (newDelivery != null)
                {
                    try
                    {
                        cl.AddNewDeliveryAsync(newDelivery);
                        MessageBox.Show("Данные успешно добавлены");
                    }
                    catch (HttpRequestException)
                    {
                        MessageBox.Show("Соединение не установлено. Проверьте подключение к серверу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        Application.Current.Shutdown();
                    }
                }
                Load();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDelivery = Deliveries.SelectedItem as Delivery;
            if (selectedDelivery == null)
            {
                MessageBox.Show("Пожалуйста, выберите строку для изменения.");
                return;
            }
            if (selectedDelivery.Status == "Новая")
            {
                MessageBox.Show("На данной стадии изменить нельзя");
                return;
            }

            var editView = new Edit(selectedDelivery);
            if (editView.ShowDialog() == true)
            {
                var changesDelivery = editView.Delivery;
                try
                {
                    cl.ChangeDeliveryAsync(changesDelivery);
                    MessageBox.Show("Данные успешно изменены.");
                }
                catch (HttpRequestException)
                {
                    MessageBox.Show("Соединение не установлено. Проверьте подключение к серверу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
                Load();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDelivery = Deliveries.SelectedItem as Delivery;
            if (selectedDelivery == null)
            {
                MessageBox.Show("Пожалуйста, выберите строку для удаления.");
                return;
            }
            MessageBoxResult answer = MessageBox.Show(@"Вы точно хотите удалить эту запись?",
                                            "Подтверждение удаления",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                try
                {
                    Deliveries.Items.Remove(selectedDelivery);
                    Deliveries.Items.Refresh();
                    cl.RemoveNewDeliveryAsync(selectedDelivery.Id);
                    MessageBox.Show("Удалено");
                }
                catch (HttpRequestException)
                {
                    MessageBox.Show("Соединение не установлено. Проверьте подключение к серверу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDelivery = Deliveries.SelectedItem as Delivery;
            if (selectedDelivery == null)
            {
                MessageBox.Show("Пожалуйста, выберите строку для изменения.");
                return;
            }
            try
            {
                cl.NextStageAsync(selectedDelivery.Id);
                MessageBox.Show("Отправлено на следующую стадию");
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Соединение не установлено. Проверьте подключение к серверу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            Load();
        }

        private void SerchByText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(SerchByText.Text))
            {
                Load();
            }
            else
            {
                try
                {
                    var list = cl.GetDeliveryByTextAsync(SerchByText.Text).Result;
                    if (list.Any())
                    {
                        Deliveries.Items.Clear();
                        foreach (var item in list)
                        {
                            Deliveries.Items.Add(item);
                        }
                        Deliveries.Items.Refresh();
                    }
                }
                catch (HttpRequestException)
                {
                    MessageBox.Show("Соединение не установлено. Проверьте подключение к серверу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown(); 
                }
            }
        }
    }
}
