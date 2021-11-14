using Microsoft.EntityFrameworkCore;
using PersonalBlog.DAL.Entities;
using PersonalBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.DAL.Repositories
{
    internal class BlogRepository : IBlogRepository
    {
        private readonly BlogsDBContext _context;

        public BlogRepository(BlogsDBContext blogsDBContext)
        {
            _context = blogsDBContext;
        }

        public async Task AddAsync(Blog entity)
        {
            await _context.Blogs.AddAsync(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var blog = await _context.Blogs.Where(b => b.Id == id).Include(b => b.Articles).ThenInclude(a => a.Comments).FirstOrDefaultAsync();
            if (blog == null)
                return;
            foreach (var a in blog.Articles)
            {
                foreach (var c in a.Comments)
                {
                    c.IsDeleted = true;
                    _context.Entry(c).State = EntityState.Modified;
                }
                a.IsDeleted = true;
                _context.Entry(a).State = EntityState.Modified;
            }
            blog.IsDeleted = true;
            _context.Entry(blog).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Blog>> GetAllAsync()
        {
            return await _context.Blogs.ToListAsync();
        }

        public async Task<IEnumerable<Blog>> GetAllBlogsWithDetailsAsync(Expression<Func<Blog, bool>> expression)
        {
            return await _context.Blogs.Include(b => b.Articles)
                .Include(b => b.UserWithIdentity)
                .Where(expression)
                .ToListAsync();
        }

        public async Task<Blog> GetBlogWithDetailsByIdAsync(int id)
        {
            return await _context.Blogs.Include(b => b.Articles)
                .Include(b => b.UserWithIdentity)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }

        public void Update(Blog entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }

}
