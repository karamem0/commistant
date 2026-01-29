//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import ReactDOM from 'react-dom/client';

import { Global } from '@emotion/react';
import { ErrorBoundary } from 'react-error-boundary';
import {
  BrowserRouter,
  Outlet,
  Route,
  Routes
} from 'react-router-dom';
import * as ress from 'ress';
import TeamsInitializer from './common/components/TeamsInitializer';
import ToastProvider from './common/providers/ToastProvider';
import Error404Page from './features/error/pages/Error404Page';
import Error500Page from './features/error/pages/Error500Page';
import HomePage from './features/home/pages/HomePage';
import ConfigurePage from './features/tab/pages/ConfigurePage';
import ContentPage from './features/tab/pages/ContentPage';
import IntlProvider from './providers/IntlProvider';
import TeamsProvider from './providers/TeamsProvider';
import TelemetryProvider from './providers/TelemetryProvider';
import ThemeProvider from './providers/ThemeProvider';

const element = document.getElementById('root') as HTMLElement;
const root = ReactDOM.createRoot(element);

root.render(
  <React.StrictMode>
    <Global styles={ress} />
    <TelemetryProvider>
      <BrowserRouter>
        <TeamsInitializer>
          {
            (inTeams) => (
              <IntlProvider>
                <ThemeProvider>
                  <ErrorBoundary fallbackRender={(props) => <Error500Page error={props.error as Error} />}>
                    <Routes>
                      {
                        inTeams ? (
                          <Route
                            element={(
                              <TeamsProvider>
                                <ToastProvider>
                                  <Outlet />
                                </ToastProvider>
                              </TeamsProvider>
                            )}>
                            <Route
                              path="/tab/configure"
                              element={(
                                <ConfigurePage />
                              )} />
                            <Route
                              path="/tab/content"
                              element={(
                                <ContentPage />
                              )} />
                          </Route>
                        ) : null
                      }
                      <Route
                        path="/"
                        element={(
                          <HomePage />
                        )} />
                      <Route
                        element={<Error404Page />}
                        path="*" />
                    </Routes>
                  </ErrorBoundary>
                </ThemeProvider>
              </IntlProvider>
            )
          }
        </TeamsInitializer>
      </BrowserRouter>
    </TelemetryProvider>
  </React.StrictMode>
);
