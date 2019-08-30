using ShadowConsil.src;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
namespace ShadowConsil
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal WowProcessManager manager = new WowProcessManager("notepad");
        internal bool enabled = false;
        internal List<KeyboardSender> aliveSenders = new List<KeyboardSender>();
        internal KeyboardHook globalKeyHook = null;

        public MainWindow()
        {
            InitializeComponent();
            processListView.ItemsSource = manager.ManagedProcessList;
        }

        private readonly List<System.Windows.Forms.Keys> ignoreKeys = new List<System.Windows.Forms.Keys>
        {
            System.Windows.Forms.Keys.A, System.Windows.Forms.Keys.S, System.Windows.Forms.Keys.D, System.Windows.Forms.Keys.W
        };
        private readonly List<System.Windows.Forms.Keys> extraAcceptKey = new List<System.Windows.Forms.Keys>
        {
            System.Windows.Forms.Keys.LShiftKey, System.Windows.Forms.Keys.LControlKey, System.Windows.Forms.Keys.LMenu,
            System.Windows.Forms.Keys.RShiftKey, System.Windows.Forms.Keys.RControlKey, System.Windows.Forms.Keys.RMenu,
            System.Windows.Forms.Keys.Space
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsAcceptKey(System.Windows.Forms.Keys key)
        {
            // filter some code
            int keyV = (int)key;

            if (ignoreKeys.Contains(key)) { return false; }
            if ((keyV >= 48 && keyV <= 90) || (keyV >= 112 && keyV <= 117)) { return true; }
            if (extraAcceptKey.Contains(key)) { return true; }

            return false;
        }

        private void Kh_KeyDown(System.Windows.Forms.Keys key)
        {
            if (IsAcceptKey(key))
            {
                Debug.WriteLine($"Accept Down {key}");
                debugTextBlock.Text = $"Accept Down {key}";
                foreach (KeyboardSender sender in aliveSenders)
                {
                    sender.KeyDown(key);
                }
            }
            else
            {
                Debug.WriteLine($"Ignore Down {key}");
                debugTextBlock.Text = $"Ignore Down {key}";
            }
        }

        private void Kh_KeyUp(System.Windows.Forms.Keys key)
        {
            if (IsAcceptKey(key))
            {
                Debug.WriteLine($"Accept Up {key}");
                debugTextBlock.Text = $"Accept Up {key}";
                foreach (KeyboardSender sender in aliveSenders)
                {
                    sender.KeyUp(key);
                }
            }
            else
            {
                Debug.WriteLine($"Ignore Up {key}");
                debugTextBlock.Text = $"Ignore Up {key}";
            }
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
