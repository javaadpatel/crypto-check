export interface CryptoQuote {
    name: string;
    symbol: string;
    issuedAt: Date;
    currencyQuotes: {[symbol: string]: number}
}