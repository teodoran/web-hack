using System.ComponentModel.DataAnnotations;

namespace Notes.Api.Models
{
    public class User
    {
        [Key]
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
