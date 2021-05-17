using System;

namespace AppCore.Exceptions
{
    public class ForbiddenActionException : Exception
    {
        public ForbiddenActionException()
        {
        }

        public ForbiddenActionException(string message)
            : base(message)
        {
        }
    }
}
