using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using System;
using System.Linq;

namespace EipqLibrary.Infrastructure.Data.Utils.Extensions.ObsoletExtensions
{
    public static class QueriableBookExtensions
    {
        private const string DefaultSortCriteria = nameof(Book.Name);

        public static IQueryable<T> SortBooks<T>(this IQueryable<T> books) where T : Book
        {
            return books.SortBy(SortOrder.Asc, DefaultSortCriteria);
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> books, SortOrder sortOrder, string sortCriteria) where T : Book
        {
            return (sortOrder == SortOrder.Asc ? books.OrderByFieldName(sortCriteria) : books.OrderByFieldNameDescending(sortCriteria))
                .ThenBy(c => c.Author);
        }

        public static IQueryable<T> FilterBooksByCategory<T>(this IQueryable<T> books, int? categoryId) where T : Book
        {
            if (categoryId.HasValue)
            {
                return books.Where(x => x.CategoryId == categoryId.Value);
            }

            return books;
        }

        public static IQueryable<T> FilterBooksByAuthor<T>(this IQueryable<T> books, string author) where T : Book
        {
            if (!String.IsNullOrEmpty(author))
            {
                return books.Where(x => x.Author == author);
            }

            return books;
        }
    }
}
