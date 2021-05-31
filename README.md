# CryptoCheck

A web application for quickly checking the price of cryptocurrencies in multiple FIAT currencies

![CryptoCheck Quote](/resources/images/cryptocheck.png)

## Index

- [Quickstart](#quickstart)
- [Architecture](#architecture)
- [Setup](#setup)
- [Running the application](#running-the-application)
- [Testing](#testing)
- [Contributing](#contributing)

## QuickStart

Enter the cryptocurrency's symbol (eg. BTC for bitcoin) and click `Get quote`.

Validation will be applied on the cryptocurrency symbol to ensure that it meets the requirements for a symbol before being able to get a quote.

The application will then either respond with a quote if the cryptocurrency symbol exists or an error message if the cryptocurrency symbol cannot be found.

## Architecture

The application is composed of a React front-end built in TypeScript and an Azure Functions API built in .Net Core 3.1.

The API's functionality can be seen through the SwaggerUI which will be accessible on `http://localhost:7071/api/swagger/ui` when running locally.

![CryptoCheck SwaggerUI](/resources/images/swagger_ui.jpeg)

## Setup

These are the basic steps to follow when configuring the application locally.

### Installing the application

The packages for the front-end need to installed. To do this, navigate to the `CryptoCheck.SPA` folder and execute:

> npm install

The backend need to be built, for this you will need the dotnet core 3.1 framework installed. From the root folder execute:

> dotnet build CryptoCheck.sln

### Configuring the application

You will need to configure the API with your `CoinMarketCap` and `ExchangeRatesApi` keys. To do this:

- Insert the required API keys into the `local.template.settings.json` file
- Rename the `local.template.settings.json` file to `local.settings.json`

## Running the application

The application can be run using the following steps:

- Run the API from within an IDE or using the [Azure Functions Core tools](https://github.com/Azure/azure-functions-core-tools) execute the command `func start` (from within the `CryptoCheck.API` folder)
- Run the front-end using `npm run start` (from within the `CryptoCheck.SPA` folder)

## Testing

The API tests can be run by executing (from the root folder):

> dotnet test CryptoCheck.sln

The front-end tests can be run by executing( from the `CryptoCheck.SPA` folder):

> npm run test

## Contributions

Any contributions to this project are welcomed. Please open a PR or an issue if you need help using the project.
