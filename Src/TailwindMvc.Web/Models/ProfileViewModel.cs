using System.ComponentModel.DataAnnotations;

namespace TailwindMvc.Web.Models
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Le prénom est requis")]
        [MaxLength(100)]
        [Display(Name = "Prénom")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est requis")]
        [MaxLength(100)]
        [Display(Name = "Nom")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
