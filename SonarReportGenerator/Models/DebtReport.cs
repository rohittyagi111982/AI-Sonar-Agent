using McpClient.SonarReportGenerator.Models.AiPlatform.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McpClient.SonarReportGenerator.Models
{
    public class DebtReport
    {
        public List<FileSummary> DebtFiles { get; set; } = new();
        public int TotalDebtFile { get; set; }
        public string TechnicalDebtSummary { get; set; } = "";
    }
}
