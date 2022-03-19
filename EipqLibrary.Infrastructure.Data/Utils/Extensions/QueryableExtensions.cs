using EipqLibrary.Domain.Core.AggregatedEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Data.Utils.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderByFieldName<T>(this IEnumerable<T> source,
            string property)
        {
            return ApplyOrder(source.AsQueryable(), property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByFieldNameDescending<T>(this IEnumerable<T> source,
            string property)
        {
            return ApplyOrder(source.AsQueryable(), property, "OrderByDescending");
        }

        public static PagedData<T> PagedList<T>(this List<T> source, PageInfo pageInfo)
        {
            return source.PagedList(pageInfo.Page, pageInfo.ItemsPerPage);
        }

        public static async Task<PagedData<T>> Paged<T>(this IQueryable<T> source, PageInfo pageInfo)
        {
            var count = source.Count();

            return new PagedData<T>
            {
                Page = new Page(count, pageInfo.ItemsPerPage, pageInfo.Page),
                Data = await source
                    .Skip((pageInfo.Page - 1) * pageInfo.ItemsPerPage)
                    .Take(pageInfo.ItemsPerPage)
                    .ToListAsync()
            };
        }

        private static PagedData<T> PagedList<T>(this IReadOnlyCollection<T> source, int page,
            int pageSize)
        {
            var count = source.Count();

            return new PagedData<T>
            {
                Page = new Page(count, pageSize, page),
                Data = source
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList()
            };
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(
            IQueryable<T> source,
            string property,
            string methodName)
        {
            var props = property.ToLower().Split('.');
            var type = typeof(T);
            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (var prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                var pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                expr = Expression.Property(expr,
                    pi ?? throw new InvalidOperationException("Invalid attribute passed during sorting"));
                type = pi.PropertyType;
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);

            var result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                              && method.IsGenericMethodDefinition
                              && method.GetGenericArguments().Length == 2
                              && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}
