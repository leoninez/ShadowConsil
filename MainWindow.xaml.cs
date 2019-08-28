using ShadowConsil.src;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ShadowConsil
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal ProcessManager _manager = new ProcessManager();

        public MainWindow()
        {
            InitializeComponent();
            processListView.ItemsSource = _manager.AllProcessList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Show_All_Process(object sender, RoutedEventArgs e)
        {

        }

        private void Refresh_All_Process_List(object sender, RoutedEventArgs e)
        {
            _manager.RefreshAllProcessList();
        }

        private void Show_WOW_Process(object sender, RoutedEventArgs e)
        {

        }
    }
}
