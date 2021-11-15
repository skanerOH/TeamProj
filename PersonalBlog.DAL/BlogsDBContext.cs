using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalBlog.DAL
{
    public class BlogsDBContext : IdentityDbContext<UserWithIdentity, IdentityRole, string>
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public BlogsDBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Comment>()
                .HasOne(c => c.UserWithIdentity)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserWithIdentityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Blog>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Comment>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Tag>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Article>().HasQueryFilter(p => !p.IsDeleted);
        }
    }

}
