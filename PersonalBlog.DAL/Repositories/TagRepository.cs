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
    internal class TagRepository : ITagRepository
    {
        private readonly BlogsDBContext _context;

        public TagRepository(BlogsDBContext blogsDBContext)
        {
            _context = blogsDBContext;
        }

        public async Task AddAsync(Tag entity)
        {
            await _context.Tags.AddAsync(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var tag = await _context.Tags.Where(t => t.Id == id).FirstOrDefaultAsync();
            if (tag == null)
                return;
            _context.Entry(tag).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task<ICollection<Tag>> GetOrCreateTagsByListAsync(IEnumerable<string> tagStrings)
        {
            var res = new List<Tag>();

            foreach (var t in tagStrings)
            {
                Tag tag = await _context.Tags.AsNoTracking().Where(tg => tg.Name.Equals(t.ToLower())).FirstOrDefaultAsync();
                if (tag == null)
                {
                    tag = new Tag { Name = t.ToLower(), IsDeleted = false };
                }
                res.Add(tag);
            }

            return res;
        }

        public void Update(Tag entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
