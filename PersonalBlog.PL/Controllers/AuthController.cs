using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PersonalBlog.BLL.Interfaces;
using PersonalBlog.BLL.Models.DataModels;
using PersonalBlog.PL.Models.BindingModels;
using PersonalBlog.PL.Models.DTOs;
using PersonalBlog.PL.Models.JWT;
using PersonalBlog.PL.Models.ResponseModels;
using PersonalBlog.PL.Validation;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Controllers
{
    /// <summary>
    /// User authentication controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        private readonly JWTConfig _jWTConfig;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="jWTConfig">JWT config</param>
        /// <param name="userManagementService">UserManagementService</param>
        public AuthController(IOptions<JWTConfig> jWTConfig, IUserManagementService userManagementService)
        {
            _jWTConfig = jWTConfig.Value;
            _userManagementService = userManagementService;
        }

        /// <summary>
        /// Register user with provided data
        /// </summary>
        /// <param name="model">user registration data</param>
        /// <returns>ResponseModel (contains JWT if succesfull)</returns>
        [HttpPost("register")]
        public async Task<ActionResult<ResponseModel>> RegisterUser([FromBody] RegisterUserBM model)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            var user = new UserModel { Email = model.Email, FullName = model.FullName };
            var res = await _userManagementService.RegisterUserAsync(user, model.Password);
            var userDTO = new UserWithTokenDTO { FullName = res.FullName, Id = res.Id, Role = res.Role, Token = GenerateToken(res, "user") };
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully registered", data = userDTO };
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="model">user login data</param>
        /// <returns>ResponseModel (contains JWT if succesfull)</returns>
        [HttpPost("login")]
        public async Task<ActionResult<ResponseModel>> Login([FromBody] LoginUserBM model)
        {
            UserModel userModel = new UserModel { Email = model.Email };
            var res = await _userManagementService.LoginAsync(userModel, model.Password);
            var uservm = new UserWithTokenDTO { FullName = res.FullName, Id = res.Id, Role = res.Role, Token = GenerateToken(res, res.Role) };
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully logged in", data = uservm };
        }

        private string GenerateToken(UserModel user, string role)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jWTConfig.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new System.Security.Claims.Claim(ClaimTypes.Role, role)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = _jWTConfig.Issuer,
                Audience = _jWTConfig.Audience

            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
