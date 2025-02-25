using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.ViewModels
{
    public class GetRolesViewModels
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
