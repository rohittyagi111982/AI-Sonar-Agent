using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McpClient.SonarReportGenerator.Models
{
    public class SeveritySummary
    {
        public int Blocker { get; set; }

        public int Critical { get; set; }

        public int Major { get; set; }

        public int Minor { get; set; }

        public int Info { get; set; }

        public int Total => Blocker + Critical + Major + Minor + Info;
    }
}
