//import dependencies
import React from "react";
import * as moment from "moment";

//import react-testing methods
import {
  render,
  fireEvent,
  waitFor,
  screen,
  within,
} from "@testing-library/react";

//addd custom jest matchers from jest-dom
import "@testing-library/jest-dom/extend-expect";

//import component to test
import QuoteInfoCard from "./QuoteInfoCard";
import { CryptoCheckContext } from "../../../Contexts/CryptoCheck-Context";
import { CryptoQuote } from "../../../Models/Api/CryptoQuote";
import { CryptoQuoteError } from "../../../Models/Api/CryptoQuoteError";

const generateFakeCryptoQuote = (): CryptoQuote => {
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

const generateFakeCryptoError = (): CryptoQuoteError => {
  return {
    message: "Oops, something went wrong",
  };
};

test("given quote should display quote information", async () => {
  //arrange
  const configObject = {
    currentQuote: generateFakeCryptoQuote(),
    quoteError: null,
  };

  render(
    <CryptoCheckContext.Provider value={configObject}>
      <QuoteInfoCard />
    </CryptoCheckContext.Provider>
  );

  //assert
  expect(screen.getByTestId("name")).toHaveTextContent(
    `${configObject.currentQuote.name}`
  );
  expect(screen.getByTestId("symbol")).toHaveTextContent(
    `${configObject.currentQuote.symbol}`
  );
  expect(screen.getByTestId("issuedAt")).toHaveTextContent(
    `${configObject.currentQuote.issuedAt.toLocaleTimeString()}`
  );

  //check currency quotes are displayed
  Object.entries(configObject.currentQuote.currencyQuotes).map((key) => {
    const row = screen.getByText(key[0]).closest("tr");

    const utils = within(row);
    expect(utils.getByText(key[0])).toBeInTheDocument();
    expect(utils.getByText(key[1].toFixed(2))).toBeInTheDocument();
  });
});

test("given quoteError should display error", async () => {
  //arrange
  const configObject = {
    currentQuote: null,
    quoteError: generateFakeCryptoError(),
  };

  render(
    <CryptoCheckContext.Provider value={configObject}>
      <QuoteInfoCard />
    </CryptoCheckContext.Provider>
  );

  //assert
  expect(screen.getByTestId("error")).toHaveTextContent(
    `${configObject.quoteError.message}`
  );
});
