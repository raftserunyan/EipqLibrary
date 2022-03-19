using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class GroupService : BaseService, IGroupService
    {
        private readonly IGroupRepository _groupRepo;
        private readonly IProfessionService _professionService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GroupService(IGroupRepository groupRepo,
                            IProfessionService professionService,
                            IMapper mapper,
                            IUnitOfWork unitOfWork)
        {
            _groupRepo = groupRepo;
            _professionService = professionService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<GroupModel> Create(GroupCreationRequest groupCreationRequest)
        {
            if (await _groupRepo.ExistsAsync(x => x.Number == groupCreationRequest.Number))
            {
                throw new BadDataException($"Group '{groupCreationRequest.Number}' already exists");
            }

            if (!await _professionService.ExistsAsync(groupCreationRequest.ProfessionId))
            {
                throw new BadDataException($"Profession with ID {groupCreationRequest.ProfessionId} does not exist");
            }

            var group = _mapper.Map<Group>(groupCreationRequest);

            await _groupRepo.AddAsync(group);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GroupModel>(group);
        }

        public async Task<GroupModel> GetActiveByNumberAsync(string groupNumber)
        {
            var group = await _groupRepo.GetFirstAsync(x => x.Number == groupNumber && x.GraduationDate > DateTime.Now);
            EnsureExists(group, $"Group '{groupNumber}' not found in active groups");

            return _mapper.Map<GroupModel>(group);
        }

        public async Task<List<GroupModel>> GetAllAsync(bool includeInactive = false)
        {
            var groups = await _groupRepo.GetAllAsync(x => includeInactive || x.GraduationDate > DateTime.Now);

            return _mapper.Map<List<GroupModel>>(groups);
        }

        public async Task<GroupModel> GetByIdAsync(int id, bool includeInactive = false)
        {
            var group = await _groupRepo.GetFirstAsync(x => x.Id == id && (includeInactive || x.GraduationDate > DateTime.Now));
            EnsureExists(group);

            return _mapper.Map<GroupModel>(group);
        }
    }
}
