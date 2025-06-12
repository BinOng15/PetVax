using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Services.ExternalService
{
    public interface ICloudinariService
    {
        Task<string> UploadImage(IFormFile file);
    }
}
