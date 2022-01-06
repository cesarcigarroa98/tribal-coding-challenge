using Application.CreditLine;
using CreditLine.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreditLine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditLineController : ControllerBase
    {
        private Credit _credit;
        public CreditLineController(Credit credit)
        {
            _credit = credit;
        }

        [HttpPost("GetCredit")]
        public async Task<IActionResult> GetCredit(Models.CreditRequest creditRequest)
        {
            try
            {
                HttpResponseMessage response = await _credit.VerifyCredit(creditRequest);

                return StatusCode((int)response.StatusCode, response.Content);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        
        }
    }
}
