using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McpClient.SonarReportGenerator.Models
{
    public class Issue
    {
        public string Key { get; set; } = "";

        public string Severity { get; set; } = "";

        public string Type { get; set; } = "";

        public string RuleKey { get; set; } = "";

        public string RuleName { get; set; } = "";

        public string Component { get; set; } = "";

        public string File { get; set; } = "";

        public int Line { get; set; }

        public string Message { get; set; } = "";

        public string TechnicalDebt { get; set; } = "";

        public List<string> Tags { get; set; } = new();

        public string CleanCodeAttribute { get; set; } = "";

        public string Impact { get; set; } = "";

        public string Recommendation { get; set; } = "";
    }
}
