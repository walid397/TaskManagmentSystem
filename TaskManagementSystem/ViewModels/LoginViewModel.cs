using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        //[EmailAddress(ErrorMessage = "Invalid Name address format.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The password is required."), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
