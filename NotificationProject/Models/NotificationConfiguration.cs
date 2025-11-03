using System.ComponentModel.DataAnnotations;

namespace NotificationProject.Models
{
    public class NotificationConfiguration
    {
        [Key]
        public int Id { get; set; }
        public string DefaultChannel { get; set; } 
        public DateTime UpdatedAt { get; set; }
    }
}
