using Application.DTO_s;
using AutoMapper;
using Domain.Models;

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class TaskAutoMapper : Profile
    {
        public TaskAutoMapper()
        {
            CreateMap<TaskItemDto, TaskItem>().ReverseMap();
            CreateMap<Role, CreateRoleDTO>().ReverseMap();

        }
    }
}
