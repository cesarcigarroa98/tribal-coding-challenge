using CreditLine.Models;
using FluentValidation;
using System;

namespace CreditLine.Validators
{
    public class CreditValidator : AbstractValidator<CreditRequest>
    {
        public CreditValidator()
        {
            RuleFor(credit => credit.FoundingType).NotEmpty();
            RuleFor(credit => credit.CashBalance).GreaterThan(0);
            RuleFor(credit => credit.MonthlyRevenue).GreaterThan(0);
            RuleFor(credit => credit.RequestedCreditLine).GreaterThan(0);
            RuleFor(credit => credit.RequestedDate).Must(date => IsValidDate(date));
        }

        private bool IsValidDate(DateTime date)
        {
            return date >= DateTime.Now;
        }
    }
}
