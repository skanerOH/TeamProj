using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DataModels;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TagService : ITagService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<TagModel>> GetTagsByStringListAsync(IEnumerable<string> tagList)
        {
            var res = new List<TagModel>();
            tagList = tagList.Select(t => t.ToLower()).Distinct().ToList();
            if (tagList == null)
                return res;

            res = (await _unitOfWork.TagRepository.GetOrCreateTagsByListAsync(tagList)).Select(t => _mapper.Map<Tag, TagModel>(t)).ToList();
            return res;
        }
    }
}
