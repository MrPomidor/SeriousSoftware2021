using System;

namespace SeriousBusiness.Infrastructure
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : base(message)
        {
        }
    }
}
