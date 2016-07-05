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

namespace EneaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        AppManager manager;
        public MainWindow()
        {
            InitializeComponent();
            manager = new AppManager();
            grid.DataContext = manager;
            manager.UpdateData();
        }

        private void CheckForElectricy_Click(object sender, RoutedEventArgs e)
        {

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


    }
}
