//import dependencies
import React from "react";

//import react-testing methods
import {
  render,
  fireEvent,
  waitFor,
  screen,
  within,
} from "@testing-library/react";
import userEvent from "@testing-library/user-event";

//addd custom jest matchers from jest-dom
import "@testing-library/jest-dom/extend-expect";

//import component to test
import QuoteForm from "./QuoteForm";
import { CryptoCheckContext } from "../../../Contexts/CryptoCheck-Context";

test("form should render correctly", async () => {
  //arrange
  const configObject = {
    getQuote: () => {
      console.log("called getQuote");
    },
  };

  render(
    <CryptoCheckContext.Provider value={configObject}>
      <QuoteForm />
    </CryptoCheckContext.Provider>
  );

  //assert
  expect(screen.getByRole("button")).toBeInTheDocument(); //there should be a submit button
  expect(screen.getByRole("textbox")).toBeInTheDocument(); //there should be an textbox field

  expect(screen.queryByTestId("symbolError")).not.toBeInTheDocument(); //there should be no error
});

test("given incorrect user input should render error and disable submit button", async () => {
  //arrange
  const configObject = {
    getQuote: () => {
      console.log("called getQuote");
    },
  };

  render(
    <CryptoCheckContext.Provider value={configObject}>
      <QuoteForm />
    </CryptoCheckContext.Provider>
  );

  userEvent.type(screen.getByRole("textbox"), "BTCCC22C");
  userEvent.click(screen.getByRole("button"));

  //assert
  await waitFor(() => {
    expect(screen.getByTestId("symbolError")).toBeInTheDocument();
    expect(screen.getByRole("button")).toBeDisabled();
  });
});
