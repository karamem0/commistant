//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { useIntl } from 'react-intl';

import { useSnackbar } from '../../../common/providers/SnackbarProvider';
import { SnackbarType } from '../../../common/types/Snackbar';
import { mapper } from '../../../mappings/AutoMapperProfile';
import { useTeams } from '../../../providers/TeamsProvider';
import { useGetValue, useSetValue } from '../../../services/PropertyService';
import { Event } from '../../../types/Event';
import { ConversationPropertyFormState } from '../../../types/Form';
import messages from '../messages';

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
          type: SnackbarType.success,
          text: intl.formatMessage(messages.SaveSucceeded)
        });
      } catch (e) {
        setSnackbar({
          type: SnackbarType.error,
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
