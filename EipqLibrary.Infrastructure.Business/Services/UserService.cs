using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.CustomExceptions;
using System;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper,
                            IUnitOfWork unitOfWork,
                            IUserRepository userRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<User> ConfirmUserAccount(string userId)
        {
            var user = await _userRepository.FindAsync(u => u.Id == userId);
            EnsureExists(user);

            user.Status = UserStatus.Active;
            await _unitOfWork.SaveChangesAsync();

            return user;
        }
        public async Task<User> DeleteUserAccount(string userId)
        {
            var user = await _userRepository.FindAsync(u => u.Id == userId);
            EnsureExists(user);

            _userRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }

        public async Task<UpdateUserStatusDto> UpdateUserStatusAsync(UpdateUserStatusRequest customerUpdateRequest)
        {
            if (!Enum.IsDefined(typeof(UserStatus), customerUpdateRequest.Status))
            {
                throw new BadDataException();
            }

            var userToUpdate = await _userRepository.FindAsync(u => u.Id == customerUpdateRequest.Id);
            EnsureExists(userToUpdate);

            userToUpdate.Status = customerUpdateRequest.Status;
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UpdateUserStatusDto>(userToUpdate);
        }

        public async Task<PagedData<UserDto>> GetAllAsync(PageInfo pageInfo, UserSortOption userSort, UserStatus? status)
        {
            var pagedUsers = await _userRepository.GetAllAsync(pageInfo, userSort, status);
            EnsureExists(pagedUsers);

            var pagedUserDtos = _mapper.Map<PagedData<UserDto>>(pagedUsers);

            return pagedUserDtos;
        }

        private static Exception InvalidDeleteException(string email)
        {
            return new GeneralException($"Could not delete user with email {email}");
        }
    }
}
