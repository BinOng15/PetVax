using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
using PetVax.BusinessObjects.Models;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VetSchedulesController : ControllerBase
    {
        private readonly IVetScheduleService _vetScheduleService;

        public VetSchedulesController(IVetScheduleService vetScheduleService)
        {
            _vetScheduleService = vetScheduleService;
        }

        
    }
}
