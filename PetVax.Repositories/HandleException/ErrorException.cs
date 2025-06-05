using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.HandleException
{
       public class ErrorException : Exception
        {
            public int ErrorCode { get; }

            public ErrorException(int errorCode, string message) : base(message)
            {
                ErrorCode = errorCode;
            }

            public ErrorException(int errorCode, string message, Exception innerException)
                : base(message, innerException)
            {
                ErrorCode = errorCode;
            }
        }
    
}

