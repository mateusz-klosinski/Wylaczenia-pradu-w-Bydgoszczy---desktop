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
        public MainWindow()
        {
            InitializeComponent();
            manager = new AppManager();
            grid.DataContext = manager;
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (todayListBox.Items.Count == 0 || tommorowListBox.Items.Count == 0 || elseListBox.Items.Count == 0)
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

        private void todayListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            todayListBox.SelectedItem = null;
        }

        private void tommorowListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            tommorowListBox.SelectedItem = null;
        }

        private void elseListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            elseListBox.SelectedItem = null;
        }

        private void PlugInDevice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartSubscription_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
