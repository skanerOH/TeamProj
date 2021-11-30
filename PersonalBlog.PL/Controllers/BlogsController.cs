using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.BLL.Interfaces;
using PersonalBlog.BLL.Models.DataModels;
using PersonalBlog.PL.Authentication;
using PersonalBlog.PL.Models.BindingModels;
using PersonalBlog.PL.Models.ResponseModels;
using PersonalBlog.PL.Models.ViewModels;
using PersonalBlog.PL.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Controllers
{
    /// <summary>
    /// Blogs controller
    /// </summary>
    [Route("api")]
    [ApiController]
    [Authorize()]
    public class BlogsController : ControllerBase
    {
        private readonly IAppUser _appUser;
        private readonly IBlogService _blogService;
        private readonly IUserManagementService _userManagementService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appUser">AppUser</param>
        /// <param name="blogService">BlogService</param>
        /// <param name="userManagementService">UserManagementService</param>
        public BlogsController(IAppUser appUser, IBlogService blogService, IUserManagementService userManagementService)
        {
            _appUser = appUser;
            _blogService = blogService;
            _userManagementService = userManagementService;
        }

        /// <summary>
        /// Get all blogs
        /// </summary>
        /// <returns>ResponseModel</returns>
        [HttpGet("blogs")]
        public async Task<ActionResult<ResponseModel>> GetAllBlogsAsync()
        {
            var blogs = await _blogService.GetAllBlogsWithDetailsAsync(_appUser.Id, _appUser.Role);
            var blogvms = blogs == null ? null : blogs.Select(async b => await BlogModelToBlogVMAsync(b)).Select(b => b.Result).ToList();
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully got requested blogs", data = blogvms };
        }

        /// <summary>
        /// Get all blogs of authorized user
        /// </summary>
        /// <returns>ResponseModel</returns>
        [HttpGet("user/blogs")]
        public async Task<ActionResult<ResponseModel>> GetAllBlogsByUserIdAsync()
        {
            var blogs = await _blogService.GetBlogsWithDetalisByUserIdAsync(_appUser.Id);
            var blogvms = blogs == null ? null : blogs.Select(async b => await BlogModelToBlogVMAsync(b)).Select(b => b.Result).ToList();
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully got requested blogs", data = blogvms };
        }

        /// <summary>
        /// Get blog by id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <returns>ResponseModel</returns>
        [HttpGet("blogs/{blogId}")]
        public async Task<ActionResult<ResponseModel>> GetBlogByIdAsync(int blogId)
        {
            var blog = await _blogService.GetBlogByIdAsync(blogId, _appUser.Id, _appUser.Role);
            var blogvm = await BlogModelToBlogVMAsync(blog);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully got requested blog", data = blogvm };
        }

        /// <summary>
        /// Create blog with provided data
        /// </summary>
        /// <param name="blogBM">blog data</param>
        /// <returns>ResponseModel</returns>
        [HttpPost("blogs")]
        public async Task<ActionResult<ResponseModel>> CreateBlogAsync([FromBody] BlogBM blogBM)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            var blogModel = new BlogModel
            {
                Title = blogBM.Title,
                Description = blogBM.Description,
                UserWithIdentityId = _appUser.Id
            };

            await _blogService.CreateBlogAsync(blogModel);

            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully created blog", data = null };
        }

        /// <summary>
        /// Edit blog data
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="blogBM">blog data</param>
        /// <returns>ResponseModel</returns>
        [HttpPut("blogs/{blogId}")]
        public async Task<ActionResult<ResponseModel>> UpdateBlogAsync(int blogId, [FromBody] BlogBM blogBM)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            var blogModel = new BlogModel
            {
                Id = blogId,
                Title = blogBM.Title,
                Description = blogBM.Description,
                UserWithIdentityId = _appUser.Id
            };

            await _blogService.UpdateBlogAsync(blogModel);

            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully updated blog", data = null };
        }

        /// <summary>
        /// Ban blog by id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <returns>ResponseModel</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("blogs/{blogId}/ban")]
        public async Task<ActionResult<ResponseModel>> BanBlog(int blogId)
        {
            await _blogService.BanArticle(blogId, _appUser.Role);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully banned blog", data = null };
        }

        /// <summary>
        /// Unban blog by id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <returns>ResponseModel</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("blogs/{blogId}/unban")]
        public async Task<ActionResult<ResponseModel>> UnbanBlog(int blogId)
        {
            await _blogService.UnbanArticle(blogId, _appUser.Role);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully unbanned blog", data = null };
        }

        /// <summary>
        /// Delete blog by id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <returns>ResponseModel</returns>
        [HttpDelete("blogs/{blogId}")]
        public async Task<ActionResult<ResponseModel>> DeleteBlogById(int blogId)
        {
            await _blogService.DeleteBlogAsync(blogId, _appUser.Id);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully deleted blog", data = null };
        }


        private async Task<BlogVM> BlogModelToBlogVMAsync(BlogModel blogModel)
        {
            return new BlogVM
            {
                Id = blogModel.Id,
                Title = blogModel.Title,
                Description = blogModel.Description,
                ModifiedAt = blogModel.ModifiedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                ArticlesCount = blogModel.Articles == null ? 0 : blogModel.Articles.Count(a => !a.IsBanned),
                PublisherId = blogModel.UserWithIdentityId,
                PublisherName = (await _userManagementService.GetUserByIdAsync(blogModel.UserWithIdentityId)).FullName,
                IsBanned = blogModel.IsBanned
            };
        }
    }
}
