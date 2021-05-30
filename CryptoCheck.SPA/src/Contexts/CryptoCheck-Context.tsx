import { createContext, useContext, Component } from "react";
import * as moment from "moment";
import axios, { AxiosInstance } from "axios";
import { Config } from "../Config/Config";
import { CryptoQuote } from "../Models/Api/CryptoQuote";
import { CryptoQuoteRequest } from "../Models/Api/CryptoQuoteRequest";
import { CryptoQuoteError } from "../Models/Api/CryptoQuoteError";

//define context interface
interface ContextValueType {
  getQuote?: (symbol: string) => Promise<void>;
}

//create context
export const CryptoCheckContext: any =
  createContext<ContextValueType | null>(null);

export const useCryptoCheck: any = () => useContext(CryptoCheckContext);

//create provider
interface IState {
  currentQuote: CryptoQuote;
  quoteError: CryptoQuoteError;
}

export class CryptoCheckProvider extends Component<{}, IState> {
  constructor(props: any) {
    super(props);
    this.state = {
      currentQuote: null,
      quoteError: null,
    };
  }

  createCryptoCheckClient = async (): Promise<AxiosInstance> => {
    return axios.create({
      baseURL: Config.cryptocheck_api_url,
    });
  };

  generateFakeCryptoQuote = (): CryptoQuote => {
    var fakeCurrency: { [symbol: string]: number } = {};
    fakeCurrency["EUR"] = 1.235 * Math.random();
    fakeCurrency["USD"] = 22.23 * Math.random();
    var fakeQuote: CryptoQuote = {
      name: "Bitcoin",
      symbol: "BTC",
      issuedAt: moment.utc().toDate(),
      currencyQuotes: fakeCurrency,
    };

    return fakeQuote;
  };

  getQuote = async (symbol: string): Promise<void> => {
    try {
      const cryptoCheckClient = await this.createCryptoCheckClient();

      const response = await cryptoCheckClient.get<CryptoQuote>(
        `/quote/${symbol}`,
        {
          params: {
            code: Config.cryptocheck_api_code,
          },
        }
      );

      this.setState({ currentQuote: response.data, quoteError: null });
    } catch (err: any) {
      var quoteError = err.response.data as CryptoQuoteError;

      this.setState({
        currentQuote: null,
        quoteError,
      });

      console.error(err);
    }
  };

  render() {
    const { currentQuote, quoteError } = this.state;
    const { children } = this.props;

    const configObject = {
      currentQuote,
      quoteError,
      getQuote: (symbol: string) => this.getQuote(symbol),
    };

    return (
      <CryptoCheckContext.Provider value={configObject}>
        {children}
      </CryptoCheckContext.Provider>
    );
  }
}
