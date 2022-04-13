using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.Constants.Admins;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.EmailService.Interfaces;
using EipqLibrary.Infrastructure.Data.Utils.Extensions;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.CustomExceptions;
using EipqLibrary.Shared.Utils.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class AdminIdentityService : IAdminIdentityService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AdminUser> _userManager;
        private readonly IAdminRefreshTokenService _refreshTokenService;
        private readonly IEmailService _emailService;

        public AdminIdentityService(IMapper mapper,
                                    UserManager<AdminUser> userManager,
                                    IAdminRefreshTokenService refreshTokenService,
                                    IEmailService emailService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
            _emailService = emailService;
        }

        public async Task<AdminInfo> GetByEmail(string email)
        {
            var admin = await _userManager.FindByEmailAsync(email);
            if (admin == null)
            {
                throw NonExistentAdminException(email);
            }

            var adminInfo = _mapper.Map<AdminInfo>(admin);
            return adminInfo;
        }

        public PagedData<AdminInfo> GetAll(PageInfo pageInfo)
        {
            var admins = _userManager.Users;
            var adminsInfo = _mapper.Map<List<AdminInfo>>(admins);

            return adminsInfo.PagedList(pageInfo);
        }

        public async Task<AdminUserModel> CreateAsync(AdminCreationDto adminCreationDto)
        {
            var adminUser = _mapper.Map<AdminUser>(adminCreationDto);
            var randomAdminPassword = GenerateRandomPassword(passwordLength: 10);
            adminUser.UserName = adminUser.Email;
            
            var result = await _userManager.CreateAsync(adminUser, randomAdminPassword);
            if (!result.Succeeded)
            {
                throw GetInvalidAdminCreationException(result);
            }

            result = await SetAdminRoleAsync(adminUser, adminCreationDto.Occupation);
            if (result != null && !result.Succeeded)
            {
                await _userManager.DeleteAsync(adminUser);
                throw GetInvalidAdminCreationException(result);
            }

            var mailMessage = _emailService.GenerateAdminRegistrationMailMessage(adminUser.Email, randomAdminPassword);

            await _emailService.SendEmailMessageAsync(mailMessage);

            return _mapper.Map<AdminUserModel>(adminUser);
        }

        public async Task ChangeStatusAsync(AdminChangeStatusDto adminChangeStatusDto)
        {
            var admin = await _userManager.FindByEmailAsync(adminChangeStatusDto.Email);
            if (admin == null)
            {
                throw NonExistentAdminException(adminChangeStatusDto.Email);
            }

            admin.IsActive = adminChangeStatusDto.ActiveStatus;
            var result = await _userManager.UpdateAsync(admin);

            if (!result.Succeeded)
            {
                throw InvalidUpdateException(adminChangeStatusDto.Email);
            }

            if (!admin.IsActive)
            {
                await _refreshTokenService.RemoveAllForAdminId(admin.Id);
            }
        }

        public async Task DeleteAsync(AdminDeletionDto adminDeletionDto)
        {
            var admin = await _userManager.FindByEmailAsync(adminDeletionDto.Email);
            if (admin == null)
            {
                throw NonExistentAdminException(adminDeletionDto.Email);
            }

            var result = await _userManager.DeleteAsync(admin);
            if (!result.Succeeded)
            {
                throw InvalidDeleteException(adminDeletionDto.Email);
            }

            await _refreshTokenService.RemoveAllForAdminId(admin.Id);
        }

        public async Task<AdminInfo> GetByEmailOrDefaultAsync(string email)
        {
            var admin = await _userManager.FindByEmailAsync(email);
            if (admin == null)
            {
                return null;
            }

            var adminInfo = _mapper.Map<AdminInfo>(admin);
            return adminInfo;
        }

        //Private methods
        private static Exception InvalidUpdateException(string adminEmail) =>
            new GeneralException($"Could not update user with email {adminEmail}");
        private static Exception NonExistentAdminException(string adminEmail) =>
            new EntityNotFoundException($"Could not find user with email {adminEmail}");
        private static Exception InvalidDeleteException(string adminEmail) =>
            new GeneralException($"Could not delete user with email {adminEmail}");
        private async Task<IdentityResult> SetAdminRoleAsync(AdminUser adminUser, Occupation newAdminOccupation)
        {
            return newAdminOccupation switch
            {
                Occupation.Librarian => await _userManager.AddToRoleAsync(adminUser, AdminRoleNames.Librarian),
                Occupation.Accountant => await _userManager.AddToRoleAsync(adminUser, AdminRoleNames.Accountant),
                Occupation.SuperAdmin => await _userManager.AddToRoleAsync(adminUser, AdminRoleNames.SuperAdmin),
                _ => null
            };
        }
        private static Exception GetInvalidAdminCreationException(IdentityResult identityResult)
        {
            return new BadDataException(identityResult.Errors.ToSelectiveErrorsDictionary(
                new[] { nameof(AdminUser.Email), nameof(AdminUser.UserName) }));
        }
        private static string GenerateRandomPassword(int passwordLength)
        {
            string[] randomChars = new[]
            {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",
                "abcdefghijkmnopqrstuvwxyz",
                "0123456789",
                "!?+-@#$"
            };
            var random = new Random();
            List<char> randomPassword = new List<char>();

            randomPassword.Insert(random.Next(0, randomPassword.Count),
                randomChars[0][random.Next(0, randomChars[0].Length)]);

            randomPassword.Insert(random.Next(0, randomPassword.Count),
                randomChars[1][random.Next(0, randomChars[1].Length)]);

            randomPassword.Insert(random.Next(0, randomPassword.Count),
                randomChars[2][random.Next(0, randomChars[2].Length)]);

            randomPassword.Insert(random.Next(0, randomPassword.Count),
                randomChars[3][random.Next(0, randomChars[3].Length)]);

            for (int i = randomPassword.Count; i < passwordLength; i++)
            {
                string randomChar = randomChars[random.Next(0, randomChars.Length)];
                randomPassword.Insert(random.Next(0, randomPassword.Count),
                    randomChar[random.Next(0, randomChar.Length)]);
            }

            return new string(randomPassword.ToArray());
        }
    }
}
