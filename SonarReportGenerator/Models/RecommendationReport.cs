using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McpClient.SonarReportGenerator.Models
{
    public class RecommendationReport
    {
        public ExecutiveSummary Summary { get; set; } = new();

        public IssuesReport Issues { get; set; } = new();

        public DebtReport Debt { get; set; } = new();

        public List<string> HighPriority { get; set; } = new();

        public List<string> MediumPriority { get; set; } = new();

        public List<string> LowPriority { get; set; } = new();

        public string Conclusion { get; set; } = "";
    }
}
