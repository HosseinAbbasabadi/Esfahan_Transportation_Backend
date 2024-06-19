using System.ComponentModel.DataAnnotations;

namespace Phoenix.SSO.Quickstart.Account
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "وارد کردن نام کاربری الزامی است.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "وارد کردن کلمه رمز الزامی است.")]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
        public string GeneratedCaptcha { get; set; }
        public string EnteredCaptcha { get; set; }
    }
}