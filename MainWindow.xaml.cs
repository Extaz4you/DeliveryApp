using DeliveryApp.Models;
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

namespace DeliveryApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addView = new Add();
            if (addView.ShowDialog() == true)
            {
                var newDelivery = addView.NewDelivery;
                if (newDelivery != null)
                {
                    Deliveries.Items.Add(newDelivery);
                    Deliveries.Items.Refresh();
                }
                MessageBox.Show("Данные успешно добавлены");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDelivery = Deliveries.SelectedItem as Delivery;
            if(selectedDelivery.Status == "Новая")
            {
                MessageBox.Show("На данной стадии изменить нельзя");
                return;
            }
            if (selectedDelivery == null)
            {
                MessageBox.Show("Пожалуйста, выберите строку для изменения.");
                return;
            }

            var addView = new Edit(selectedDelivery);
            if (addView.ShowDialog() == true)
            {
                Deliveries.Items.Refresh();
                MessageBox.Show("Данные успешно изменены.");
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
                MessageBox.Show("Удалено");
            }
            else
            {
                return;
            }

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Поиск выполнен");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDelivery = Deliveries.SelectedItem as Delivery;
            if (selectedDelivery == null)
            {
                MessageBox.Show("Пожалуйста, выберите строку для изменения.");
                return;
            }
            selectedDelivery.Status = "Передано на выполнение";
            Deliveries.Items.Refresh();
            MessageBox.Show("Отправлено на следующую стадию");
        }
    }
}
