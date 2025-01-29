using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO_s
{
    public class TaskItemDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters.")]
        public string Description { get; set; }

        public Priority Priority { get; set; }

        [Required(ErrorMessage = "DueDate is required.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "AssignedUser is required.")]
        [StringLength(20, ErrorMessage = "AssignedUser cannot be longer than 20 characters.")]
        public string AssignedUser { get; set; }
    }
}
