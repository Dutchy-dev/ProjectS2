using System.ComponentModel.DataAnnotations;

namespace WebApplication.Presentation.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overeen")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
