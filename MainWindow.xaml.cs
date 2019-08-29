using ShadowConsil.src;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
namespace ShadowConsil
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal WowProcessManager manager = new WowProcessManager("Wow");
        internal bool enabled = false;
        internal List<KeyboardSender> aliveSenders = new List<KeyboardSender>();
        internal KeyboardHook globalKeyHook = null;

        public MainWindow()
        {
            InitializeComponent();
            processListView.ItemsSource = manager.ManagedProcessList;
        }

        List<System.Windows.Forms.Keys> ignoreKeys = new List<System.Windows.Forms.Keys>
        { System.Windows.Forms.Keys.A, System.Windows.Forms.Keys.S, System.Windows.Forms.Keys.D, System.Windows.Forms.Keys.W };

        private void Kh_KeyDown(System.Windows.Forms.Keys key, bool shift, bool ctrl, bool alt)
        {
            // filter some code
            int keyV = (int)key;

            if (((keyV >= 48 && keyV <= 90) || (keyV >= 112 && keyV <= 117)) && !ignoreKeys.Contains(key))
            {
                Debug.WriteLine($"{key}, Shift = {shift}, Ctrl = {ctrl}, Alt = {alt}");
                debugTextBlock.Text = $"{key}, Shift = {shift}, Ctrl = {ctrl}, Alt = {alt}";
                foreach (KeyboardSender sender in aliveSenders)
                {
                    sender.KeyDown(key, shift, ctrl, alt);
                }
            }
            else
            {
                Debug.WriteLine($"Ignore {key}, Shift = {shift}, Ctrl = {ctrl}, Alt = {alt}");
                debugTextBlock.Text = $"Ignore {key}, Shift = {shift}, Ctrl = {ctrl}, Alt = {alt}";
            }                Debug.WriteLine($"{key}, Shift = {shift}, Ctrl = {ctrl}, Alt = {alt}");
                debugTextBlock.Text = $"{key}, Shift = {shift}, Ctrl = {ctrl}, Alt = {alt}";
        }

        private void Kh_KeyUp(System.Windows.Forms.Keys key, bool shift, bool ctrl, bool alt)
        {
            // filter some code
            int keyV = (int)key;

            if (((keyV >= 48 && keyV <= 90) || (keyV >= 112 && keyV <= 117)) && !ignoreKeys.Contains(key))
            {
                Debug.WriteLine($"{key}, Shift = {shift}, Ctrl = {ctrl}, Alt = {alt}");
                debugTextBlock.Text = $"{key}, Shift = {shift}, Ctrl = {ctrl}, Alt = {alt}";
                foreach (KeyboardSender sender in aliveSenders)
                {
                    sender.KeyUp(key, shift, ctrl, alt);
                }
            }
            else
            {
                Debug.WriteLine($"Ignore {key}, Shift = {shift}, Ctrl = {ctrl}, Alt = {alt}");
                debugTextBlock.Text = $"Ignore {key}, Shift = {shift}, Ctrl = {ctrl}, Alt = {alt}";
            }
            Debug.WriteLine($"{key}, Shift = {shift}, Ctrl = {ctrl}, Alt = {alt}");
            debugTextBlock.Text = $"{key}, Shift = {shift}, Ctrl = {ctrl}, Alt = {alt}";
        }

        private void Refresh_WOW_Process(object sender, RoutedEventArgs e)
        {
            manager.RefreshAllProcessList();
        }

        private void Enable_Key_Receive(object sender, RoutedEventArgs e)
        {
            if (enabled)  // try to disable receive key
            {
                enabled = false;
                actionButton.Content = "Enable Receive Key";
                actionButton.Background = Brushes.Lime;

                this.aliveSenders.Clear();
                globalKeyHook.Dispose();
                globalKeyHook = null;

                debugTextBlock.Text = "Disabled Receive Key";
            }
            else // try to enable receive key
            {
                enabled = true;
                actionButton.Content = "Disable Receive Key";
                actionButton.Background = Brushes.Red;

                foreach (ProcessInfo info in manager.ManagedProcessList)
                {
                    if (info.flag == ProcessInfo.ProcessFlag.RECEIVE_KEY)
                    {
                        this.aliveSenders.Add(info.sender);
                    }
                }

                globalKeyHook = new KeyboardHook(true);
                globalKeyHook.KeyDown += Kh_KeyDown;
                globalKeyHook.KeyUp += Kh_KeyUp;

                debugTextBlock.Text = "Enable Receive Key";
            }
        }

        private void Set_Receive_Key(object sender, RoutedEventArgs e)
        {
            ProcessInfo info = (ProcessInfo)processListView.SelectedItem;

            if (info != null)
            {
                info.flag = ProcessInfo.ProcessFlag.RECEIVE_KEY;
            }

            manager.RefreshAllProcessList();
        }

        private void Unset_Receive_Key(object sender, RoutedEventArgs e)
        {
            ProcessInfo info = (ProcessInfo)processListView.SelectedItem;

            if (info != null)
            {
                info.flag = ProcessInfo.ProcessFlag.NOT_RECEIVE_KEY;
            }

            manager.RefreshAllProcessList();
        }

        private void Test_Receive_Key(object sender, RoutedEventArgs e)
        {
            ProcessInfo info = (ProcessInfo)processListView.SelectedItem;

            if (info != null)
            {
                info.sender.KeyDown(System.Windows.Forms.Keys.A);
            }
        }


    }
}
