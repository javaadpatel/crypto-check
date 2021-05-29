import moment from "moment";
import { Card, Icon, Item, Table, Message } from "semantic-ui-react";
import { useCryptoCheck } from "../../Contexts/CryptoCheck-Context";
import { CryptoQuote } from "../../Models/Api/CryptoQuote";
import { CryptoQuoteError } from "../../Models/Api/CryptoQuoteError";

const QuoteInfoCard = () => {
  const {
    currentQuote,
    quoteError,
  }: { currentQuote: CryptoQuote; quoteError: CryptoQuoteError } =
    useCryptoCheck();

  const renderCryptoInformation = (quote: CryptoQuote) => {
    return (
      <>
        <p>
          <strong>Name:</strong> {quote.name}
        </p>
        <p>
          <strong>Symbol:</strong> {quote.symbol}
        </p>
        <p>
          <strong>Last refreshed at:</strong>{" "}
          {moment(quote.issuedAt).toLocaleString()}
        </p>
      </>
    );
  };

  const renderCryptoPrices = (quotes: { [symbol: string]: number }) => {
    return (
      <>
        <Table basic="very" celled>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Currency</Table.HeaderCell>
              <Table.HeaderCell>Price</Table.HeaderCell>
            </Table.Row>
          </Table.Header>

          <Table.Body>
            {Object.entries(quotes).map((key) => {
              return (
                <Table.Row key={key[0]}>
                  <Table.Cell>{key[0]}</Table.Cell>
                  <Table.Cell>{key[1].toFixed(2)}</Table.Cell>
                </Table.Row>
              );
            })}
          </Table.Body>
        </Table>
      </>
    );
  };

  const renderQuoteError = (error: CryptoQuoteError) => {
    return (
      <Message negative>
        <Message.Header>Sorry, couldn't retrieve your quote.</Message.Header>
        <p>{error.message}</p>
      </Message>
    );
  };

  return (
    <>
      {currentQuote ? (
        <Card fluid>
          <Card.Content header="CryptoCurrency Information" />
          <Card.Content>{renderCryptoInformation(currentQuote)}</Card.Content>
          <Card.Content>
            {renderCryptoPrices(currentQuote.currencyQuotes)}
          </Card.Content>
        </Card>
      ) : (
        <></>
      )}
      {quoteError ? renderQuoteError(quoteError) : <></>}
    </>
  );
};

export default QuoteInfoCard;
