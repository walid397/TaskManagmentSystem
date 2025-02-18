using Application.DTO_s;
using Domain.Models;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;



namespace Application.Services
{
    public class AuthorizeService
    {
        private UserManager<User> _userManager;
        public AuthorizeService(UserManager<User> userManager, SignInManager<User> signInManager)
        {

        }
        public async Task<ResponseDTO<User>>CheckAdminRole(LoginDTO userDto)
        {
            if (userDto != null)
            {
                if(userDto.Name=="Walid led" && userDto.Password=="1997")
                {
                    return  new ResponseDTO<User>
                    {
                        IsSuccessfull = true,
                        ErrorMessage = "Admin Role Will Assigned"
                    };
                }              
                    return new ResponseDTO<User>
                    {
                        IsSuccessfull = false,
                        ErrorMessage = "User Role Will Assigned"
                    }; 
            }
            return new ResponseDTO<User>
            {
                IsSuccessfull = false,
                ErrorMessage = "User Doesn't Exist"
            };
        }
    }
}