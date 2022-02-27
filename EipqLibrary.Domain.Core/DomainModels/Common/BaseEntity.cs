using System;

namespace EipqLibrary.Domain.Core.DomainModels.Common
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }
    }
}
