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
        internal ProcessManager manager = new ProcessManager();
        ProcessInfo selected = null;

        public MainWindow()
        {
            InitializeComponent();
            processListView.ItemsSource = manager.AllProcessList;
            selectedListView.ItemsSource = manager.ManagedProcessList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Show_All_Process(object sender, RoutedEventArgs e)
        {

        }

        private void Refresh_All_Process_List(object sender, RoutedEventArgs e)
        {
            manager.RefreshAllProcessList();
        }

        private void Show_WOW_Process(object sender, RoutedEventArgs e)
        {
    
        }

        private void processListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var lv = (ListView)sender;
            if (lv.SelectedItem != null)
            {
                this.selected = (ProcessInfo)lv.SelectedItem;
            }
            else this.selected = null;
        }

        private void selectedView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = (ListView)sender;
            if (lv.SelectedItem != null)
            {
                this.selected = (ProcessInfo)lv.SelectedItem;
            }
            else this.selected = null;
        }

        private void As_Master(object sender, RoutedEventArgs e)
        {
            if (this.selected != null)
            {
                this.manager.AsMaster(this.selected);
            }
        }

        private void As_Slave(object sender, RoutedEventArgs e)
        {
            if (this.selected != null)
            {
                this.manager.AsSlave(this.selected);
            }
        }

        private void To_Remove(object sender, RoutedEventArgs e)
        {
           
        }



        private ProcessInfo getListViewProcessInfo(object sender)
        {
            Console.WriteLine(sender.ToString());
            var lv = (ListView)sender;
            return (ProcessInfo)lv.SelectedItem;
        }
    }
}
