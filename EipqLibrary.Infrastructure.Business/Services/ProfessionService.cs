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
    public class ProfessionService : IProfessionService
    {
        private readonly IProfessionRepository _professionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProfessionService(IProfessionRepository professionRepository,
                                IMapper mapper,
                                IUnitOfWork unitOfWork)
        {
            _professionRepository = professionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProfessionModel> Create(ProfessionCreationRequest professionCreationRequest)
        {
            if (await _professionRepository.ExistsAsync(x => x.Name == professionCreationRequest.Name))
            {
                throw new BadDataException($"A profession with name '{professionCreationRequest.Name}' already exists");
            }

            var profession = _mapper.Map<Profession>(professionCreationRequest);

            await _professionRepository.AddAsync(profession);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProfessionModel>(profession);
        }

        public async Task<bool> ExistsAsync(int professionId)
        {
            return await _professionRepository.ExistsAsync(x => x.Id == professionId);
        }

        public async Task<List<ProfessionModel>> GetAllAsync()
        {
            var professions = await _professionRepository.GetAllAsync();

            return _mapper.Map<List<ProfessionModel>>(professions);
        }

        public async Task<ProfessionModel> GetByIdAsync(int id)
        {
            var profession = await _professionRepository.GetByIdAsync(id);

            return _mapper.Map<ProfessionModel>(profession);
        }
    }
}
