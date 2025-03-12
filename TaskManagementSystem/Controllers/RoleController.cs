using Application.DTO_s;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using NuGet.Protocol.Plugins;
using System.Data.Entity;
using TaskManagementSystem.ViewModels;

namespace TaskManagementSystem.Controllers
{
    [Authorize]

    public class RoleController : Controller
    {
        private RoleManager<Role> _roleManager;
        private IMapper _mapper;

        public RoleController(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;

        }
        public async Task<IActionResult> GetRoles()
        {
            var ListOfRoles = _roleManager.Roles.ToList();
            if (ListOfRoles != null)
            {
                var MappedRoles = ListOfRoles.Select(role => new GetRolesViewModels
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsActive = role.IsActive
                }).ToList();

                return View(MappedRoles);
            }
            else
            {
                return BadRequest(new { message = "List Is Empty" });
            }
        }
        public async Task<IActionResult> UpdateRoles(int id)
        {


            var RoleSelected = await _roleManager.FindByIdAsync(id.ToString());
            if (RoleSelected == null)
                return BadRequest(new { message = "Invalid Task" });

            var RoleDisplayed = new UpdateRoleDTO()
            {
                Id = RoleSelected.Id,
                Name = RoleSelected.Name,
                IsActive = RoleSelected.IsActive,
                ConcurrencyStamp = RoleSelected.ConcurrencyStamp
            };
            return View(RoleDisplayed);
        }
        public async Task<IActionResult> SaveUpdate([FromBody] UpdateRoleDTO model)
        {
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    var RoleById = await _roleManager.FindByIdAsync(model.Id.ToString());
                    if (RoleById != null)
                    {

                        RoleById.Name = model.Name;
                        RoleById.IsActive = model.IsActive;
                        RoleById.ConcurrencyStamp = model.ConcurrencyStamp;
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = "Invalid data" });
                    }
                    await _roleManager.UpdateAsync(RoleById);
                    return Ok(new { success = true, message = "Role updated successfully" });
                }
                return BadRequest(new { success = false, message = "Invalid data", errors = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage) });

            }
            return BadRequest(new { success = false, message = "Invalid data" });

        }
        public async Task<IActionResult> Delete(int id)
        {
            var RoleById = await _roleManager.FindByIdAsync(id.ToString());
            if (RoleById != null)
            {

                await _roleManager.DeleteAsync(RoleById);
                return RedirectToAction("GetRoles");

            }
            return BadRequest(new { success = false, message = "Role Doesn't Exist" });

        }
        [HttpGet]
        public async Task<IActionResult> Create(CreateRoleDTO Roledto)
        {
            return View(Roledto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleDTO Roledto)
        {
            if (Roledto != null)
            {

                if (ModelState.IsValid)
                {
                    var ExistRole = await _roleManager.FindByNameAsync(Roledto.Name);
                    if (ExistRole == null)
                    {

                        var rolemapped = _mapper.Map<Role>(Roledto);
                        await _roleManager.CreateAsync(rolemapped);
                        return RedirectToAction("GetRoles");

                    }
                    else
                    {
                        return BadRequest(new { success = false, message = "Role Already Exist" });
                    }

                }
            }
            return BadRequest(new { success = false, message = "Invalid data" });
        }
    }
}
