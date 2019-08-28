using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowConsil.src
{
    public class ProcessManager
    {
        public ObservableCollection<ProcessInfo> AllProcessList { get; set; }
        public ObservableCollection<ProcessInfo> ManagedProcessList { get; set; }

        public ProcessManager()
        {
            this.AllProcessList = new ObservableCollection<ProcessInfo>();
            this.ManagedProcessList = new ObservableCollection<ProcessInfo>();
        }

        public void RefreshAllProcessList()
        {
            AllProcessList.Clear();
            var processes = Process.GetProcesses();

            foreach (Process p in processes)
            {
                var info = new ProcessInfo
                {
                    pid = p.Id,
                    name = p.ProcessName,
                    title = p.MainWindowTitle,
                    flag = ProcessInfo.ProcessFlag.NONE,
                    exe_path = p.StartInfo.WorkingDirectory,

                    raw = p
                };

                AllProcessList.Add(info);
            }
        }
    }
}
