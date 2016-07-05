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
        }

        private void CheckForElectricy_Click(object sender, RoutedEventArgs e)
        {
            manager.UpdateData();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            manager.TestList();
        }
    }
}
