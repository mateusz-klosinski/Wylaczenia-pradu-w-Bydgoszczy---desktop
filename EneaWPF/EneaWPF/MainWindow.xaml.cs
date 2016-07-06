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
using System.Windows.Threading;

namespace EneaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        AppManager manager;
        HandlePhone phone;
        bool isSubscriptionTurnedOn = false;

        public MainWindow()
        {
            InitializeComponent();
            manager = new AppManager();
            phone = new HandlePhone();
            grid.DataContext = manager;

            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (todayListBox.Items.Count == 0 && tommorowListBox.Items.Count == 0 && elseListBox.Items.Count == 0)
            {
                downloadingTextBlock.Text += ".";
                if (downloadingTextBlock.Text.Length > 20)
                    downloadingTextBlock.Text = "Pobieranie danych.";
            }
            else
            {
                downloadingTextBlock.Visibility = Visibility.Collapsed;
                mainStackPanel.Visibility = Visibility.Visible;

                if (todayListBox.Items.Count > 0)
                    todayListBox.SelectedIndex = 0;
                else if (tommorowListBox.Items.Count > 0)
                    tommorowListBox.SelectedIndex = 0;
                else if (elseListBox.Items.Count > 0)
                    elseListBox.SelectedIndex = 0;

                timer.Stop();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            manager.TestList();
        }

        private void todayListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Disconnection disconnection = todayListBox.SelectedItem as Disconnection;
            if (disconnection != null)
                manager.updateDetails(disconnection);
        }

        private void tommorowListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Disconnection disconnection = tommorowListBox.SelectedItem as Disconnection;
            if (disconnection != null)
                manager.updateDetails(disconnection);
        }

        private void elseListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Disconnection disconnection = elseListBox.SelectedItem as Disconnection;
            if (disconnection != null)
                manager.updateDetails(disconnection);
        }



        private void PlugInDevice_Click(object sender, RoutedEventArgs e)
        {

            if (phone.COMCheck() == true)
            {
                MessageBox.Show("Znaleiono urządzenie");
                StartSubscription.IsEnabled = true;
                PlugInDevice.IsEnabled = false;
                PlugInDevice.Content = "Podłączono telefon";
            }
            else MessageBox.Show("Nie znaleziono urządzenia");

        }

        private void StartSubscription_Click(object sender, RoutedEventArgs e)
        {
            isSubscriptionTurnedOn = true;

            SubscriptionIsOffTextBlock.Visibility = Visibility.Collapsed;
            SubscriptionIsOnTextBlock.Visibility = Visibility.Visible;
            StartSubscription.Visibility = Visibility.Collapsed;
            EndSubscription.Visibility = Visibility.Visible;
        }

        private void EndSubscription_Click(object sender, RoutedEventArgs e)
        {
            isSubscriptionTurnedOn = false;

            SubscriptionIsOnTextBlock.Visibility = Visibility.Collapsed;
            SubscriptionIsOffTextBlock.Visibility = Visibility.Visible;
            StartSubscription.Visibility = Visibility.Visible;
            EndSubscription.Visibility = Visibility.Collapsed;
        }

        private void mainStackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            todayListBox.SelectedItem = null;
            tommorowListBox.SelectedItem = null;
            elseListBox.SelectedItem = null;
        }

    }
}
