using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Exceptions
{
    public class ProfileTypeMismatchException : Exception
    {
        public ProfileTypeMismatchException()
        {
        }

        public ProfileTypeMismatchException(string message) : base(message)
        {
        }

        public ProfileTypeMismatchException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}