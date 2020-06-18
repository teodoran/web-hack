using System.ComponentModel.DataAnnotations;

namespace Notes.Api.Models
{
    public class Secret
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Value { get; set; }
    }
}
