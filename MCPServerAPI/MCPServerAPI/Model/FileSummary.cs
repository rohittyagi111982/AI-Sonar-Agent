using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPServerAPI.Model
{
    public class FileSummary
    {
        public string FileName { get; set; } = string.Empty;

        public int TotalIssues { get; set; }

        public int Bugs { get; set; }

        public int Vulnerabilities { get; set; }

        public int CodeSmells { get; set; }

        public int Blocker { get; set; }

        public int Critical { get; set; }

        public int Major { get; set; }

        public int Minor { get; set; }
    }
}
