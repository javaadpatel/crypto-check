# CryptoCheck

A web application for quickly checking the price of cryptocurrencies in multiple FIAT currencies

![CryptoCheck Quote](/resources/images/cryptocheck.png)

## Index

- [Quickstart](#quickstart)
- [Architecture](#architecture)
- [Setup](#setup)
- [Deploying FloodRunner](#deploying-floodrunner)
  - [1. Deploying Traefik](#1-deploying-traefik)
  - [2. Deploying RabbitMq](#2-deploying-rabbitMq)
  - [3. Deploying MongoDB](#3-deploying-mongoDB)
  - [4. Deploying FloodRunner NestJs API](#4-deploying-floodrunner-nestjs-api)
    - [4.1 Setting up Azure Blob Storage](#4.1-setting-up-azure-blob-storage)
  - [5. Deploying FloodRunner React Web App](#5-deploying-floodRunner-react-web-app)
- [Contributing](#contributing)
- [Reporting Issues](#reporting-issues)

## QuickStart

Enter the cryptocurrency's symbol (eg. BTC for bitcoin) and click `Get quote`.

Validation will be applied on the cryptocurrency symbol to ensure that it meets the requirements for a symbol before being able to get a quote.

The application will then either respond with a quote if the cryptocurrency symbol exists or an error message if the cryptocurrency symbol cannot be found.

## Architecture

The application is composed of a React front-end built in TypeScript and an Azure Functions API built in .Net Core 3.1.

The API's functionality can be seen through the SwaggerUI which will be accessible on `http://localhost:7071/api/swagger/ui` when running locally.

![CryptoCheck SwaggerUI](/resources/images/swagger_ui.jpeg)

## Setup

These are the basic steps to follow when running the application locally.

### Installing the application

The packages for the front-end need to installed. To do this, navigate to the `CryptoCheck.SPA` folder and execute:

> npm install

The packages for the backend need to be installed, for this you will need the dotcore 3.1 framework installed. From the root folder execute:

> dotnet restore CryptoCheck.sln

### Configuring the application

You will need to configure the API with your `Coin Market Cap` and `ExchangeRatesApi` keys. To do this:

- Insert the required API keys into the `local.template.settings.json` file
- Rename the `local.template.settings.json` file to `local.settings.json`

### Running the application

The application can be run using the following steps:

- Run the API using `dotnet run` (from within the root folder)
- Run the front-end using `npm run start` (from within the `CryptoCheck.SPA` folder)

## Testing

## Contributions
