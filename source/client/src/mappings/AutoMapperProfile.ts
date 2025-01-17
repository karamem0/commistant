//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import { PojosMetadataMap, pojos } from '@automapper/pojos';
import {
  createMap,
  createMapper,
  forMember,
  mapFrom
} from '@automapper/core';
import { ConversationProperty } from '../types/Model';
import { ConversationPropertyFormState } from '../types/Form';

export const mapper = createMapper({
  strategyInitializer: pojos()
});

PojosMetadataMap.create<ConversationProperty>('ConversationProperty', {
  startMeetingSchedule: Number,
  startMeetingMessage: String,
  startMeetingUrl: String,
  endMeetingSchedule: Number,
  endMeetingMessage: String,
  endMeetingUrl: String,
  inMeetingSchedule: Number,
  inMeetingMessage: String,
  inMeetingUrl: String
});

PojosMetadataMap.create<ConversationPropertyFormState>('ConversationPropertyFormState', {
  startMeetingSchedule: String,
  startMeetingMessage: String,
  startMeetingUrl: String,
  endMeetingSchedule: String,
  endMeetingMessage: String,
  endMeetingUrl: String,
  inMeetingSchedule: String,
  inMeetingMessage: String,
  inMeetingUrl: String
});

createMap<ConversationProperty, ConversationPropertyFormState>(
  mapper,
  'ConversationProperty',
  'ConversationPropertyFormState',
  forMember(
    (target) => target.startMeetingSchedule,
    mapFrom((source) => String(source.startMeetingSchedule))),
  forMember(
    (target) => target.endMeetingSchedule,
    mapFrom((source) => String(source.endMeetingSchedule))),
  forMember(
    (target) => target.inMeetingSchedule,
    mapFrom((source) => String(source.inMeetingSchedule)))
);

createMap<ConversationPropertyFormState, ConversationProperty>(
  mapper,
  'ConversationPropertyFormState',
  'ConversationProperty',
  forMember(
    (target) => target.startMeetingSchedule,
    mapFrom((source) => Number(source.startMeetingSchedule))),
  forMember(
    (target) => target.endMeetingSchedule,
    mapFrom((source) => Number(source.endMeetingSchedule))),
  forMember(
    (target) => target.inMeetingSchedule,
    mapFrom((source) => Number(source.inMeetingSchedule)))
);
