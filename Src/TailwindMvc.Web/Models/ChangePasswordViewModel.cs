using System.ComponentModel.DataAnnotations;

namespace TailwindMvc.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Le mot de passe actuel est requis")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe actuel")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nouveau mot de passe est requis")]
        [StringLength(100, ErrorMessage = "Le mot de passe doit contenir entre {2} et {1} caractères.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
