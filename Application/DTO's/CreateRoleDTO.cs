﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO_s
{
    public class CreateRoleDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }
}
