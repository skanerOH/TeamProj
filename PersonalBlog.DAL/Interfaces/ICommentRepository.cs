using PersonalBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.DAL.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        public Task<IEnumerable<Comment>> GetCommentsAsync(Expression<Func<Comment, bool>> expression);
    }

}
