﻿using Application.Interfaces;
using CreditLine.Models;
using CreditLine.Validators;
using Domain;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditLine
{
    public class Credit : ICredit
    {
        private const String Startup = "Startup";
        private const String SME = "SME";

        private DataContext _context;
        public Credit(DataContext context)
        {
            _context = context;
        }

        public async Task<HttpResponseMessage> VerifyCredit (CreditRequest creditRequest)
        {
            using (_context)
            {
                using (var dbContext = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //Create response object
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                        //Validate inputs
                        CreditValidator validator = new CreditValidator();
                        ValidationResult result = validator.Validate(creditRequest);
                        if (!result.IsValid)
                        {
                            response.StatusCode = HttpStatusCode.BadRequest;
                            response.Content = new StringContent("Credit line rejected");

                            return response;
                        }

                        //Get client's MAC address. This will be used as an unique identifier
                        String macAddress = GetMACAddress();

                        //Find current client transaction
                        Transaction clientTxn = await _context.Transactions.FirstOrDefaultAsync(
                            clientTxn => clientTxn.ClientsMacAddress == "");

                        //There is credit line already accepted for this user
                        if (clientTxn.LastValidRequest != null)
                        {
                            clientTxn.RequestsCounter++;
                            await _context.SaveChangesAsync();
                            dbContext.Commit();

                            //The system cannot receive 3 or more requests in a 2 minutes period after an
                            //accepted credit line
                            double timeGap = (DateTime.Now - (DateTime)clientTxn.LastValidRequest).TotalMinutes;
                            if (timeGap <= 2 && clientTxn.RequestsCounter > 2)
                            {
                                response.StatusCode = HttpStatusCode.TooManyRequests;
                                return response;
                            }
                            else if(timeGap > 2 && clientTxn.RequestsCounter > 2)
                            {
                                //Reset counter
                                clientTxn.RequestsCounter = 0;
                                await _context.SaveChangesAsync();

                                response.Content = new StringContent("Credit line already accepted");
                                return response;
                            }
                            else
                            {
                                response.Content = new StringContent("Credit line already accepted");
                                return response;
                            }
                        }

                        //There is an invalid request from this user
                        if (clientTxn.LastInvalidRequest != null)
                        {
                            //The system cannot receive a new requests in a 30 seconds period after a
                            //rejected credit line
                            double timeGap = (DateTime.Now - (DateTime)clientTxn.LastValidRequest).TotalSeconds;
                            if (timeGap < 30)
                            {
                                return new HttpResponseMessage(HttpStatusCode.TooManyRequests);
                            }

                        }

                        //Verify the credit application
                        if (IsValidCrdApplication(creditRequest))
                        {
                            clientTxn.LastValidRequest = DateTime.Now;
                            await _context.SaveChangesAsync();
                            dbContext.Commit();

                            response.Content = new StringContent("Credit line accepted");

                            return response;

                        }
                        else
                        {
                            clientTxn.InvalidRequestsCounter++;
                            clientTxn.LastInvalidRequest = DateTime.Now;
                            await _context.SaveChangesAsync();
                            dbContext.Commit();

                            //Client has failed 3 times
                            if (clientTxn.InvalidRequestsCounter > 2)
                            {
                                response.StatusCode = HttpStatusCode.BadRequest;
                                response.Content = new StringContent("A sales agent will contact you");

                                return response;
                            }

                            response.StatusCode = HttpStatusCode.BadRequest;
                            response.Content = new StringContent("Credit line rejected");

                            return response;
                        }

                    }
                    catch (Exception) 
                    {
                        dbContext.Rollback();

                        return new HttpResponseMessage()
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            Content = new StringContent("Credit line rejected")
                        };

                    }
                }
            }
        }

        private static bool IsValidCrdApplication(global::CreditLine.Models.CreditRequest creditRequest)
        {
            String foundingType = creditRequest.FoundingType;
            if (foundingType == SME)
            {
                //Calculate credit line based on monthly revenue
                //Credit line must be equal or greater than one fifth of the monthly renevue
                if (creditRequest.MonthlyRevenue / 5 > creditRequest.RequestedCreditLine) return true;
            }
            else if (foundingType == Startup)
            {
                //Calculate credit line based on the max value between cash balance and monthly renevue
                //Credit line must be equal or greater than one fifth of the monthly renevue or
                //it must be equal or greater than one third of the cash balance
                double cashRule = creditRequest.CashBalance / 3;
                double monthlyRule = creditRequest.MonthlyRevenue / 5;
                if (Math.Max(cashRule, monthlyRule) > creditRequest.RequestedCreditLine) return true;
            }

            return false;
        }

        private String GetMACAddress()
        {
            return "";
        }


    }
}