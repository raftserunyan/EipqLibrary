using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.CustomExceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public UserService(IMapper mapper,
                            IUnitOfWork unitOfWork,
                            UserManager<User> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<User> ConfirmUserAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            EnsureExists(user);

            user.Status = UserStatus.Active;
            await _unitOfWork.SaveChangesAsync();

            return user;
        }
        public async Task<User> DeleteUserAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            EnsureExists(user);

            var deletionResult = await _userManager.DeleteAsync(user);
            if (!deletionResult.Succeeded)
            {
                throw InvalidDeleteException(user.Email);
            }

            return user;
        }

        public async Task<UpdateUserStatusDto> UpdateUserStatusAsync(UpdateUserStatusRequest customerUpdateRequest)
        {
            if (!Enum.IsDefined(typeof(UserStatus), customerUpdateRequest.Status))
            {
                throw new BadDataException();
            }

            var userToUpdate = await _userManager.FindByIdAsync(customerUpdateRequest.Id);
            EnsureExists(userToUpdate);

            userToUpdate.Status = customerUpdateRequest.Status;
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UpdateUserStatusDto>(userToUpdate);
        }

        private static Exception InvalidDeleteException(string email)
        {
            return new GeneralException($"Could not delete user with email {email}");
        }
    }
}
