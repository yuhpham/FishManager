using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiFishManager.Models.KoiFish
{
    public class KoiFishRequest
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public double Size { get; set; }
        public string Origin { get; set; }
        public string HealthStatus { get; set; }
    }
}
