using EipqLibrary.Shared.CustomExceptions;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public abstract class BaseService
    {
        protected void EnsureExists(object entity, string message = "")
        {
            if (entity == null)
            {
                throw new EntityNotFoundException(message);
            }
        }

        protected BadDataException BadRequest(string message = "Bad Data")
        {
            throw new BadDataException(message);
        }
    }
}
