using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.HandleException
{
    public class ErrorResponseModel
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }

        public ErrorResponseModel(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
