//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { useAsyncFn, useError } from 'react-use';
import { ConversationProperty } from '../types/Model';
import axios from 'axios';

function getValue(meetingId: string): Promise<ConversationProperty> {
  return axios
    .get<ConversationProperty>(
      `/api/property?&channelId=msteams&meetingId=${meetingId}`
    )
    .then((response) => response.data);
}

type GetValueFunction = typeof getValue;

export const useGetValue = (): GetValueFunction => {

  const dispatchError = useError();
  const [ state, fetch ] = useAsyncFn(getValue);

  React.useEffect(() => {
    if (!state.error) {
      return;
    }
    dispatchError(state.error);
  }, [
    dispatchError,
    state
  ]);

  return fetch;

};

function setValue(meetingId: string, value: ConversationProperty): Promise<ConversationProperty> {
  return axios
    .put<ConversationProperty>(
      '/api/property',
      {
        channelId: 'msteams',
        meetingId,
        ...value
      }
    )
    .then((response) => response.data);
}

type SetValueFunction = typeof setValue;

export const useSetValue = (): SetValueFunction => {

  const dispatchError = useError();
  const [ state, fetch ] = useAsyncFn(setValue);

  React.useEffect(() => {
    if (!state.error) {
      return;
    }
    dispatchError(state.error);
  }, [
    dispatchError,
    state
  ]);

  return fetch;

};
