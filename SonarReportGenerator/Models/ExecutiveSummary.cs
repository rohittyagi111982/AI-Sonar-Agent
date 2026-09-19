using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McpClient.SonarReportGenerator.Models
{
    public class ExecutiveSummary
    {
        public string ProjectName { get; set; } = "";

        public string QualityGate { get; set; } = "";

        public string Reliability { get; set; } = "";

        public string Security { get; set; } = "";

        public string Maintainability { get; set; } = "";

        public string Coverage { get; set; } = "";

        public string Duplication { get; set; } = "";

        public SeveritySummary SeveritySummary { get; set; } = new();

        public string ExecutiveSummaryText { get; set; } = "";

        public string QualityOverview { get; set; } = "";
        public int TotalIssues { get; set; }

        public int Bugs { get; set; }

        public int Vulnerabilities { get; set; }

        public int CodeSmells { get; set; }
    }
}
