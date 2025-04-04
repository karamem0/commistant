//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { useGetValue, useSetValue } from '../../../services/SettingsService';
import { CommandSettingsFormState } from '../../../types/Form';
import { Event } from '../../../types/Event';
import { mapper } from '../../../mappings/AutoMapperProfile';
import messages from '../messages';
import { useIntl } from 'react-intl';
import { useSnackbar } from '../../../common/providers/SnackbarProvider';
import { useTeams } from '../../../providers/TeamsProvider';

import Presenter from './ContentPage.presenter';

function ContentPage() {

  const intl = useIntl();
  const { context } = useTeams();
  const { setSnackbar } = useSnackbar();
  const getValue = useGetValue();
  const setValue = useSetValue();

  const [ loading, setLoading ] = React.useState<boolean>(false);
  const [ disabled, setDisabled ] = React.useState<boolean>(true);
  const [ state, setState ] = React.useState<CommandSettingsFormState>();

  const handleSubmit = React.useCallback((_?: Event, data?: CommandSettingsFormState) => {
    (async () => {
      if (!data) {
        throw new Error();
      }
      try {
        setLoading(true);
        const conversationId = context?.chat?.id;
        if (!conversationId) {
          throw new Error();
        }
        const value = await setValue(
          conversationId,
          mapper.map(
            data,
            'CommandSettingsFormState',
            'CommandSettings'
          ));
        setState(mapper.map(
          value,
          'CommandSettings',
          'CommandSettingsFormState'
        ));
        setSnackbar({
          intent: 'success',
          text: intl.formatMessage(messages.SaveSucceeded)
        });
      } catch {
        setSnackbar({
          intent: 'error',
          text: intl.formatMessage(messages.SaveFailed)
        });
      } finally {
        setLoading(false);
      }
    })();
  }, [
    context,
    intl,
    setValue,
    setSnackbar
  ]);

  React.useEffect(() => {
    (async () => {
      const conversationId = context?.chat?.id;
      if (!conversationId) {
        throw new Error();
      }
      const value = await getValue(conversationId);
      setDisabled(!value.isOrganizer);
      setState(mapper.map(
        value,
        'CommandSettings',
        'CommandSettingsFormState'
      ));
    })();
  }, [
    context,
    getValue
  ]);

  return (
    <Presenter
      disabled={disabled}
      loading={loading}
      value={state}
      onSubmit={handleSubmit} />
  );

}

export default ContentPage;
