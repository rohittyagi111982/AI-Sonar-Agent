using McpClient.SonarReportGenerator.Models.AiPlatform.Contracts;
using McpClient.SonarReportGenerator.Models.AiPlatform.Contractss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McpClient.SonarReportGenerator.Models
{
    public class IssuesReport
    {
        public List<FileSummary> TopFiles { get; set; } = new();

        public List<SonarIssue> TopIssues { get; set; } = new();

        public string AiAnalysis { get; set; } = "";
    }
}
