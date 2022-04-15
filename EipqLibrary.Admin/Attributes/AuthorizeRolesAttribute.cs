﻿using Microsoft.AspNetCore.Authorization;

namespace EipqLibrary.Admin.Attributes
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}
