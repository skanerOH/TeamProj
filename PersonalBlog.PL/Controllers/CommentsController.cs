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
    /// Comments controller
    /// </summary>
    [Route("api")]
    [ApiController]
    [Authorize()]
    public class CommentsController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        private readonly ICommentService _commentService;
        private readonly IAppUser _appUser;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commentService">CommentService</param>
        /// <param name="userManagementService">UserManagementService</param>
        /// <param name="appUser">AppUser</param>
        public CommentsController(ICommentService commentService, IUserManagementService userManagementService, IAppUser appUser)
        {
            _appUser = appUser;
            _commentService = commentService;
            _userManagementService = userManagementService;
        }

        /// <summary>
        /// Get comment with provided comment id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="articleId">article id</param>
        /// <param name="commentId">comment id</param>
        /// <returns>ResponseModel</returns>
        [HttpGet("blogs/{blogId}/articles/{articleId}/comments/{commentId}")]
        public async Task<ActionResult<ResponseModel>> GetCommentByIdAsync(int blogId, int articleId, int commentId)
        {
            var commentmodel = await _commentService.GetCommentByIdAsync(commentId);
            var commentVm = new CommentVM
            {
                Id = commentmodel.Id,
                Text = commentmodel.Text,
                ModifiedAt = commentmodel.ModifiedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                PublisherName = (await _userManagementService.GetUserByIdAsync(commentmodel.UserWithIdentityId)).FullName,
                PublisherId = commentmodel.UserWithIdentityId,
                IsBanned = commentmodel.IsBanned
            };
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully got comment by id", data = commentVm };
        }

        /// <summary>
        /// Get comments by article id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="articleId">article id</param>
        /// <returns>ResponseModel</returns>
        [HttpGet("blogs/{blogId}/articles/{articleId}/comments")]
        public async Task<ActionResult<ResponseModel>> GetCommentsByArticleIdAsync(int blogId, int articleId)
        {
            IEnumerable<CommentModel> comments = await _commentService.GetCommentsByArticleIdAsync(articleId, _appUser.Id, _appUser.Role);
            var resdata = comments.Select(async c => new CommentVM
            {
                Id = c.Id,
                Text = c.Text,
                ModifiedAt = c.ModifiedAt.ToString("dd/MM/yyyy HH:mm:ss"),
                PublisherName = (await _userManagementService.GetUserByIdAsync(c.UserWithIdentityId)).FullName,
                PublisherId = c.UserWithIdentityId,
                IsBanned = c.IsBanned
            }).Select(t => t.Result).ToList();

            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "succesfully got comments", data = resdata };
        }

        /// <summary>
        /// Create comment with provided data
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="articleId">article id</param>
        /// <param name="model">comment data</param>
        /// <returns>ResponseModel</returns>
        [HttpPost("blogs/{blogId}/articles/{articleId}/comments")]
        public async Task<ActionResult<ResponseModel>> AddComment([FromRoute] int blogId, [FromRoute] int articleId, [FromBody] CommentBM model)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            await _commentService.AddCommentAsync(model.Text, _appUser.Id, articleId);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully created comment", data = null };
        }

        /// <summary>
        /// Ban comment with provided comment id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="articleId">article id</param>
        /// <param name="commentId">comment id</param>
        /// <returns>ResponseModel</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("blogs/{blogId}/articles/{articleId}/comments/{commentId}/ban")]
        public async Task<ActionResult<ResponseModel>> BanCommentAsync(int blogId, int articleId, int commentId)
        {
            await _commentService.UpdateCommentBanStatusAsync(commentId, true);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "comment was successfully banned", data = null };
        }

        /// <summary>
        /// Unban comment with provided id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="articleId">article id</param>
        /// <param name="commentId">comment id</param>
        /// <returns>ResponseModel</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("blogs/{blogId}/articles/{articleId}/comments/{commentId}/unban")]
        public async Task<ActionResult<ResponseModel>> UnbanCommentAsync(int blogId, int articleId, int commentId)
        {
            await _commentService.UpdateCommentBanStatusAsync(commentId, false);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "comment was successfully banned", data = null };
        }

        /// <summary>
        /// Edit existing comment
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="articleId">article id</param>
        /// <param name="commentId">comment id</param>
        /// <param name="model">comment data</param>
        /// <returns>ResponseModel</returns>
        [HttpPut("blogs/{blogId}/articles/{articleId}/comments/{commentId}")]
        public async Task<ActionResult<ResponseModel>> EditCommentTextAsync(int blogId, int articleId, int commentId, [FromBody] CommentBM model)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            await _commentService.UpdateCommentTextAsync(commentId, _appUser.Id, model.Text);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "comment text was successfully edited", data = null };
        }

        /// <summary>
        /// Delete comment with provided id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="articleId">article id</param>
        /// <param name="commentId">comment id</param>
        /// <returns>ResponseModel</returns>
        [HttpDelete("blogs/{blogId}/articles/{articleId}/comments/{commentId}")]
        public async Task<ActionResult<ResponseModel>> DeleteCommentAsync(int blogId, int articleId, int commentId)
        {
            await _commentService.DeleteCommentAsync(commentId, _appUser.Id, _appUser.Role);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "comment was successfully deleted", data = null };
        }
    }
}
