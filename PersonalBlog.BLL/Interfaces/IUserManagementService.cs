using PersonalBlog.BLL.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.BLL.Interfaces
{
    public interface IUserManagementService
    {
        public Task<UserModel> RegisterUserAsync(UserModel userModel, string password);

        public Task<UserModel> LoginAsync(UserModel userModel, string password);

        public Task<UserModel> GetUserByIdAsync(string Id);

        public Task ChangeUserPasswordAsync(string userId, string password, string newPassword, string confirmPassword);
    }
}
