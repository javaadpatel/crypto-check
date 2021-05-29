import React from 'react';
import ReactDOM from 'react-dom';
import { Router } from "react-router-dom";
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import history from "./Utils/history";
import { CryptoCheckProvider } from './Contexts/CryptoCheck-Context';

ReactDOM.render(
  <CryptoCheckProvider>
    <Router history={history}>
          <App />
    </Router>
  </CryptoCheckProvider>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
