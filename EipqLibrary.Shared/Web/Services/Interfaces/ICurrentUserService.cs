using System;

namespace EipqLibrary.Shared.Web.Services.Interfaces
{
    public interface ICurrentUserService
    {
        public string CurrentUserId { get; }
    }
}
