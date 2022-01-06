using CreditLine.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICredit
    {
        Task<HttpResponseMessage> VerifyCredit(CreditRequest creditRequest, string clientIPAddress);
    }
}
