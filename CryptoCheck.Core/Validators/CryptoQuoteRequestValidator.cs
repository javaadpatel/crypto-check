using CryptoCheck.Core.Models;
using FluentValidation;
using System;
using System.Linq;

namespace CryptoCheck.Core.Validators
{
    public class CryptoQuoteRequestValidator : AbstractValidator<CryptoQuoteRequest>
    {
        public CryptoQuoteRequestValidator()
        {
            //Symbols cannot be null, and must be more than 3 characters and must be letters only
            RuleFor(x => x.Symbol)
                .NotNull()
                .Length(3,4)
                .Must(x => x.All(Char.IsLetter));
        }
    }
}
