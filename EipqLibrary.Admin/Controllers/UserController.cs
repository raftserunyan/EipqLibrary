using EipqLibrary.EmailService.Interfaces;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
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

        public UserController(IUserService userService,
                                IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("changeStatus")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> ChangeUserStatus([FromBody]UpdateUserStatusRequest customerUpdateRequest)
        {
            var changedStatusCustomer = await _userService.UpdateUserStatusAsync(customerUpdateRequest);

            return Accepted(changedStatusCustomer);
        }

        [HttpPost("confirm")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> ConfirmUserAccount(string userId)
        {
            var user = await _userService.ConfirmUserAccount(userId);
            var emailMessage = _emailService.GenerateRegistrationDeniedMailMessage(user.Email);
            await _emailService.SendEmailMessageAsync(emailMessage);

            return NoContent();
        }

        [HttpPost("delete")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteUserAccount(DeleteUserRequest deleteRequest)
        {
            var deletedUser = await _userService.DeleteUserAccount(deleteRequest.UserId);
            var emailMessage = _emailService.GenerateRegistrationDeniedMailMessage(deletedUser.Email, deleteRequest.MessageToUser);
            await _emailService.SendEmailMessageAsync(emailMessage);

            return NoContent();
        }
    }
}
