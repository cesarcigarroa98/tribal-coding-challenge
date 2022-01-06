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
        protected ICredit Credit => _credit ??= HttpContext.RequestServices
            .GetService<ICredit>();
        //public CreditLineController(ICredit credit)
        //{
        //    _credit = credit;
        //}

        [HttpPost("GetCredit")]
        public async Task<IActionResult> GetCredit(Models.CreditRequest creditRequest)
        {
            try
            {
                String clientIPAddress = this.Request.HttpContext.Connection.RemoteIpAddress.ToString();

                HttpResponseMessage response = await _credit.VerifyCredit(creditRequest, clientIPAddress);

                return StatusCode((int)response.StatusCode, response.Content);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        
        }
    }
}
