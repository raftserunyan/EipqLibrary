using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace EipqLibrary.Shared.Utils.Extensions
{
    public static class IdentityErrorExtensions
    {
        public static Dictionary<string, IEnumerable<string>> ToSelectiveErrorsDictionary(this IEnumerable<IdentityError> errors, IEnumerable<string> keys)
        {
            var enumerable = keys as string[] ?? keys.ToArray();
            var transformedErrors = enumerable.ToDictionary(k => k, k => new List<string>());

            foreach (var error in errors)
            {
                foreach (var key in enumerable)
                {
                    if (error.Code.Contains(key, System.StringComparison.OrdinalIgnoreCase))
                    {
                        transformedErrors[key].Add(error.Description);
                        break;
                    }
                }
            }

            return transformedErrors.Where(p => p.Value.Any()).ToDictionary(p => p.Key, p => p.Value as IEnumerable<string>);
        }
    }
}
