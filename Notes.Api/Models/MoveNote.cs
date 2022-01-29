namespace Notes.Api.Models;

using System.ComponentModel.DataAnnotations;

public class MoveNote
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public string NewAuthor { get; set; }
}