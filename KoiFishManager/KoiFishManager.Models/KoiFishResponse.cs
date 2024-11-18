using System;

namespace KoiFishManager.Models
{
    public class KoiFishResponse
    {
        public int Id { get; set; }                 // ID cá Koi
        public string Name { get; set; }            // Tên cá Koi
        public string Color { get; set; }           // Màu sắc cá Koi
        public double Size { get; set; }            // Kích thước cá Koi
        public string Origin { get; set; }          // Xuất xứ cá Koi
        public string HealthStatus { get; set; }    // Tình trạng sức khỏe
    }
}
