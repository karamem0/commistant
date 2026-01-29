//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { app, pages } from '@microsoft/teams-js';
import { useIntl } from 'react-intl';
import messages from '../messages';

import Presenter from './ConfigurePage.presenter';

function ConfigurePage() {

  const intl = useIntl();

  const handleSave = React.useCallback(async (event: pages.config.SaveEvent) => {
    try {
      await pages.config.setConfig({
        contentUrl: `${window.origin}/tab/content`,
        entityId: '55da67fc-cfa4-481c-a77f-de2c0b6deaed',
        suggestedDisplayName: intl.formatMessage(messages.AppTitle),
        websiteUrl: window.origin
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
