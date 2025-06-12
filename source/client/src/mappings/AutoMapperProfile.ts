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
import { CommandSettings } from '../types/Model';
import { CommandSettingsFormState } from '../types/Form';

export const mapper = createMapper({
  strategyInitializer: pojos()
});

PojosMetadataMap.create<CommandSettings>('CommandSettings', {
  meetingStartSchedule: Number,
  meetingStartMessage: String,
  meetingStartUrl: String,
  meetingEndSchedule: Number,
  meetingEndMessage: String,
  meetingEndUrl: String,
  meetingRunSchedule: Number,
  meetingRunMessage: String,
  meetingRunUrl: String
});

PojosMetadataMap.create<CommandSettingsFormState>('CommandSettingsFormState', {
  meetingStartSchedule: String,
  meetingStartMessage: String,
  meetingStartUrl: String,
  meetingEndSchedule: String,
  meetingEndMessage: String,
  meetingEndUrl: String,
  meetingRunSchedule: String,
  meetingRunMessage: String,
  meetingRunUrl: String
});

createMap<CommandSettings, CommandSettingsFormState>(
  mapper,
  'CommandSettings',
  'CommandSettingsFormState',
  forMember(
    (target) => target.meetingStartSchedule,
    mapFrom((source) => String(source.meetingStartSchedule))),
  forMember(
    (target) => target.meetingEndSchedule,
    mapFrom((source) => String(source.meetingEndSchedule))),
  forMember(
    (target) => target.meetingRunSchedule,
    mapFrom((source) => String(source.meetingRunSchedule)))
);

createMap<CommandSettingsFormState, CommandSettings>(
  mapper,
  'CommandSettingsFormState',
  'CommandSettings',
  forMember(
    (target) => target.meetingStartSchedule,
    mapFrom((source) => Number(source.meetingStartSchedule))),
  forMember(
    (target) => target.meetingEndSchedule,
    mapFrom((source) => Number(source.meetingEndSchedule))),
  forMember(
    (target) => target.meetingRunSchedule,
    mapFrom((source) => Number(source.meetingRunSchedule)))
);
