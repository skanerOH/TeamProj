using PersonalBlog.BLL.Interfaces;
using PersonalBlog.BLL.Models.DataModels;
using PersonalBlog.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalBlog.BLL.Validation;

namespace PersonalBlog.BLL.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<UserWithIdentity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<UserWithIdentity> _signInManager;

        public UserManagementService(UserManager<UserWithIdentity> userManager, RoleManager<IdentityRole> roleManager, SignInManager<UserWithIdentity> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task ChangeUserPasswordAsync(string userId, string password, string newPassword, string confirmPassword)
        {
            var dbuser = await _userManager.FindByIdAsync(userId);

            if (!newPassword.Equals(confirmPassword))
                throw new BlogsException("confirm password does not match new password");

            if (password.Equals(newPassword))
                throw new BlogsException("new password can not be same");

            if (dbuser == null)
                throw new BlogsException("user not found");

            var res = await _userManager.ChangePasswordAsync(dbuser, password, newPassword);

            if (!res.Succeeded)
            {
                throw new BlogsException(res.Errors.Select(e => e.Description).ToList());
            }
        }

        public async Task<UserModel> GetUserByIdAsync(string Id)
        {
            var dbuser = await _userManager.FindByIdAsync(Id);
            return new UserModel { Id = dbuser.Id, Email = dbuser.Email, FullName = dbuser.FullName };
        }

        public async Task<UserModel> LoginAsync(UserModel userModel, string password)
        {
            if (String.IsNullOrEmpty(userModel.Email) || String.IsNullOrEmpty(password))
            {
                throw new BlogsException("empty user credentials");
            }

            var res = await _signInManager.PasswordSignInAsync(userModel.Email, password, false, false);

            if (res.Succeeded)
            {
                var dbUser = await _userManager.FindByEmailAsync(userModel.Email);
                var userrole = (await _userManager.GetRolesAsync(dbUser)).FirstOrDefault();
                var uservm = new UserModel { Id = dbUser.Id, Email = dbUser.Email, FullName = dbUser.FullName, Role = userrole };

                return uservm;
            }

            throw new BlogsException("unexisting user credentials");
        }

        public async Task<UserModel> RegisterUserAsync(UserModel userModel, string password)
        {
            if (_userManager.Users.Where(u => u.FullName.Equals(userModel.FullName)).Any())
                throw new BlogsException("This user name is already taken");

            UserWithIdentity user = new UserWithIdentity()
            {
                FullName = userModel.FullName,
                Email = userModel.Email,
                UserName = userModel.Email
            };

            var res = await _userManager.CreateAsync(user, password);
            var roleErrors = new List<string>();

            if (res.Succeeded)
            {
                var rres = await _userManager.AddToRoleAsync(user, "user");

                if (rres.Succeeded)
                {
                    var dbUser = await _userManager.FindByEmailAsync(userModel.Email);
                    var resuser = new UserModel { Id = dbUser.Id, Email = dbUser.Email, FullName = dbUser.FullName, Role = "user" };
                    return resuser;
                }

                roleErrors = rres.Errors.Select(x => x.Description).ToList();
            }

            throw new BlogsException(res.Errors.Select(x => x.Description).ToArray().Concat(roleErrors));
        }
    }
}
