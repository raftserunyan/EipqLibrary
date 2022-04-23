using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.EmailService.Interfaces;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
    [Route("api/admin/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,
                                IEmailService emailService,
                                IMapper mapper)
        {
            _userService = userService;
            _emailService = emailService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedData<UserModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] PageInfo pageInfo, [FromQuery] UserSortOption userSort, [FromQuery] UserStatus? status)
        {
            //if (!User.IsInRole(AdminRoleNames.SuperAdmin) && (status == null || status.Value == UserStatus.Deleted))
            //{
            //    return Unauthorized();
            //}

            var customers = await _userService.GetAllAsync(pageInfo, userSort, status);

            if (!customers.Data.Any())
            {
                return NoContent();
            }

            var customersModel = _mapper.Map<PagedData<UserModel>>(customers);

            return Ok(customersModel);
        }

        [HttpPost("changeStatus")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangeUserStatus([FromBody]UpdateUserStatusRequest customerUpdateRequest)
        {
            var changedStatusCustomer = await _userService.UpdateUserStatusAsync(customerUpdateRequest);

            return Ok(changedStatusCustomer);
        }

        [HttpPost("confirm")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> ConfirmUserAccount(UserConfirmationRequest request)
        {
            var user = await _userService.ConfirmUserAccount(request.UserId);
            var emailMessage = _emailService.GenerateRegistrationConfirmedMailMessage(user.Email);
            await _emailService.SendEmailMessageAsync(emailMessage);

            return Ok();
        }

        [HttpPost("delete")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteUserAccount(DeleteUserRequest deleteRequest)
        {
            var deletedUser = await _userService.DeleteUserAccount(deleteRequest.UserId);
            var emailMessage = _emailService.GenerateAccountWasDeletedMailMessage(deletedUser.Email, deleteRequest.MessageToUser);
            await _emailService.SendEmailMessageAsync(emailMessage);

            return Ok();
        }
    }
}
