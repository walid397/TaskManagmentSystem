using Application.Contract;
using Application.DTO_s;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    public class TaskItemController : Controller
    {
     private IServices _services;
        public TaskItemController(IServices services ) 
        {
            _services = services;
        }
        public async Task<IActionResult> Index()
        {
         var result = await  _services.GetAllAsync();
            return View(result.Entity);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _services.GetByIdAsync(id);
            if (!result.IsSuccessfull)
                return NotFound();

            return View(result.Entity);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItemDto taskDto)
        {
            var result = await _services.CreateAsync(taskDto);
            if (!result.IsSuccessfull)
            {
                ModelState.AddModelError("" , result.ErrorMessage);
                return View(taskDto);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var taskResult = await _services.GetByIdAsync(id);

            if (!taskResult.IsSuccessfull)
            {
                return NotFound();
            }

            return View(taskResult.Entity);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEdit(TaskItemDto taskDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.UpdateAsync(taskDto);

                if (!result.IsSuccessfull)
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                    return View(taskDto); 
                }

                return RedirectToAction("Index"); 
            }

            return View(taskDto); 
        }

     

        [HttpPost]


        [Authorize(Roles ="Admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _services.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> SearchByPriority(Priority level)
        {
            var result = await _services.FilterByPriority(level);

            if (!result.IsSuccessfull || result.Entity == null)
            {
                return View("Index", new List<TaskItemDto>()); 
            }

            return View("Index", result.Entity); 
        }

    }
}
