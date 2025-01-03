using System.ComponentModel.DataAnnotations;

namespace PlatFormService.Models
{
    public class Platform
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Publisher { get; set; }
        
        [Required]
        public required string Cost { get; set; }
    }
}
