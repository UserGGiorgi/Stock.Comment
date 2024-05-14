using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Principal;
using WWWW_Stock.DTOs.Account;
using WWWW_Stock.Interface;
using WWWW_Stock.Models;

namespace WWWW_Stock.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountControler:ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountControler(UserManager<AppUser> userManager,ITokenService tokenService,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService=tokenService;
            _signInManager=signInManager;
        }
        [HttpPost("login")]
        public async Task<IActionResult> login(LoginDto loginDto)
        {
            if ((!ModelState.IsValid))
                  return BadRequest(ModelState);
            var user=await _userManager.Users.FirstOrDefaultAsync(x=>x.UserName == loginDto.UserName);  
            if(user == null) return Unauthorized("Invalid Username!");
            var result=await _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);
            if(!result.Succeeded) return Unauthorized("UserName Not Found Or/And Password Is Incorrect");
            return Ok(
                new NewUserDto
                { email=loginDto.UserName,
                  UserName=loginDto.UserName,
                  Token=_tokenService.CreateToken(user)
                });  
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var AppUser = new AppUser()
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                };

                var createdUser=await _userManager.CreateAsync(AppUser,registerDto.Password);
                if(createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(AppUser, "User");
                    if(roleResult.Succeeded)
                    {
                        return Ok(
                            new NewUserDto
                            {
                                UserName=AppUser.UserName,
                                email=AppUser.Email,
                                Token=_tokenService.CreateToken(AppUser)
                            });
                    }
                    else
                    {
                        return StatusCode(500,roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }

            }
            catch (Exception e)
            {
                return StatusCode(500,e);
            }

        }
    }
}
