using System;
using System.Linq;

namespace EipqLibrary.Infrastructure.Data.Utils.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasProperty(this Type classType, string propertyName)
        {
            return classType.GetProperties().Any(p => p.Name.ToLower().Equals(propertyName?.ToLower()));
        }
    }
}
