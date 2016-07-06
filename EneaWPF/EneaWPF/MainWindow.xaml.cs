using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


        bool isPhoneHandled = false;



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
                PlugInDevice.IsEnabled = false;
                PlugInDevice.Content = "Podłączono telefon";

                if (manager.PhoneNumber != string.Empty)
                    StartSMSSubscription.IsEnabled = true;

                isPhoneHandled = true;

            }
            else MessageBox.Show("Nie znaleziono urządzenia");

        }


        private void mainStackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            todayListBox.SelectedItem = null;
            tommorowListBox.SelectedItem = null;
            elseListBox.SelectedItem = null;
        }


        private void insertNumberBox_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (insertNumberBox.Text.Contains("Wpisz numer..."))
                insertNumberBox.Text = "";
        }

        private void insertEmailBox_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (insertEmailBox.Text.Contains("Wpisz adres e-mail..."))
                insertEmailBox.Text = "";
        }

        private void insertEmailBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex emailRegex = new Regex(@"^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$");
            if (emailRegex.IsMatch(insertEmailBox.Text))
            {
                ConfirmMailButton.IsEnabled = true;
                ConfirmMailButton.Content = "Zatwierdź";
                StartEmailSubscription.IsEnabled = false;
            }
            else if (ConfirmMailButton != null)
            {
                ConfirmMailButton.IsEnabled = false;
                ConfirmMailButton.Content = "Błędny adres e-mail!";
                StartEmailSubscription.IsEnabled = false;
            }

        }

        private void insertNumberBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex phoneRegex= new Regex(@"^[0-9]{9}$");
            if (phoneRegex.IsMatch(insertNumberBox.Text))
            {
                ConfirmPhoneNumberButton.IsEnabled = true;
                ConfirmPhoneNumberButton.Content = "Zatwierdź";
                StartSMSSubscription.IsEnabled = false;
            }
            else if (ConfirmPhoneNumberButton != null)
            {
                ConfirmPhoneNumberButton.IsEnabled = false;
                ConfirmPhoneNumberButton.Content = "Błędny numer telefonu!";
                StartSMSSubscription.IsEnabled = false;
            }

        }

        private void ConfirmPhoneNumberButton_Click(object sender, RoutedEventArgs e)
        {
            manager.PhoneNumber = insertNumberBox.Text;
            ConfirmPhoneNumberButton.IsEnabled = false;
            ConfirmPhoneNumberButton.Content = "Zatwierdzono";

            if (isPhoneHandled)
            {
                StartSMSSubscription.IsEnabled = true;
            }
        }

        private void ConfirmMailButton_Click(object sender, RoutedEventArgs e)
        {
            manager.EmailAddress = insertEmailBox.Text;
            ConfirmMailButton.IsEnabled = false;
            ConfirmMailButton.Content = "Zatwierdzono";
            StartEmailSubscription.IsEnabled = true;
        }



        private void StartSubscription_Click(object sender, RoutedEventArgs e)
        {
            SubscriptionIsOffTextBlock.Visibility = Visibility.Collapsed;
            SubscriptionIsOnTextBlock.Visibility = Visibility.Visible;
            StartSMSSubscription.Visibility = Visibility.Collapsed;
            EndSMSSubscription.Visibility = Visibility.Visible;
        }

        private void EndSubscription_Click(object sender, RoutedEventArgs e)
        {
            SubscriptionIsOnTextBlock.Visibility = Visibility.Collapsed;
            SubscriptionIsOffTextBlock.Visibility = Visibility.Visible;
            StartSMSSubscription.Visibility = Visibility.Visible;
            EndSMSSubscription.Visibility = Visibility.Collapsed;
        }

        private void StartEmailSubscription_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EndEmailSubscription_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
