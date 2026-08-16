using System.ComponentModel.DataAnnotations;

namespace TailwindMvc.Web.Models
{

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        public string Email { get; set; } = string.Empty;
    }
}
