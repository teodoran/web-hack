using System.ComponentModel.DataAnnotations;

namespace Notes.Api.Models
{
    public class QueryParameters
    {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Author { get; set; }
    }
}
