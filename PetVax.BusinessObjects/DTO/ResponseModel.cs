using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }

        public string Title { get; set; }
        public object Data { get; set; }

        public ResponseModel(int statusCode, string title, object data)
        {
            StatusCode = statusCode;
            Title = title;
            Data = data;
        }
    }
}
