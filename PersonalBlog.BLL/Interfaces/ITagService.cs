using BLL.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ITagService
    {
        public Task<ICollection<TagModel>> GetTagsByStringListAsync(IEnumerable<string> tagList);
    }
}
