using AutoMapper.Configuration.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The password is required."), DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
