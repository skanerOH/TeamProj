using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Authentication
{
    /// <summary>
    /// Current user data got from jwt
    /// </summary>
    public class AppUser : IAppUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="httpContextAccessor">httpContextAccessor</param>
        public AppUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// current user id
        /// </summary>
        public string Id => GetUserId();

        /// <summary>
        /// current user role
        /// </summary>
        public string Role => GetUserRole();

        private string GetUserId()
        {
            var header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var tokenstr = header.ToString().Split(' ')[1];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenstr);

            return token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId).Value;
        }

        private string GetUserRole()
        {
            var header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var tokenstr = header.ToString().Split(' ')[1];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenstr);

            return token.Claims.FirstOrDefault(x => x.Type == "role").Value;
        }
    }
}
