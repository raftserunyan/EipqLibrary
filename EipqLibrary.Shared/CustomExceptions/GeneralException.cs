using System;

namespace EipqLibrary.Shared.CustomExceptions
{
    public class GeneralException : Exception
    {
        public const string DefaultMessage = "Something went wrong.";
        public GeneralException(string message = DefaultMessage) : base(message)
        {
        }
    }
}
