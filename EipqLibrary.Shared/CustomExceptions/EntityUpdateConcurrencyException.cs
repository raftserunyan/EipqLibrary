using System;

namespace EipqLibrary.Shared.CustomExceptions
{
    public class EntityUpdateConcurrencyException : Exception
    {
        public const string DefaultMessage = "The entity you are trying to save has been modified.";

        public object Entity { get; }

        public EntityUpdateConcurrencyException(object entity,
            string message = DefaultMessage) : base(message)
        {
            Entity = entity;
        }
    }
}
