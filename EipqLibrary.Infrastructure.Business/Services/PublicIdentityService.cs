using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.CustomExceptions;
using EipqLibrary.Shared.Utils.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class PublicIdentityService : IPublicIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public PublicIdentityService(UserManager<User> userManager,
                                     IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var userCreationDto = _mapper.Map<UserCreationDto>(request);
            var newUser = await CreateUser(userCreationDto);

            return new RegistrationResponse("Շնորհակալություն\nՁեր գրանցման հայտը ուղարկվել է հաստատման\nԱրդյունքի մասին դուք կստանաք հաղորդագրություն ձեր կողմից նշված էլ․հասցեին");
        }

        private async Task<User> CreateUser(UserCreationDto userCreationDto)
        {
            var newUser = _mapper.Map<User>(userCreationDto);
            newUser.UserName = userCreationDto.Email;

            var creationResult = userCreationDto.Password == null
                ? await _userManager.CreateAsync(newUser)
                : await _userManager.CreateAsync(newUser, userCreationDto.Password);

            if (!creationResult.Succeeded)
            {
                throw new BadDataException(creationResult.Errors.ToSelectiveErrorsDictionary(
                    new[] { nameof(userCreationDto.Email), nameof(userCreationDto.Password) }));
            }

            return newUser;
        }
    }
}
