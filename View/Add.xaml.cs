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
using System.Windows.Shapes;
using DelAPI;
namespace DeliveryApp.View
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        private Delivery _delivery;
        public Delivery NewDelivery { get; set; }
        public Action DeliveryAdded;
        public Add()
        {
            InitializeComponent();
            Load();
        }
        private void Load()
        {
            DeliveryTime.SelectedDate = DateTime.Now;
            Status.SelectedIndex = 0;
            Status.IsEnabled = false;
            CancellationReason.IsEnabled = false;
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            var clientName = ClientName.Text.Trim();
            if (string.IsNullOrEmpty(clientName))
            {
                MessageBox.Show("Имя клиента не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var delivery = new Delivery()
            {
                ClientName = ClientName.Text,
                CargoDescription = CargoDescription.Text,
                DeliveryAddress = DeliveryAddress.Text,
                DeliveryTime = DeliveryTime.SelectedDate ?? DateTime.Now,
                Status = Status.Text,
                CancellationReason = CancellationReason.Text
            };

            NewDelivery = delivery;
            DeliveryAdded?.Invoke();

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
