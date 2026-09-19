using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McpClient.SonarReportGenerator.Models
{
    public class DebtFile
    {
        public string FileName { get; set; } = "";
        public string FilePath { get; set; } = "";
        public string Language { get; set; } = "";
        public int TotalIssues { get; set; }
        public string TechnicalDebt { get; set; } = "";
        public decimal DebtPercentage { get; set; }
        public string PrimaryIssueType { get; set; } = "";
        public string AiRecommendation { get; set; } = "";
    }
    public class RecommendationInput
    {
        public string ProjectName { get; set; } = "";

        public string QualityGate { get; set; } = "";

        public string Coverage { get; set; } = "";

        public int CriticalIssues { get; set; }

        public int BlockerIssues { get; set; }

        public int Vulnerabilities { get; set; }

        public List<string> TopDebtFiles { get; set; } = new();

        public List<string> TopIssueFiles { get; set; } = new();
    }
}
