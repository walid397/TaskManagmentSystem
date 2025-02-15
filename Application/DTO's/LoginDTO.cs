using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO_s
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        //[EmailAddress(ErrorMessage = "Invalid Name address format.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The password is required."), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
