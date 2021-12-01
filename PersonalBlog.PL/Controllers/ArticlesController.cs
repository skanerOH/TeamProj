using AutoMapper;
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
    /// Articles controller
    /// </summary>
    [Route("api")]
    [Authorize()]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IAppUser _appUser;
        private readonly IArticleService _articleService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appUser">request user data</param>
        /// <param name="articleService">articles service</param>
        /// <param name="mapper">mapper</param>
        /// <param name="tagService">tag service</param>
        public ArticlesController(IAppUser appUser, IArticleService articleService, IMapper mapper, ITagService tagService)
        {
            _appUser = appUser;
            _articleService = articleService;
            _tagService = tagService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get articles by blog id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <returns>ResponseModel</returns>
        [HttpGet("blogs/{blogId}/articles")]
        public async Task<ActionResult<ResponseModel>> GetArticlesByBlogIdAsync(int blogId)
        {
            var articleModels = await _articleService.GetArticlesWithDetailsByBlogIdAsync(blogId, _appUser.Id, _appUser.Role);
            var articleVMs = _mapper.Map<IEnumerable<ArticleModel>, IEnumerable<ArticleVM>>(articleModels);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully got requested article", data = articleVMs };
        }

        /// <summary>
        /// Get article by id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="articleId">article id</param>
        /// <returns>Response model</returns>
        [HttpGet("blogs/{blogId}/articles/{articleId}")]
        public async Task<ActionResult<ResponseModel>> GetArticleByIdAsync(int blogId, int articleId)
        {
            var article = await _articleService.GetArticleByIdAsync(articleId, _appUser.Id, _appUser.Role);
            var articleVM = _mapper.Map<ArticleModel, ArticleVM>(article);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully got requested articles", data = articleVM };
        }

        /// <summary>
        /// Create new article in blog with provided id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="model">article data</param>
        /// <returns>ResponseModel</returns>
        [HttpPost("blogs/{blogId}/articles")]
        public async Task<ActionResult<ResponseModel>> CreateArticleAsync(int blogId, [FromBody] ArticleBM model)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            var articleModel = new ArticleModel
            {
                Text = model.Text,
                Title = model.Title,
                Tags = await _tagService.GetTagsByStringListAsync(model.Tags)
            };

            await _articleService.CreateArticleAsync(blogId, _appUser.Id, articleModel);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully created article", data = null };
        }

        /// <summary>
        /// Edit existing article with provided id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="articleId">article id</param>
        /// <param name="model">article data</param>
        /// <returns>ResponseModel</returns>
        [HttpPut("blogs/{blogId}/articles/{articleId}")]
        public async Task<ActionResult<ResponseModel>> UpdateArticleAsync(int blogId, int articleId, [FromBody] ArticleBM model)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            var articleModel = new ArticleModel
            {
                Id = articleId,
                Text = model.Text,
                Title = model.Title,
                Tags = await _tagService.GetTagsByStringListAsync(model.Tags)
            };

            await _articleService.UpdateArticleAsync(blogId, _appUser.Id, articleModel);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully edited article", data = null };
        }

        /// <summary>
        /// Ban article with provided id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="articleId">article id</param>
        /// <returns>ResponseModel</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("blogs/{blogId}/articles/{articleId}/ban")]
        public async Task<ActionResult<ResponseModel>> BanArticleAsync(int blogId, int articleId)
        {
            await _articleService.BanArticleByIdAsync(articleId, _appUser.Id, _appUser.Role);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully banned article", data = null };
        }

        /// <summary>
        /// Unban article with provided id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="articleId">article id</param>
        /// <returns>ResponseModel</returns>
        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("blogs/{blogId}/articles/{articleId}/unban")]
        public async Task<ActionResult<ResponseModel>> UnbanArticleAsync(int blogId, int articleId)
        {
            await _articleService.UnbanArticleByIdAsync(articleId, _appUser.Id, _appUser.Role);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully banned article", data = null };

        }

        /// <summary>
        /// Delete article with provided id
        /// </summary>
        /// <param name="blogId">blog id</param>
        /// <param name="articleId">article id</param>
        /// <returns>ResponseModel</returns>
        [HttpDelete("blogs/{blogId}/articles/{articleId}")]
        public async Task<ActionResult<ResponseModel>> DeleteArticleAsync(int blogId, int articleId)
        {
            await _articleService.DeleteArticleAsync(articleId, _appUser.Id);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully deleted article", data = null };
        }

        /// <summary>
        /// Search article by provided params
        /// </summary>
        /// <param name="model">search parameters model</param>
        /// <returns>ResponseModel</returns>
        [HttpGet("articles/search")]
        public async Task<ActionResult<ResponseModel>> SearchArticlesAsync([FromBody] ArticleSearchFilterBM model) //Should be FromQuery but changed for testing
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateException(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            var mres = await _articleService.GetArticlesByFiltersAsync(model.S, model.T, model.I, 3, _appUser.Role);
            var res = _mapper.Map<IEnumerable<ArticleModel>, IEnumerable<ArticleVM>>(mres);
            return new ResponseModel { responseCode = ResponseModelCode.OK, responseMessage = "successfully got articles", data = res };
        }
    }
}
