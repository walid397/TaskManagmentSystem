using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskManagementSystem.ViewModels;
using Microsoft.AspNetCore.Http;
using static System.Net.WebRequestMethods;
using Application.Services;
using Application.DTO_s;
namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IMapper _mapper;
        private IConfiguration _config;
        private IHttpContextAccessor _httpContextAccessor;
        private RoleManager<Role> _roleManager;
        private AuthorizeService _authorizeService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IConfiguration configur, IHttpContextAccessor httpContextAccessor, RoleManager<Role> roleManager, AuthorizeService authorizeService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _config = configur;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _authorizeService = authorizeService;
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
                user.UserName = UserViewModel.FirstName + " " + UserViewModel.LastName;

                IdentityResult respons = await _userManager.CreateAsync(user, UserViewModel.Password);
                if (respons.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "TaskItem");


                }
                foreach (var item in respons.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Register", UserViewModel);
        }
        //[HttpPost]
        //public async Task<IActionResult> SaveRegister(RegisterViewModel UserViewModel)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        User user = new User();
        //        user.Email = UserViewModel.Email;
        //        user.PasswordHash = UserViewModel.Password;
        //        user.UserName = UserViewModel.FirstName;

        //     IdentityResult respons  =  await _userManager.CreateAsync(user,UserViewModel.Password);
        //        if (respons.Succeeded)
        //        {
        //            //Create Cookie
        //         await _signInManager.SignInAsync(user, isPersistent: false);
        //            RedirectToAction("Index", "Home");
        //        }
        //        foreach (var item in respons.Errors)
        //        {
        //            ModelState.AddModelError("", item.Description);
        //        }
        //    }
        //     return View("Register", UserViewModel);
        //}
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
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
                User user = await _userManager.FindByNameAsync(userViewModel.Username);
                if (user != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(user, userViewModel.Password);
                    //Create JWT
                   
                        if (found)
                        {
                            var respons = await _signInManager.PasswordSignInAsync(user.UserName, userViewModel.Password, true, false);

                            if (respons.Succeeded)
                            {
                                await _signInManager.SignInAsync(user, isPersistent: true); // 👈 حفظ الكوكيز هنا
                            }
                            //var mappingLoginDto = _mapper.Map<LoginDTO>(userViewModel);
                            LoginDTO mappingLoginDto = new LoginDTO
                            {
                                Name = userViewModel.Username,
                                Password = userViewModel.Password
                            };
                            var result = await _authorizeService.CheckAdminRole(mappingLoginDto);
                            if (result.IsSuccessfull == true)
                            {

                                await _roleManager.CreateAsync(new Role { Name = "Admin" });
                                await _userManager.AddToRoleAsync(user, "Admin");
                            }
                            else
                            {

                                await _roleManager.CreateAsync(new Role { Name = "User" });
                                await _userManager.AddToRoleAsync(user, "User");
                            }


                            List<Claim> userClaims = new List<Claim>();
                            {
                                //Token Generated id change (JWT Predifined Claims)
                                userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                                userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                                userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
                                userClaims.Add(new Claim(ClaimTypes.Email, user.Email));
                                var userRoles = await _userManager.GetRolesAsync(user);
                                foreach (var role in userRoles)
                                {
                                    userClaims.Add(new Claim(ClaimTypes.Role, role));
                                }
                            }; SymmetricSecurityKey SignKey = new(Encoding.UTF8.GetBytes(_config["JWT:SecruitKey"]));
                            SigningCredentials signingCred = new(SignKey, SecurityAlgorithms.HmacSha256Signature);
                            JwtSecurityToken mytoken = new(
                                issuer: _config["JWT:IssuerIP"],
                                audience: _config["JWT:AudienceIP"],
                                expires: DateTime.UtcNow.AddDays(1),
                                claims: userClaims,
                                signingCredentials: signingCred

                                );





                            return RedirectToAction("Index", "TaskItem", new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                                expiration = DateTime.Now.AddDays(1)
                            });
                            //return RedirectToAction("Index", "TaskItem");

                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "UserName Or Password is wrong");

                    }
                }
                return View("Login", userViewModel);
            

        }
    }
}
