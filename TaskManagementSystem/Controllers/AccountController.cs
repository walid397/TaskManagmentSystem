using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.ViewModels;
namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IMapper _mapper;
        public AccountController(UserManager<User> userManager , SignInManager<User> signInManager , IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SaveRegister(RegisterViewModel UserViewModel)
        {

            if (ModelState.IsValid)
            {
                User user = new User();
                user.Email = UserViewModel.Email;
                user.PasswordHash = UserViewModel.Password;
                user.UserName = UserViewModel.FirstName;

             IdentityResult respons  =  await _userManager.CreateAsync(user,UserViewModel.Password);
                if (respons.Succeeded)
                {
                    //Create Cookie
                 await _signInManager.SignInAsync(user, isPersistent: false);
                    RedirectToAction("Index", "Home");
                }
                foreach (var item in respons.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
             return View("Register", UserViewModel);
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return View("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveLogin(LoginViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
       User user  =  await _userManager.FindByNameAsync(userViewModel.Name);
                if (user != null)
                {
                 bool found =  await _userManager.CheckPasswordAsync(user, userViewModel.Password);
                    //Create Cookie
                    if (found) 
                    {
                        await _signInManager.SignInAsync(user,isPersistent:false);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "UserName Or Password is wrong");

                }
            }
            return RedirectToAction("Index","TaskItem");
        }

    }
}
