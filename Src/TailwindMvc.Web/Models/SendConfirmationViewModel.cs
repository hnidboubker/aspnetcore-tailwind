using System.ComponentModel.DataAnnotations;

namespace TailwindMvc.Web.Models
{
    public class SendConfirmationViewModel
    {
        [Display(Name = "Adresse e-mail")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail est invalide.")]
        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        public string Email { get; set; } = string.Empty;

        public string? ConfirmationMessage { get; set; }
    }
}
