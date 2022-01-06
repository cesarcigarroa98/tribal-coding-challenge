using Application.CreditLine;
using CreditLine.Controllers;
using CreditLine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CreditLine.Tests
{
    public class CreditLineControllerTests
    {
        [Fact]
        public async void GetCredit_Returns_Bad_Request_Because_Of_Invalid_Numbers()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "SME",
                CashBalance = 0,
                MonthlyRevenue = 0,
                RequestedCreditLine = 0,
                RequestedDate = DateTime.Now
            };

            //Get DB connection
            DataContext dataContext = GetDataContext();

            //Request data
            Credit credit = new Credit(dataContext);
            CreditLineController controller = new CreditLineController(credit);
            IActionResult response = await controller.GetCredit(creditTest);

            //Clear data from DB
            dataContext.Transactions.RemoveRange(dataContext.Transactions);
            dataContext.SaveChanges();

            //Test
            ObjectResult reponseObjectResult = response as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, (int)reponseObjectResult.StatusCode);
            Assert.Equal("Credit line rejected", reponseObjectResult.Value);
        }

        [Fact]
        public async void GetCredit_Returns_Bad_Request_Because_Of_Date_In_The_Past()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "SME",
                CashBalance = 435.30,
                MonthlyRevenue = 4235.45,
                RequestedCreditLine = 100,
                RequestedDate = new DateTime(2021, 12, 31)
            };

            //Get DB connection
            DataContext dataContext = GetDataContext();

            //Request data
            Credit credit = new Credit(dataContext);
            CreditLineController controller = new CreditLineController(credit);
            IActionResult response = await controller.GetCredit(creditTest);

            //Clear data from DB
            dataContext.Transactions.RemoveRange(dataContext.Transactions);
            dataContext.SaveChanges();

            //Test
            ObjectResult reponseObjectResult = response as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, (int)reponseObjectResult.StatusCode);
            Assert.Equal("Credit line rejected", reponseObjectResult.Value);
        }

        [Fact]
        public async void GetCredit_Returns_Bad_Request_Because_Of_Empty_String()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "",
                CashBalance = 435.30,
                MonthlyRevenue = 4235.45,
                RequestedCreditLine = 100,
                RequestedDate = DateTime.Now
            };

            //Get DB connection
            DataContext dataContext = GetDataContext();

            //Request data
            Credit credit = new Credit(dataContext);
            CreditLineController controller = new CreditLineController(credit);
            IActionResult response = await controller.GetCredit(creditTest);

            //Clear data from DB
            dataContext.Transactions.RemoveRange(dataContext.Transactions);
            dataContext.SaveChanges();

            //Test
            ObjectResult reponseObjectResult = response as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, (int)reponseObjectResult.StatusCode);
            Assert.Equal("Credit line rejected", reponseObjectResult.Value);
        }

        [Fact]
        public async void GetCredit_Returns_Bad_Request_Because_Of_Invalid_Founding_Type()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "ECommerce",
                CashBalance = 435.30,
                MonthlyRevenue = 4235.45,
                RequestedCreditLine = 100,
                RequestedDate = new DateTime(2022, 12, 31)
            };

            //Get DB connection
            DataContext dataContext = GetDataContext();

            //Request data
            Credit credit = new Credit(dataContext);
            CreditLineController controller = new CreditLineController(credit);
            IActionResult response = await controller.GetCredit(creditTest);

            //Clear data from DB
            dataContext.Transactions.RemoveRange(dataContext.Transactions);
            dataContext.SaveChanges();

            //Test
            ObjectResult reponseObjectResult = response as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, (int)reponseObjectResult.StatusCode);
            Assert.Equal("Credit line rejected", reponseObjectResult.Value);
        }

        [Fact]
        public async void GetCredit_Returns_OK_Using_Startup()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "Startup",
                CashBalance = 435.30,
                MonthlyRevenue = 4235.45,
                RequestedCreditLine = 100,
                RequestedDate = new DateTime(2022, 12, 31)
            };

            //Get DB connection
            DataContext dataContext = GetDataContext();

            //Request data
            Credit credit = new Credit(dataContext);
            CreditLineController controller = new CreditLineController(credit);
            IActionResult response = await controller.GetCredit(creditTest);

            //Clear data from DB
            dataContext.Transactions.RemoveRange(dataContext.Transactions);
            dataContext.SaveChanges();

            //Test
            ObjectResult reponseObjectResult = response as ObjectResult;
            Assert.Equal(StatusCodes.Status200OK, (int)reponseObjectResult.StatusCode);
            Assert.Equal("Credit line accepted", reponseObjectResult.Value);
        }

        [Fact]
        public async void GetCredit_Returns_Bad_Request_Credit_Rejected_Using_Startup()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "Startup",
                CashBalance = 435.30,
                MonthlyRevenue = 4235.45,
                RequestedCreditLine = 1000,
                RequestedDate = new DateTime(2022, 12, 31)
            };

            //Get DB connection
            DataContext dataContext = GetDataContext();

            //Request data
            Credit credit = new Credit(dataContext);
            CreditLineController controller = new CreditLineController(credit);
            IActionResult response = await controller.GetCredit(creditTest);

            //Clear data from DB
            dataContext.Transactions.RemoveRange(dataContext.Transactions);
            dataContext.SaveChanges();

            //Test
            ObjectResult reponseObjectResult = response as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, (int)reponseObjectResult.StatusCode);
            Assert.Equal("Credit line rejected", reponseObjectResult.Value);
        }

        [Fact]
        public async void GetCredit_Returns_OK_Using_SME()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "SME",
                CashBalance = 700.30,
                MonthlyRevenue = 4500.45,
                RequestedCreditLine = 260.50,
                RequestedDate = new DateTime(2022, 12, 31)
            };

            //Get DB connection
            DataContext dataContext = GetDataContext();

            //Request data
            Credit credit = new Credit(dataContext);
            CreditLineController controller = new CreditLineController(credit);
            IActionResult response = await controller.GetCredit(creditTest);

            //Clear data from DB
            dataContext.Transactions.RemoveRange(dataContext.Transactions);
            dataContext.SaveChanges();

            //Test
            ObjectResult reponseObjectResult = response as ObjectResult;
            Assert.Equal(StatusCodes.Status200OK, (int)reponseObjectResult.StatusCode);
            Assert.Equal("Credit line accepted", reponseObjectResult.Value);
        }

        [Fact]
        public async void GetCredit_Returns_Bad_Request_Credit_Rejected_Using_SME()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "Startup",
                CashBalance = 7800.50,
                MonthlyRevenue = 5400.90,
                RequestedCreditLine = 3000,
                RequestedDate = new DateTime(2022, 12, 31)
            };

            //Get DB connection
            DataContext dataContext = GetDataContext();

            //Request data
            Credit credit = new Credit(dataContext);
            CreditLineController controller = new CreditLineController(credit);
            IActionResult response = await controller.GetCredit(creditTest);

            //Clear data from DB
            dataContext.Transactions.RemoveRange(dataContext.Transactions);
            dataContext.SaveChanges();

            //Test
            ObjectResult reponseObjectResult = response as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, (int)reponseObjectResult.StatusCode);
            Assert.Equal("Credit line rejected", reponseObjectResult.Value);
        }

        [Fact]
        public async void GetCredit_Returns_429_After_Trying_3_Times_After_Successful_Response()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "Startup",
                CashBalance = 435.30,
                MonthlyRevenue = 5000,
                RequestedCreditLine = 6000,
                RequestedDate = new DateTime(2022, 12, 31)
            };

            //Get DB connection
            DataContext dataContext = GetDataContext();

            //Request data
            Credit credit = new Credit(dataContext);
            CreditLineController controller = new CreditLineController(credit);
            IActionResult response = null;

            for (int i = 0; i < 4; i++)
            {
                response = await controller.GetCredit(creditTest);
            }

            //Clear data from DB
            dataContext.Transactions.RemoveRange(dataContext.Transactions);
            dataContext.SaveChanges();

            //Test
            ObjectResult reponseObjectResult = response as ObjectResult;
            Assert.Equal(StatusCodes.Status429TooManyRequests, (int)reponseObjectResult.StatusCode);
        }

        [Fact]
        public async void GetCredit_Returns_429_After_Failing_And_Sending_Request_Right_Away()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "Startup",
                CashBalance = 435.30,
                MonthlyRevenue = 5000,
                RequestedCreditLine = 6000,
                RequestedDate = new DateTime(2022, 12, 31)
            };

            //Get DB connection
            DataContext dataContext = GetDataContext();

            //Request data
            Credit credit = new Credit(dataContext);
            CreditLineController controller = new CreditLineController(credit);
            IActionResult response = null;

            for (int i = 0; i < 2; i++)
            {
                //Make two requests fail one after the other
                response = await controller.GetCredit(creditTest);
            }

            //Clear data from DB
            dataContext.Transactions.RemoveRange(dataContext.Transactions);
            dataContext.SaveChanges();

            //Test
            ObjectResult reponseObjectResult = response as ObjectResult;
            Assert.Equal(StatusCodes.Status429TooManyRequests, (int)reponseObjectResult.StatusCode);
        }

        [Fact]
        public async void GetCredit_Returns_Bad_Request_And_Custom_Message_After_Failing_3_Times()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "Startup",
                CashBalance = 435.30,
                MonthlyRevenue = 5000,
                RequestedCreditLine = 6000,
                RequestedDate = new DateTime(2022, 12, 31)
            };

            //Get DB connection
            DataContext dataContext = GetDataContext();

            //Request data
            Credit credit = new Credit(dataContext);
            CreditLineController controller = new CreditLineController(credit);
            IActionResult response = null;

            for (int i = 0; i < 4; i++)
            {
                //Make three requests fail but every 30 seconds 
                Task.Delay(30000).Wait();
                response = await controller.GetCredit(creditTest);
            }

            //Clear data from DB
            dataContext.Transactions.RemoveRange(dataContext.Transactions);
            dataContext.SaveChanges();

            //Test
            ObjectResult reponseObjectResult = response as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, (int)reponseObjectResult.StatusCode);
            Assert.Equal("A sales agent will contact you", reponseObjectResult.Value);
        }

        [Fact]
        public async void GetCredit_Returns_OK_Using_SME_With_Same_Monthtly_And_Cash_Rules()
        {
            CreditRequest creditTest = new CreditRequest
            {
                FoundingType = "SME",
                CashBalance = 1500,
                MonthlyRevenue = 2500,
                RequestedCreditLine = 260.50,
                RequestedDate = new DateTime(2022, 12, 31)
            };

            //Get DB connection
            DataContext dataContext = GetDataContext();

            //Request data
            Credit credit = new Credit(dataContext);
            CreditLineController controller = new CreditLineController(credit);
            IActionResult response = await controller.GetCredit(creditTest);

            //Clear data from DB
            dataContext.Transactions.RemoveRange(dataContext.Transactions);
            dataContext.SaveChanges();

            //Test
            ObjectResult reponseObjectResult = response as ObjectResult;
            Assert.Equal(StatusCodes.Status200OK, (int)reponseObjectResult.StatusCode);
            Assert.Equal("Credit line accepted", reponseObjectResult.Value);
        }

        private DataContext GetDataContext()
        {

            //Create DB connection
            String connectionString = "Server=localhost; Port=5432; User Id=admin; Password=secret; Database=creditline";
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql(connectionString);

            //Get datacontext
            return new DataContext(optionsBuilder.Options);

        }

    }
}
