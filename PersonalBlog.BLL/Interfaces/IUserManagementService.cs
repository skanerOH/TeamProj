using PersonalBlog.BLL.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.BLL.Interfaces
{
    public interface IUserManagementService
    {
        public Task<Tuple<IEnumerable<string>, UserModel>> RegisterUserAsync(UserModel userModel, string password);

        public Task<Tuple<IEnumerable<String>, UserModel>> LoginAsync(UserModel userModel, string password);

        public Task<UserModel> GetUserByIdAsync(string Id);
    }
}
