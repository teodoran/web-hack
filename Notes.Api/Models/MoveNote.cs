using System.ComponentModel.DataAnnotations;

namespace Notes.Api.Models
{
    public class MoveNote
    {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string NewAuthor { get; set; }
    }
}
