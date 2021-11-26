using PersonalBlog.BLL.Interfaces;
using PersonalBlog.BLL.Models.DataModels;
using PersonalBlog.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<UserModel> GetUserByIdAsync(string Id)
        {
            var dbuser = await _userManager.FindByIdAsync(Id);
            return new UserModel { Id = dbuser.Id, Email = dbuser.Email, FullName = dbuser.FullName};
        }

        public async Task<Tuple<IEnumerable<string>, UserModel>> LoginAsync(UserModel userModel, string password)
        {
            try
            {
                if (String.IsNullOrEmpty(userModel.Email) || String.IsNullOrEmpty(password))
                {
                    return new Tuple<IEnumerable<string>, UserModel>(new List<string> { "empty user credentials" }, null);
                }
                var res = await _signInManager.PasswordSignInAsync(userModel.Email, password, false, false);
                if (res.Succeeded)
                {
                    var dbUser = await _userManager.FindByEmailAsync(userModel.Email);
                    var userrole = (await _userManager.GetRolesAsync(dbUser)).FirstOrDefault();
                    var uservm = new UserModel { Id = dbUser.Id, Email = dbUser.Email, FullName = dbUser.FullName, Role = userrole };

                    return new Tuple<IEnumerable<string>, UserModel>(null, uservm);
                }
                return new Tuple<IEnumerable<string>, UserModel>(new List<string> { "unexisting user credentials" }, null);
            }
            catch (Exception e)
            {
                return new Tuple<IEnumerable<string>, UserModel>(new List<string> { e.Message }, null);
            }
        }

        public async Task<Tuple<IEnumerable<string>, UserModel>> RegisterUserAsync(UserModel userModel, string password)
        {
            try
            {
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
                        return new Tuple<IEnumerable<string>, UserModel>(null, resuser);
                    }
                    roleErrors = rres.Errors.Select(x => x.Description).ToList();
                }
                return new Tuple<IEnumerable<string>, UserModel>(res.Errors.Select(x => x.Description).ToArray().Concat(roleErrors), null);
            }
            catch (Exception e)
            {
                return new Tuple<IEnumerable<string>, UserModel>(new List<string> { e.Message }, null);
            }
        }
    }
}
