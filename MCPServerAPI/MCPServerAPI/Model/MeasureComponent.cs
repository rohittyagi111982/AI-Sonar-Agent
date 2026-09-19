using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPServerAPI.Model
{
    public class MeasureComponent
    {
        public List<Measure> Measures { get; set; } = new();
    }

    public class Measure
    {
        public string Metric { get; set; } = "";

        public string Value { get; set; } = "";
    }

    public class ProjectStatusResponse
    {
        public ProjectStatus ProjectStatus { get; set; } = new();
    }

    public class ProjectStatus
    {
        public string Status { get; set; } = "";
    }
}
