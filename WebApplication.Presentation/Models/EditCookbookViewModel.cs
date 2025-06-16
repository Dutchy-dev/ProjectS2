using System.ComponentModel.DataAnnotations;

namespace WebApplication.Presentation.Models
{
    public class EditCookbookViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Naam is verplicht")]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
