using CreditLine.Controllers;
using CreditLine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace CreditLine.Tests
{
    public class CreditLineControllerTests
    {
        [Fact]
        public void GetCredit_Returns_Bad_Request_Because_Of_Invalid_Numbers()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "SME",
                CashBalance = 0,
                MonthlyRevenue = 0,
                RequestedCreditLine = 0,
                RequestedDate = DateTime.Now
            };

            //Make request
            CreditLineController controller = new CreditLineController();

            //Get status code from response
            IActionResult response = controller.GetCredit(creditTest);
            ObjectResult reponseObjectResult = response as ObjectResult;

            //Test
            Assert.Equal(StatusCodes.Status400BadRequest, reponseObjectResult.StatusCode);
        }

        [Fact]
        public void GetCredit_Returns_Bad_Request_Because_Of_Date_In_The_Past()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "SME",
                CashBalance = 435.30,
                MonthlyRevenue = 4235.45,
                RequestedCreditLine = 100,
                RequestedDate = new DateTime(2021, 12, 31)
            };

            //Make request
            CreditLineController controller = new CreditLineController();

            //Get status code from response
            IActionResult response = controller.GetCredit(creditTest);
            ObjectResult reponseObjectResult = response as ObjectResult;

            //Test
            Assert.Equal(StatusCodes.Status400BadRequest, reponseObjectResult.StatusCode);
        }

        [Fact]
        public void GetCredit_Returns_Bad_Request_Because_Of_Empty_String()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "",
                CashBalance = 435.30,
                MonthlyRevenue = 4235.45,
                RequestedCreditLine = 100,
                RequestedDate = DateTime.Now
            };

            //Make request
            CreditLineController controller = new CreditLineController();

            //Get status code from response
            IActionResult response = controller.GetCredit(creditTest);
            ObjectResult reponseObjectResult = response as ObjectResult;

            //Test
            Assert.Equal(StatusCodes.Status400BadRequest, reponseObjectResult.StatusCode);
        }

        [Fact]
        public void GetCredit_Returns_Bad_Request_After_Failing_3_Times()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "Startup",
                CashBalance = 435.30,
                MonthlyRevenue = 5000,
                RequestedCreditLine = 6000,
                RequestedDate = new DateTime(2022, 12, 31)
            };

            //Make request
            CreditLineController controller = new CreditLineController();

            //Get status code from response
            IActionResult response = null;
            for(int i = 0; i < 4; i ++)
            {
                response = controller.GetCredit(creditTest);
            }
            ObjectResult reponseObjectResult = response as ObjectResult;

            //Test
            Assert.Equal(StatusCodes.Status400BadRequest, reponseObjectResult.StatusCode);
        }

        [Fact]
        public void GetCredit_Returns_429_After_Failing_And_Sending_Request_Right_Away()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "Startup",
                CashBalance = 435.30,
                MonthlyRevenue = 5000,
                RequestedCreditLine = 6000,
                RequestedDate = new DateTime(2022, 12, 31)
            };

            //Make request
            CreditLineController controller = new CreditLineController();

            //Get status code from response
            IActionResult response = null;
            for (int i = 0; i < 2; i++)
            {
                response = controller.GetCredit(creditTest);
            }
            ObjectResult reponseObjectResult = response as ObjectResult;

            //Test
            Assert.Equal(StatusCodes.Status429TooManyRequests, reponseObjectResult.StatusCode);
        }
    }
}
