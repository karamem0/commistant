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
import { getValue, setValue } from '../../../services/PropertyService';
import { Event } from '../../../types/Event';
import { ConversationPropertyFormState } from '../../../types/Form';
import messages from '../messages';

import Presenter from './PropertyPage.presenter';

function PropertyPage() {

  const intl = useIntl();
  const { context } = useTeams();
  const { setSnackbar } = useSnackbar();

  const [ state, setState ] = React.useState<ConversationPropertyFormState>();

  const handleSubmit = React.useCallback(async (_?: Event, data?: ConversationPropertyFormState) => {
    if (!data) {
      throw new Error();
    }
    try {
      const conversationId = context?.chat?.id;
      if (!conversationId) {
        throw new Error();
      }
      await setValue(
        conversationId,
        mapper.map(
          data,
          'ConversationPropertyFormState',
          'ConversationProperty'
        ));
      setState(data);
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
  }, [
    context,
    intl,
    setSnackbar
  ]);

  React.useEffect(() => {
    (async () => {
      const conversationId = context?.chat?.id;
      if (!conversationId) {
        throw new Error();
      }
      setState(mapper.map(
        await getValue(conversationId),
        'ConversationProperty',
        'ConversationPropertyFormState'
      ));
    })();
  }, [
    context
  ]);

  return (
    <Presenter
      value={state}
      onSubmit={handleSubmit} />
  );

}

export default PropertyPage;
