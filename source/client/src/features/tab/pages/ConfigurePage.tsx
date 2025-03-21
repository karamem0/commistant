//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { app, pages } from '@microsoft/teams-js';
import Presenter from './ConfigurePage.presenter';
import messages from '../messages';
import { useIntl } from 'react-intl';

function ConfigurePage() {

  const intl = useIntl();

  const handleSave = React.useCallback(async (event: pages.config.SaveEvent) => {
    try {
      await pages.config.setConfig({
        websiteUrl: window.origin,
        contentUrl: `${window.origin}/tab/content`,
        entityId: '55da67fc-cfa4-481c-a77f-de2c0b6deaed',
        suggestedDisplayName: intl.formatMessage(messages.AppName)
      });
      event.notifySuccess();
    } catch (error) {
      event.notifyFailure(error instanceof Error ? error.message : Object.prototype.toString.call(error));
    }
  }, [
    intl
  ]);

  React.useEffect(() => {
    (async () => {
      await app.initialize();
      pages.config.registerOnSaveHandler(handleSave);
      pages.config.setValidityState(true);
    })();
  }, [
    handleSave
  ]);

  return (
    <Presenter />
  );

}

export default ConfigurePage;
