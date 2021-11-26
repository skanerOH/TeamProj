using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.BLL.Interfaces;
using PersonalBlog.PL.Authentication;
using PersonalBlog.PL.Models.BindingModels;
using PersonalBlog.PL.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Controllers
{
    /// <summary>
    /// Accounts controller
    /// </summary>
    [Route("api/[controller]")]
    [Authorize()]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IAppUser _appUser;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManagementService">UserManagementService</param>
        /// <param name="appUser">AppUser</param>
        public AccountsController(IUserManagementService userManagementService, IAppUser appUser)
        {
            _userManagementService = userManagementService;
            _appUser = appUser;
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="model">ChangePasswordBM</param>
        /// <returns>ResponseModel</returns>
        [HttpPost("changepassword")]
        public async Task<ActionResult<ResponseModel>> ChangePasswordAsync([FromBody] ChangePasswordBM model)
        {
            await _userManagementService.ChangeUserPasswordAsync(_appUser.Id, model.Password, model.NewPassword, model.ConfirmPassword);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "password was successfully changed", data = null };
        }
    }
}
