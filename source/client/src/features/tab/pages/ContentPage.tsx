//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { useIntl } from 'react-intl';
import { useToast } from '../../../common/providers/ToastProvider';
import { mapper } from '../../../mappings/AutoMapperProfile';
import { useTeams } from '../../../providers/TeamsProvider';
import { useGetValue, useSetValue } from '../../../services/SettingsService';
import { ArgumentNullError, DependencyNullError } from '../../../types/Errot';
import { Event } from '../../../types/Event';
import { CommandSettingsFormState } from '../../../types/Form';
import messages from '../messages';

import Presenter from './ContentPage.presenter';

function ContentPage() {

  const intl = useIntl();
  const { context } = useTeams();
  const dispatchToast = useToast();
  const getValue = useGetValue();
  const setValue = useSetValue();

  const [ loading, setLoading ] = React.useState<boolean>(false);
  const [ disabled, setDisabled ] = React.useState<boolean>(true);
  const [ state, setState ] = React.useState<CommandSettingsFormState>();

  const handleSubmit = React.useCallback((_?: Event, data?: CommandSettingsFormState) => {
    (async () => {
      if (data == null) {
        throw new ArgumentNullError('data');
      }
      try {
        setLoading(true);
        const conversationId = context?.chat?.id;
        if (conversationId == null) {
          throw new DependencyNullError('conversationId');
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
        dispatchToast(intl.formatMessage(messages.SaveSucceeded), 'success');
      } catch (error) {
        if (error instanceof Error) {
          dispatchToast(intl.formatMessage(messages.SaveFailed), 'error');
        } else {
          throw error;
        }
      } finally {
        setLoading(false);
      }
    })();
  }, [
    context,
    intl,
    dispatchToast,
    setValue
  ]);

  React.useEffect(() => {
    (async () => {
      const conversationId = context?.chat?.id;
      if (conversationId == null) {
        throw new DependencyNullError('conversationId');
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
