using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CreditLine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditLineController : ControllerBase
    {
        private ICredit _credit;

        public CreditLineController(ICredit credit)
        {
            _credit = credit;
        }

        [HttpPost("GetCredit")]
        public async Task<IActionResult> GetCredit(Models.CreditRequest creditRequest)
        {
            try
            {
                //Get client's IP address
                String clientIPAddress = this.Request != null ?
                    this.Request.HttpContext.Connection.RemoteIpAddress.ToString() : "192.168.1.254";

                HttpResponseMessage response = await _credit.VerifyCredit(creditRequest, clientIPAddress);

                return StatusCode((int)response.StatusCode, response.Content.ReadAsStringAsync().Result);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        
        }
    }
}
