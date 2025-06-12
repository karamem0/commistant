//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

export interface CommandSettings {
  isOrganizer: boolean,
  meetingStartSchedule: number,
  meetingStartMessage?: string,
  meetingStartUrl?: string,
  meetingEndSchedule: number,
  meetingEndMessage?: string,
  meetingEndUrl?: string,
  meetingRunSchedule: number,
  meetingRunMessage?: string,
  meetingRunUrl?: string
}
