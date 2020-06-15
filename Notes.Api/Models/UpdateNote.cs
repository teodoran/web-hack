using System.ComponentModel.DataAnnotations;

namespace Notes.Api.Models
{
    public class UpdateNote
    {
        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Content { get; set; }
    }
}
