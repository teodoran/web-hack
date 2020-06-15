using System.ComponentModel.DataAnnotations;

namespace Notes.Api.Models
{
    public class CreateNote
    {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Author { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Content { get; set; }
    }
}
