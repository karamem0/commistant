//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import axios from 'axios';

import { ConversationProperty } from '../types/Model';

export function getValue(meetingId: string): Promise<ConversationProperty> {
  return axios
    .post<ConversationProperty>(
      '/api/getproperty',
      {
        channelId: 'msteams',
        meetingId
      }
    )
    .then((response) => response.data);
}

export function setValue(meetingId: string, value: ConversationProperty): Promise<ConversationProperty> {
  return axios
    .post<ConversationProperty>(
      '/api/setproperty',
      {
        channelId: 'msteams',
        meetingId,
        ...value
      }
    )
    .then((response) => response.data);
}
