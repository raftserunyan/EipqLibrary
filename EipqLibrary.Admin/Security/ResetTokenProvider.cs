using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EipqLibrary.Admin.Security
{
    public class ResetTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public ResetTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<ResetTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {
        }
    }
}
