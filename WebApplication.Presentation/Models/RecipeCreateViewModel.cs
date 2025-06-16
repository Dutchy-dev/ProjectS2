using System.ComponentModel.DataAnnotations;

namespace WebApplication.Presentation.Models
{
    public class RecipeCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int CookbookId { get; set; }

    }
}
