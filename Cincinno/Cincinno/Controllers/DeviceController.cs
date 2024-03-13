using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cincinno.Controllers
{
    [Route("api/[controller]")]
    public class DeviceController : Controller
    {


        [HttpGet("deviceinfo/{id}")]
        public string GetDeviceInfo(int id)
        {
            return "value";
        }

        [HttpPost("entrylog")]
        public void PostEntryLog([FromBody]string value)
        {
        }

        [HttpGet("getphotos/{id}")]
        public void GetUserPhotos(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpPost("errorlog/{id}")]
        public void LogErrorMessage(int id)
        {
        }
    }
}

