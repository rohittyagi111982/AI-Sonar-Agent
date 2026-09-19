using MCPServerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPServerAPI.Model
{
    public class SonarReport
    {
        public SonarSummary Summary { get; set; }

        public List<SonarIssue> Issues { get; set; }

        public List<FileSummary> Files { get; set; }

        public DateTime GeneratedOn { get; set; }

        public string ProjectKey { get; set; }
    }
}
