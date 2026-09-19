using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McpClient.SonarReportGenerator.Models
{
    public class TopFile
    {
        public string FileName { get; set; } = "";

        public string FilePath { get; set; } = "";

        public string Language { get; set; } = "";

        public int TotalIssues { get; set; }

        public int Blocker { get; set; }

        public int Critical { get; set; }

        public int Major { get; set; }

        public int Minor { get; set; }

        public int Info { get; set; }

        public string TechnicalDebt { get; set; } = "";

        public decimal Coverage { get; set; }

        public decimal Duplication { get; set; }

        public string MaintainabilityRating { get; set; } = "";
    }
}
