using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EipqLibrary.Infrastructure.Data.Utils.Extensions
{
    public static class DbUpdateExceptionExtension
    {
        public static bool ForeignKeyConstraintConflictOnInsert(this SqlException sqlException)
        {
            return sqlException.Number == 547;
        }

        public static bool ForeignKeyConstraintConflictOnInsert(this DbUpdateException exception)
        {
            return exception.InnerException is SqlException sqlException &&
                   sqlException.ForeignKeyConstraintConflictOnInsert();
        }
    }
}
