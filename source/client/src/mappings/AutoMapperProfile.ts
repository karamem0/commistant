//
// Copyright (c) 2022-2026 karamem0
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
import { CommandSettings } from '../types/Model';
import { CommandSettingsFormState } from '../types/Form';

export const mapper = createMapper({
  strategyInitializer: pojos()
});

PojosMetadataMap.create<CommandSettings>('CommandSettings', {
  meetingStartedSchedule: Number,
  meetingStartedMessage: String,
  meetingStartedUrl: String,
  meetingEndingSchedule: Number,
  meetingEndingMessage: String,
  meetingEndingUrl: String,
  meetingInProgressSchedule: Number,
  meetingInProgressMessage: String,
  meetingInProgressUrl: String
});

PojosMetadataMap.create<CommandSettingsFormState>('CommandSettingsFormState', {
  meetingStartedSchedule: String,
  meetingStartedMessage: String,
  meetingStartedUrl: String,
  meetingEndingSchedule: String,
  meetingEndingMessage: String,
  meetingEndingUrl: String,
  meetingInProgressSchedule: String,
  meetingInProgressMessage: String,
  meetingInProgressUrl: String
});

createMap<CommandSettings, CommandSettingsFormState>(
  mapper,
  'CommandSettings',
  'CommandSettingsFormState',
  forMember(
    (target) => target.meetingStartedSchedule,
    mapFrom((source) => String(source.meetingStartedSchedule))),
  forMember(
    (target) => target.meetingEndingSchedule,
    mapFrom((source) => String(source.meetingEndingSchedule))),
  forMember(
    (target) => target.meetingInProgressSchedule,
    mapFrom((source) => String(source.meetingInProgressSchedule)))
);

createMap<CommandSettingsFormState, CommandSettings>(
  mapper,
  'CommandSettingsFormState',
  'CommandSettings',
  forMember(
    (target) => target.meetingStartedSchedule,
    mapFrom((source) => Number(source.meetingStartedSchedule))),
  forMember(
    (target) => target.meetingEndingSchedule,
    mapFrom((source) => Number(source.meetingEndingSchedule))),
  forMember(
    (target) => target.meetingInProgressSchedule,
    mapFrom((source) => Number(source.meetingInProgressSchedule)))
);
