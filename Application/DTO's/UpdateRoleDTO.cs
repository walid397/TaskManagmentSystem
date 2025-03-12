using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO_s
{
    public class UpdateRoleDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name must contain only letters")]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string ConcurrencyStamp { get; set; }

    }
}
