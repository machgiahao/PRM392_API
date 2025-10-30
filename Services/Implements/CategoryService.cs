using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using Repositories.Repositories.Interfaces;
using Repositories.Uow;
using Services.Dtos;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categoryRepository = _unitOfWork.Repository<Category>();
            var categories = await categoryRepository.GetAllAsync();
            var categoriesDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return categoriesDtos;
        }
    }
}
