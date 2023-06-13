//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import ReactDOM from 'react-dom/client';

import { ErrorBoundary } from 'react-error-boundary';
import {
  BrowserRouter,
  Route,
  Routes
} from 'react-router-dom';

import { Global } from '@emotion/react';
import ress from 'ress';

import SnackbarProvider from './common/providers/SnackbarProvider';
import Error404Page from './features/error/pages/Error404Page';
import Error500Page from './features/error/pages/Error500Page';
import HomePage from './features/home/pages/HomePage';
import ConfigurePage from './features/tab/pages/ConfigurePage';
import PropertyPage from './features/tab/pages/PropertyPage';
import IntlProvider from './providers/IntlProvider';
import TeamsProvider from './providers/TeamsProvider';
import TelemetryProvider from './providers/TelemetryProvider';
import ThemeProvider from './providers/ThemeProvider';

const element = document.getElementById('root') as HTMLElement;
const root = ReactDOM.createRoot(element);

root.render(
  <React.Fragment>
    <Global styles={ress} />
    <BrowserRouter>
      <TelemetryProvider>
        <IntlProvider>
          <ThemeProvider>
            <ErrorBoundary fallbackRender={(props) => <Error500Page {...props} />}>
              <Routes>
                <Route
                  path="/"
                  element={(
                    <HomePage />
                )} />
                <Route
                  path="/tab/configure"
                  element={(
                    <ConfigurePage />
                )} />
                <Route
                  path="/tab/property"
                  element={(
                    <TeamsProvider>
                      <SnackbarProvider>
                        <PropertyPage />
                      </SnackbarProvider>
                    </TeamsProvider>
                )} />
                <Route
                  element={<Error404Page />}
                  path="*" />
              </Routes>
            </ErrorBoundary>
          </ThemeProvider>
        </IntlProvider>
      </TelemetryProvider>
    </BrowserRouter>
  </React.Fragment>
);
