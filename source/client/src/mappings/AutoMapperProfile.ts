//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import {
  createMap,
  createMapper,
  forMember,
  mapFrom
} from '@automapper/core';
import { PojosMetadataMap, pojos } from '@automapper/pojos';
import { CommandSettingsFormState } from '../types/Form';
import { CommandSettings } from '../types/Model';

export const mapper = createMapper({
  strategyInitializer: pojos()
});

PojosMetadataMap.create<CommandSettings>('CommandSettings', {
  meetingEndingMessage: String,
  meetingEndingSchedule: Number,
  meetingEndingUrl: String,
  meetingInProgressMessage: String,
  meetingInProgressSchedule: Number,
  meetingInProgressUrl: String,
  meetingStartedMessage: String,
  meetingStartedSchedule: Number,
  meetingStartedUrl: String
});

PojosMetadataMap.create<CommandSettingsFormState>('CommandSettingsFormState', {
  meetingEndingMessage: String,
  meetingEndingSchedule: String,
  meetingEndingUrl: String,
  meetingInProgressMessage: String,
  meetingInProgressSchedule: String,
  meetingInProgressUrl: String,
  meetingStartedMessage: String,
  meetingStartedSchedule: String,
  meetingStartedUrl: String
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
