//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { useGetValue, useSetValue } from '../../../services/PropertyService';
import { ConversationPropertyFormState } from '../../../types/Form';
import { Event } from '../../../types/Event';
import { mapper } from '../../../mappings/AutoMapperProfile';
import messages from '../messages';
import { useIntl } from 'react-intl';
import { useSnackbar } from '../../../common/providers/SnackbarProvider';
import { useTeams } from '../../../providers/TeamsProvider';

import Presenter from './PropertyPage.presenter';

function PropertyPage() {

  const intl = useIntl();
  const { context } = useTeams();
  const { setSnackbar } = useSnackbar();
  const getValue = useGetValue();
  const setValue = useSetValue();

  const [ disabled, setDisabled ] = React.useState<boolean>(true);
  const [ state, setState ] = React.useState<ConversationPropertyFormState>();

  const handleSubmit = React.useCallback((_?: Event, data?: ConversationPropertyFormState) => {
    (async () => {
      if (!data) {
        throw new Error();
      }
      try {
        const conversationId = context?.chat?.id;
        if (!conversationId) {
          throw new Error();
        }
        const value = await setValue(
          conversationId,
          mapper.map(
            data,
            'ConversationPropertyFormState',
            'ConversationProperty'
          ));
        setState(mapper.map(
          value,
          'ConversationProperty',
          'ConversationPropertyFormState'
        ));
        setSnackbar({
          intent: 'success',
          text: intl.formatMessage(messages.SaveSucceeded)
        });
      } catch (e) {
        setSnackbar({
          intent: 'error',
          text: intl.formatMessage(messages.SaveFailed)
        });
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
        'ConversationProperty',
        'ConversationPropertyFormState'
      ));
    })();
  }, [
    context,
    getValue
  ]);

  return (
    <Presenter
      disabled={disabled}
      value={state}
      onSubmit={handleSubmit} />
  );

}

export default PropertyPage;
