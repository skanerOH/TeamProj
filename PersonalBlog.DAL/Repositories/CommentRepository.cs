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
    internal class CommentRepository : ICommentRepository
    {
        private readonly BlogsDBContext _context;

        public CommentRepository(BlogsDBContext blogsDBContext)
        {
            _context = blogsDBContext;
        }

        public async Task AddAsync(Comment entity)
        {
            await _context.Comments.AddAsync(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var comment = await _context.Comments.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (comment == null)
                return;
            comment.IsDeleted = true;
            _context.Entry(comment).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync(Expression<Func<Comment, bool>> expression)
        {
            return await _context.Comments.Where(expression).ToListAsync();
        }

        public void Update(Comment entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }

}
