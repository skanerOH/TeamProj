using PersonalBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.DAL.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        public Task<ICollection<Tag>> GetOrCreateTagsByListAsync(IEnumerable<string> tagStrings);
    }

}
