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
    /// Логика взаимодействия для Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        private Delivery delivery;
        public Delivery Delivery { get; set; }
        public Edit(Delivery currentDelivery)
        {
            delivery = currentDelivery;
            InitializeComponent();
            LoadWindow(delivery);
        }
        private void LoadWindow(Delivery delivery)
        {
            ClientName.Text = delivery.ClientName;
            CargoDescription.Text = delivery.CargoDescription;
            DeliveryAddress.Text = delivery.DeliveryAddress;
            DeliveryTime.SelectedDate = DateTime.Parse(delivery.DeliveryTime.ToString());
            Status.Text = delivery.Status;
            CargoDescription.Text = delivery.CargoDescription;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            delivery.ClientName = ClientName.Text;
            delivery.CargoDescription = CargoDescription.Text;
            delivery.DeliveryAddress = DeliveryAddress.Text;
            delivery.DeliveryTime = DeliveryTime.SelectedDate ?? DateTime.Now;
            delivery.Status = Status.Text;
            var selectedItem = Status.SelectedItem as ComboBoxItem;

            if (selectedItem.Content.ToString() == "Отменена")
            {
                if (string.IsNullOrWhiteSpace(CancellationReason.Text))
                {
                    MessageBox.Show("Пожалуйста, укажите причину отмены.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; 
                }
            }
            Delivery = delivery;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Status_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            if(Status.SelectedItem is ComboBoxItem selectedItem)
            {
                if (selectedItem != null)
                {
                    if(selectedItem.Content.ToString() == "Отменена") CancellationReason.IsEnabled = true;
                    else CancellationReason.IsEnabled = false;
                }
            }
        }
    }
}
