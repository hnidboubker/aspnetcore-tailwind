using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TailwindIdentity.Core.Models;

public class ApplicationUser : IdentityUser<int>
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";
}


