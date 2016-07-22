using System.ComponentModel.DataAnnotations;

namespace GigHub.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}