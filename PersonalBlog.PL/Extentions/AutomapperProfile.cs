using AutoMapper;
using PersonalBlog.BLL.Models.DataModels;
using PersonalBlog.DAL.Entities;
using PersonalBlog.PL.Models.ViewModels;
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

            CreateMap<ArticleModel, ArticleVM>()
                .ForMember(vm => vm.CommentsCount, x => x.MapFrom(m => m.Comments.Count(c => !c.IsBanned)))
                .ForMember(vm => vm.ModifiedAt, x => x.MapFrom(m => m.ModifiedAt.ToString("dd/MM/yyyy HH:mm:ss")))
                .ForMember(vm => vm.Tags, x => x.MapFrom(m => m.Tags.Select(t => t.Name).ToList()))
                .ForMember(vm => vm.PublisherId, x => x.MapFrom(m => m.Blog.UserWithIdentityId))
                .ForMember(vm => vm.BlogTitle, x => x.MapFrom(m => m.Blog.Title));

        }
    }
}
