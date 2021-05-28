﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCheck.Core.Models
{
    public class CryptoQuoteRequest
    {
        public CryptoQuoteRequest(string symbol)
        {
            if (String.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(nameof(symbol));
            }

            Symbol = symbol;
        }

        /// <summary>
        /// The cryptocurrency symbol (eg. BTC, ETH)
        /// </summary>
        public string Symbol { get; set; }
    }
}
