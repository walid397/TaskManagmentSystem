using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.ViewModels
{
    public class RegisterViewModel
    {
        //[Required(ErrorMessage = "The FirstName is required.")]
        //[StringLength(20, MinimumLength = 3, ErrorMessage = "The FirstName must be between 3 and 20 characters.")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "The LastName is required.")]
        //[StringLength(20, MinimumLength = 3, ErrorMessage = "The LastName must be between 3 and 20 characters.")]
        public string LastName { get; set; }


        //[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        //ErrorMessage = "Password must be at least 8 characters long, include an uppercase letter, a lowercase letter, a number, and a special character.")]

        //[Required(ErrorMessage = "The password is required."), DataType(DataType.Password)]
        public string Password { get; set; }


        //[Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
    }
}
