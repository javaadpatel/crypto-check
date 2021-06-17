using CryptoCheck.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCheck.Services
{
    public delegate ICryptoQuoteService CryptoQuoteServiceResolver(string key);
}
