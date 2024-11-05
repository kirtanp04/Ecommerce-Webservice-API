using Ecommerce_Webservices.DataObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce_Webservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public Auth(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] Register objRegister)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if(objRegister.UserName == "" || objRegister.Email == "" || objRegister.Password == "" || objRegister.Phone == "")
                {
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    objRes.message = "Please provide all the required values.";
                    return BadRequest(objRes);
                }

                // checking email already exit or not
                var user = await _userManager.FindByEmailAsync(objRegister.Email);
                if (user != null)
                {
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    objRes.message = "Email id already exits, register your account with another email.";
                    return BadRequest(objRes);
                }

                user = new User { UserName= objRegister.UserName,Email=objRegister.Email,PhoneNumber=objRegister.Phone};

              var isSuccess=  await _userManager.CreateAsync(user,objRegister.Password);

                if (!isSuccess.Succeeded)
                {
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    objRes.message = isSuccess.Errors.ToString()!;
                    return BadRequest(objRes);
                }

                objRes.isSuccess = true;
                objRes.Data = "Successfull created new user";
                objRes.message = "";
                return Ok(objRes);
            }
            catch (Exception ex)
            {
                UserResponse<string> objRes = new UserResponse<string>
                {
                    isSuccess = false,
                    message = ex.Message,
                    Data = "",
                };
                return BadRequest(objRes);
            }
        }

        [HttpPost("login")]
        public async Task <IActionResult> Login([FromBody] Login objlogin)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (objlogin.email == "" || objlogin.password == "" )
                {
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    objRes.message = "Please provide all the required values.";
                    return BadRequest(objRes);
                }

                // checking email exit or not
                var user = await _userManager.FindByEmailAsync(objlogin.email);
                if (user is null)
                {
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    objRes.message = "Emai Id does not exits";
                    return BadRequest(objRes);
                }

               bool isValidPass =  await _userManager.CheckPasswordAsync(user, objlogin.password);

                if (!isValidPass)
                {
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    objRes.message = "Invalid password, provide valid password for account" + user!.Email;
                    return Unauthorized(objRes);
                }

                // getting user role
                var userRoles = await _userManager.GetRolesAsync(user);

                //creating claims
                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Email,user.Email!),
                    new Claim(ClaimTypes.Name,user.UserName!),
                    new Claim("userID",user.Id),
                };

                var roleClaims = userRoles.Select(role => new Claim(ClaimTypes.Role, role));
                authClaims.AddRange(roleClaims);

                // jwt key
                var JwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                var signIn = new SigningCredentials(JwtKey, SecurityAlgorithms.HmacSha256);

                // token create
                var token = new JwtSecurityToken(
                   issuer: _configuration["Jwt:Issuer"],
                   audience: _configuration["Jwt:Audience"], 
                   expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]!)),
                   claims: authClaims,
                   signingCredentials: signIn
               );

                var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);    

                objRes.isSuccess = true;
                objRes.Data = tokenValue;
                objRes.message = "";
                return Ok(objRes);
            }
            catch (Exception ex)
            {

                UserResponse<string> objRes = new UserResponse<string>
                {
                    isSuccess = false,
                    message = ex.Message,
                    Data = "",
                };
                return BadRequest(objRes);
            }
        }
    }
}
