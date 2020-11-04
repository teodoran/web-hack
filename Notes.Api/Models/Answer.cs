using System.ComponentModel.DataAnnotations;

namespace Notes.Api.Models
{
    public class Answer
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public Flag Flag { get; set; }
    }
}
