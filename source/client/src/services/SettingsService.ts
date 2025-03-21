//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import React from 'react';

import { useAsyncFn, useError } from 'react-use';
import { CommandSettings } from '../types/Model';
import axios from 'axios';

function getValue(meetingId: string): Promise<CommandSettings> {
  return axios
    .post<CommandSettings>(
      '/api/getSettings',
      {
        channelId: 'msteams',
        meetingId
      }
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

function setValue(meetingId: string, value: CommandSettings): Promise<CommandSettings> {
  return axios
    .post<CommandSettings>(
      '/api/setSettings',
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
