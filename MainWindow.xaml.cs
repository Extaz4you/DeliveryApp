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
        // nswag openapi2csclient /input:http://localhost:3333/swagger/v1/swagger.json /output:DeliveryApiService.cs /namespace:DelAPI
        // docker-compose up -d --build

        private readonly HttpClient _httpClient;
        private readonly string _baseAddress = "http://localhost:3333";
        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            Load();
        }
        private void Load()
        {
            Deliveries.Items.Clear();
            var cl = new Client(_baseAddress, _httpClient);
            var list = cl.ShowAllDeliveriesAsync().Result;
            foreach (var item in list)
            {
                Deliveries.Items.Add(item);
            }
            Deliveries.Items.Refresh();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addView = new Add();
            if (addView.ShowDialog() == true)
            {
                var newDelivery = addView.NewDelivery;
                if (newDelivery != null)
                {
                    var cl = new Client(_baseAddress, _httpClient);
                    cl.AddNewDeliveryAsync(newDelivery);
                }
                MessageBox.Show("Данные успешно добавлены");
            }
            Load();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDelivery = Deliveries.SelectedItem as Delivery;
            if (selectedDelivery.Status == "Новая")
            {
                MessageBox.Show("На данной стадии изменить нельзя");
                return;
            }
            if (selectedDelivery == null)
            {
                MessageBox.Show("Пожалуйста, выберите строку для изменения.");
                return;
            }

            var editView = new Edit(selectedDelivery);
            if (editView.ShowDialog() == true)
            {
                var changesDelivery = editView.Delivery;
                var cl = new Client(_baseAddress, _httpClient);
                MessageBox.Show(changesDelivery.ClientName);
                cl.ChangeDeliveryAsync(changesDelivery);
                MessageBox.Show("Данные успешно изменены.");
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
                Deliveries.Items.Remove(selectedDelivery);
                Deliveries.Items.Refresh();
                var cl = new Client(_baseAddress, _httpClient);
                cl.RemoveNewDeliveryAsync(selectedDelivery.Id);
                MessageBox.Show("Удалено");
            }
            else
            {
                return;
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
            var cl = new Client(_baseAddress, _httpClient);
            var list = cl.NextStageAsync(selectedDelivery.Id);
            MessageBox.Show("Отправлено на следующую стадию");
            Load();
        }

        private void SerchByText_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (string.IsNullOrEmpty(SerchByText.Text)) Load();
            else
            {
            }
        }
    }
}
