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
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } // Tên cá Koi

    
        [MaxLength(20)]
        public string Color { get; set; } // Màu sắc

 
        [Range(1, 100)]
        public double Size { get; set; } // Kích thước

    
        [MaxLength(50)]
        public string Origin { get; set; } // Xuất xứ

        [Required]
        [MaxLength(20)]
        public string HealthStatus { get; set; } // Tình trạng sức khỏe

        [Required]
        public int PondId { get; set; } // Mã hồ cá
    }
}
