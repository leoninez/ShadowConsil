using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowConsil.src
{
    public class WowProcessManager
    {
        public ObservableCollection<ProcessInfo> ManagedProcessList { get; set; }
        public Dictionary<int, ProcessInfo> ManagedProcessMap { get; set; }

        public string TargetAppName { get; set; }
        public bool CaseSensive { get; set; }

        public WowProcessManager(string targetName, bool caseSensive = true)
        {
            this.ManagedProcessList = new ObservableCollection<ProcessInfo>();
            this.ManagedProcessMap = new Dictionary<int, ProcessInfo>();
            this.TargetAppName = caseSensive ? targetName : targetName.ToLower();
            this.CaseSensive = caseSensive;
        }

        public void RefreshAllProcessList()
        {
            this.ManagedProcessList.Clear();

            var processes = Process.GetProcesses();

            foreach (Process p in processes)
            {
                if (this.ManagedProcessMap.ContainsKey(p.Id))
                {
                    ManagedProcessList.Add(this.ManagedProcessMap[p.Id]);
                    continue;
                }

                string processName = CaseSensive ? p.ProcessName : p.ProcessName.ToLower();
                if (TargetAppName != null && processName != this.TargetAppName)
                {
                    continue;
                }

                var info = new ProcessInfo
                {
                    pid = p.Id,
                    name = p.ProcessName,
                    title = p.MainWindowTitle,
                    flag = ProcessInfo.ProcessFlag.NOT_RECEIVE_KEY,
                    exe_path = p.StartInfo.WorkingDirectory,

                    raw = p
                };

                info.sender = new KeyboardSender(info);

                ManagedProcessList.Add(info);
            }

            RefreshDic();
        }

        public void RefreshDic()
        {
            this.ManagedProcessMap.Clear();
            foreach (ProcessInfo info in this.ManagedProcessList)
            {
                this.ManagedProcessMap.Add(info.pid, info);
            }
        }
    }
}
