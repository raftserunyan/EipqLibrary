using System.Collections.Generic;

namespace EipqLibrary.Shared.Web.Dtos.Tokens
{
    public class UserTokenInfo
    {
        public string Id { get; set; }
        public ICollection<string> Roles { get; set; }
    }
}
