using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowConsil.src
{
    public class ProcessInfo
    {
        public int pid { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public ProcessFlag flag { get; set; }
        public string exe_path { get; set; }

        public Process raw { get; set; }

        public override string ToString()
        {
            return $"{pid} - {name} - {title} - {flag}";
        }

        public enum ProcessFlag { NONE, MASTER, SLAVE}
    }
}
