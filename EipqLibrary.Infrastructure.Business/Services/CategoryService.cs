﻿using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.CustomExceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CreateCategory(CategoryCreationRequest categoryCreationRequest)
        {
            var category = await _unitOfWork.CategoryRepository.GetFirstWithIncludeAsync(x => x.Name == categoryCreationRequest.Name);

            if (category != null)
                throw new BadDataException("Category already exists");

            category = _mapper.Map<Category>(categoryCreationRequest);

            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return category.Id;
        }

        public async Task DeleteAsync(int categoryId)
        {
            await _unitOfWork.CategoryRepository.DeleteAsync(categoryId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<CategoryModel>> GetAllAsync()
        {
            var categories =  await _unitOfWork.CategoryRepository.GetAllAsync();

            return _mapper.Map<List<CategoryModel>>(categories);
        }

        public async Task<CategoryModel> GetByIdAsync(int categoryId)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);

            return _mapper.Map<CategoryModel>(category);
        }

        public async Task<CategoryModel> UpdateAsync(CategoryUpdateRequest updateRequest)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(updateRequest.Id);
            EnsureExists(category, $"Category with ID {updateRequest.Id} does not exist");

            _mapper.Map(updateRequest, category);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CategoryModel>(category);
        }
    }
}
