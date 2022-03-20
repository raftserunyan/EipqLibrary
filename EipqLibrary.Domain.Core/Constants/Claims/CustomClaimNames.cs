using System.Collections.Generic;
using System.Linq;

namespace EipqLibrary.Domain.Core.Constants.Claims
{
    public static class CustomClaimNames
    {
        public const string DeviceId = "deviceId";
        public const string Provider = "provider"; // Google, Facebook, GitHub, Other

        public static IEnumerable<string> GetAll()
        {
            return typeof(CustomClaimNames)
                .GetFields()
                .Select(r => (string)r.GetRawConstantValue());
        }
    }
}
