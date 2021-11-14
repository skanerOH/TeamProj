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
    internal class ArticleRepository : IArticleRepository
    {
        private readonly BlogsDBContext _context;

        public ArticleRepository(BlogsDBContext blogsDBContext)
        {
            _context = blogsDBContext;
        }

        public async Task AddAsync(Article entity)
        {
            await _context.Articles.AddAsync(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var article = await _context.Articles.Where(a => a.Id == id).Include(a => a.Comments).Include(a => a.Tags).FirstOrDefaultAsync();
            if (article == null)
                return;
            foreach (var c in article.Comments)
            {
                c.IsDeleted = true;
                _context.Entry(c).State = EntityState.Modified;
            }
            article.Tags.Clear();
            article.IsDeleted = true;
            _context.Entry(article).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticlesWithDetailsAsync(Expression<Func<Article, bool>> expression)
        {
            return await _context.Articles.Include(a => a.Comments)
                .Include(a => a.Blog)
                .Include(a => a.Tags)
                .Where(expression).ToListAsync();
        }

        public async Task<Article> GetArticleWithDatailsByIdAsync(int id)
        {
            return await _context.Articles.Include(a => a.Comments)
                .Include(a => a.Blog)
                .Include(a => a.Tags)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Article> GetByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task<IEnumerable<Article>> SearchArticleWithDetailsByFiltersAndTakeThatNotTakenAsync(string textSearchStr, IEnumerable<string> tags, IEnumerable<int> takenIds, int countToTake, Expression<Func<Article, bool>> expression)
        {
            var query = _context.Articles.Include(a => a.Comments)
                .Include(a => a.Blog)
                .Include(a => a.Tags)
                .Where(expression)
                .Where(a => a.Text.ToLower().Contains(textSearchStr.ToLower()) || a.Title.ToLower().Contains(textSearchStr.ToLower()));

            foreach (var tag in tags)
            {
                query = query.Where(a => a.Tags.Any(t => t.Name.Equals(tag.ToLower())));
            }

            query = query.Where(a => !takenIds.Contains(a.Id));
            var executedQuery = await query.ToListAsync();

            int valuesToTakeCount = executedQuery.Count();
            valuesToTakeCount = countToTake > valuesToTakeCount ? valuesToTakeCount : countToTake;

            return executedQuery.Take(valuesToTakeCount).ToList();
        }

        public void Update(Article entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }

}
