using PersonalBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.DAL.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        public Task<IEnumerable<Article>> GetArticlesWithDetailsAsync(Expression<Func<Article, bool>> expression);

        public Task<Article> GetArticleWithDatailsByIdAsync(int id);

        public Task<IEnumerable<Article>> SearchArticleWithDetailsByFiltersAndTakeThatNotTakenAsync(string textSearchStr, IEnumerable<string> tags, IEnumerable<int> takenIds, int countToTake, Expression<Func<Article, bool>> expression);
    }

}
