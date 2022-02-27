using System;
using System.Collections.Generic;
using System.Linq;

namespace EipqLibrary.Shared.CustomExceptions
{
    public class BadDataException : Exception
    {
        public const string DefaultMessage = "Bad data";
        public IDictionary<string, IEnumerable<string>> Errors { get; }
        public IDictionary<string, string> ErrorsAsSingleString { get; }

        public BadDataException(
            Dictionary<string, IEnumerable<string>> errors,
            string message = DefaultMessage)
            : base(ModifyMessage(message, errors))
        {
            Errors = errors;
            ErrorsAsSingleString = new Dictionary<string, string>();
            foreach (KeyValuePair<string, IEnumerable<string>> entry in Errors)
            {
                ErrorsAsSingleString.Add(entry.Key, string.Join(string.Empty, entry.Value));
            }
        }

        private static string ModifyMessage(string message, Dictionary<string, IEnumerable<string>> errors)
        {
            var firstError = errors.Count > 0 ? errors.ElementAt(0).Value.ElementAt(0) : string.Empty;
            return $"{message}; {firstError}";
        }

        public BadDataException(string message = DefaultMessage) : base(message)
        {
            Errors = new Dictionary<string, IEnumerable<string>>();
        }
    }
}
