using System.Collections.Generic;
using System.Linq;

namespace EipqLibrary.Domain.Core.Constants.Admins
{
    public static class AdminRoleNames
    {
        public const string Librarian = "Librarian";
        public const string Accountant = "Accountant";
        public const string SuperAdmin = "SuperAdmin";

        public static IEnumerable<string> GetAll()
        {
            return typeof(AdminRoleNames)
                .GetFields()
                .Select(r => (string)r.GetRawConstantValue());
        }
    }
}
