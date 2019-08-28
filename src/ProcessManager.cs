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
        public Dictionary<int, ProcessInfo> ManagedProcessMap { get; set; }

        public ProcessInfo Master { get; set; }

        public ProcessManager()
        {
            this.AllProcessList = new ObservableCollection<ProcessInfo>();
            this.ManagedProcessList = new ObservableCollection<ProcessInfo>();
            this.ManagedProcessMap = new Dictionary<int, ProcessInfo>();

            this.Master = null;
        }


        public void AsMaster(ProcessInfo p)
        {
            if (this.Master != null)
            {
                this.Master.flag = ProcessInfo.ProcessFlag.SLAVE;
            }

            this.Master = p;
            this.Master.flag = ProcessInfo.ProcessFlag.MASTER;

            this.RefreshSelectedProcessList();
        }

        public void AsSlave(ProcessInfo p)
        {
            if (this.Master == p)
            {
                this.Master = null;
            }

            p.flag = ProcessInfo.ProcessFlag.SLAVE;
            this.RefreshSelectedProcessList();
        }

        public void AsRemove(ProcessInfo p)
        {
            if (this.Master == p)
            {
                this.Master = null;
            }

            p.flag = ProcessInfo.ProcessFlag.NONE;
            this.RefreshSelectedProcessList();
        }

        public void RefreshAllProcessList()
        {
            this.AllProcessList.Clear();

            var processes = Process.GetProcesses();
      
            foreach (Process p in processes)
            {
                if (this.ManagedProcessMap.ContainsKey(p.Id))
                {
                    AllProcessList.Add(this.ManagedProcessMap[p.Id]);
                    continue;
                }

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

        public void RefreshSelectedProcessList()
        {
            this.ManagedProcessList.Clear();
            this.ManagedProcessMap.Clear();
            foreach (ProcessInfo p in this.AllProcessList)
            {
                if (p.flag != ProcessInfo.ProcessFlag.NONE)
                {
                    this.ManagedProcessList.Add(p);
                    this.ManagedProcessMap.Add(p.pid, p);
                }
            }
        }
    }
}
