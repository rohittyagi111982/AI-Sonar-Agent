using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McpClient.SonarReportGenerator.Models
{
    public class QualityOverview
    {
        public string QualityGate { get; set; } = "";
        public string ReliabilityRating { get; set; } = "";
        public string SecurityRating { get; set; } = "";
        public string MaintainabilityRating { get; set; } = "";
        public decimal Coverage { get; set; }
        public decimal Duplication { get; set; }
        public string TechnicalDebt { get; set; } = "";
        public int Bugs { get; set; }
        public int Vulnerabilities { get; set; }
        public int CodeSmells { get; set; }
        public int SecurityHotspots { get; set; }
    }
}
