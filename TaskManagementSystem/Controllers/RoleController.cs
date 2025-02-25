using Application.DTO_s;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using TaskManagementSystem.ViewModels;

namespace TaskManagementSystem.Controllers
{
    [Authorize]

    public class RoleController : Controller
    {
        private RoleManager<Role> _roleManager;
        private IMapper _mapper;

        public RoleController(RoleManager<Role> roleManager , IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;

        }
        public async Task<IActionResult> GetRoles()
        {
            var ListOfRoles = _roleManager.Roles.ToList();

            var MappedRoles = ListOfRoles.Select(role => new GetRolesViewModels
            {
                Id = role.Id,
                Name = role.Name,
                IsActive = role.IsActive
            }).ToList();

            return View(MappedRoles); 
        }
        public async Task<IActionResult> UpdateRoles(int id)
        {

        
         var RoleSelected = await _roleManager.FindByIdAsync(id.ToString());
            if (RoleSelected == null)
                return BadRequest(new { message = "Invalid Task" });

            var RoleDisplayed = new UpdateRoleDTO()
            {
                Id=RoleSelected.Id,
                Name = RoleSelected.Name,
                IsActive = RoleSelected.IsActive,

            };
            return View(RoleDisplayed);
        }
        public async Task<IActionResult> SaveUpdate([FromBody] UpdateRoleViewModel model)
        {
            if(model!=null)
            {
            if(ModelState.IsValid)
            {
                    if (model == null) return BadRequest(new { message = "Invalid data" });
            }
            var RoleById =await _roleManager.FindByIdAsync(model.Id.ToString());
                RoleById.Id=model.Id;
                RoleById.Name=model.Name;
                RoleById.IsActive=model.IsActive;

            //var roleupdated = new UpdateRoleViewModel
            //{
            //    Id = RoleById.Id,
            //    Name = RoleById.Name,
            //    IsActive = RoleById.IsActive,

            //};
                //var roledisplayed = new Role()
                //{
                //    Id = roleupdated.Id,
                //    Name = roleupdated.Name,
                //    IsActive = roleupdated.IsActive,
                //};
                
                var RoleFinally = await _roleManager.UpdateAsync(RoleById);
            return Ok(new { message = "Role updated successfully" }); 
            }
            return BadRequest(new { message = "Invalid data" });
        }
    }
}
