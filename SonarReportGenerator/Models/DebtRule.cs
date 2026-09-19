using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McpClient.SonarReportGenerator.Models
{
    public class DebtRule
    {
        public string RuleKey { get; set; } = "";

        public string RuleName { get; set; } = "";

        public string Severity { get; set; } = "";

        public int IssueCount { get; set; }

        public string TechnicalDebt { get; set; } = "";

        public string Category { get; set; } = "";

        public string Recommendation { get; set; } = "";
    }
}
