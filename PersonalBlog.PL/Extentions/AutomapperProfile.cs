using AutoMapper;
using PersonalBlog.BLL.Models.DataModels;
using PersonalBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Extentions
{
    /// <summary>
    /// Profile for automapper
    /// </summary>
    public class AutomapperProfile : Profile
    {
        /// <summary>
        /// mapping profiles
        /// </summary>
        public AutomapperProfile()
        {
            CreateMap<Article, ArticleModel>().ReverseMap();
            CreateMap<Blog, BlogModel>().ReverseMap();
            CreateMap<Tag, TagModel>().ReverseMap();
            CreateMap<Comment, CommentModel>().ReverseMap();
            CreateMap<UserWithIdentity, UserModel>();
        }
    }
}
