using AutoMapper;
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
    public class ProfessionService : BaseService, IProfessionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProfessionService(IMapper mapper,
                                IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProfessionModel> Create(ProfessionCreationRequest professionCreationRequest)
        {
            if (await _unitOfWork.ProfessionRepository.ExistsAsync(x => x.Name == professionCreationRequest.Name))
            {
                throw new BadDataException($"'{professionCreationRequest.Name}' անվանմամբ մասնագիտություն արդեն կա");
            }

            var profession = _mapper.Map<Profession>(professionCreationRequest);

            await _unitOfWork.ProfessionRepository.AddAsync(profession);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProfessionModel>(profession);
        }

        public async Task<bool> ExistsAsync(int professionId)
        {
            return await _unitOfWork.ProfessionRepository.ExistsAsync(x => x.Id == professionId);
        }

        public async Task<List<ProfessionModel>> GetAllAsync()
        {
            var professions = await _unitOfWork.ProfessionRepository.GetAllAsync();

            return _mapper.Map<List<ProfessionModel>>(professions);
        }

        public async Task<ProfessionModel> GetByIdAsync(int id)
        {
            var profession = await _unitOfWork.ProfessionRepository.GetByIdAsync(id);

            return _mapper.Map<ProfessionModel>(profession);
        }

        public async Task<ProfessionModel> UpdateAsync(ProfessionUpdateRequest professionUpdateRequest)
        {
            var existingProfession = await _unitOfWork.ProfessionRepository.GetByIdAsync(professionUpdateRequest.Id);
            EnsureExists(existingProfession);

            bool isExistingProfessionActive = existingProfession.IsActive;

            _mapper.Map(professionUpdateRequest, existingProfession);

            if (isExistingProfessionActive != professionUpdateRequest.IsActive)
            {
                if (isExistingProfessionActive)
                {
                    existingProfession.DeletionDate = System.DateTime.Now;
                }
                else
                {
                    existingProfession.DeletionDate = null;
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProfessionModel>(existingProfession);
        }
    }
}
