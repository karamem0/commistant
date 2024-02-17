//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { useIntl } from 'react-intl';

import { app, pages } from '@microsoft/teams-js';

import messages from '../messages';

import Presenter from './ConfigurePage.presenter';

function ConfigurePage() {

  const intl = useIntl();

  React.useEffect(() => {
    (async () => {
      await app.initialize();
      pages.config.registerOnSaveHandler((e: pages.config.SaveEvent) => {
        pages.config
          .setConfig({
            websiteUrl: window.origin,
            contentUrl: `${window.origin}/tab/property`,
            entityId: '55da67fc-cfa4-481c-a77f-de2c0b6deaed',
            suggestedDisplayName: intl.formatMessage(messages.AppName)
          })
          .then(() => e.notifySuccess())
          .catch((error) => e.notifyFailure(error));
      });
      pages.config.setValidityState(true);
    })();
  }, [
    intl
  ]);

  return (
    <Presenter />
  );

}

export default ConfigurePage;
