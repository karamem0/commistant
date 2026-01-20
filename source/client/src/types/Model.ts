//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

export interface CommandSettings {
  isOrganizer: boolean,
  meetingStartedSchedule: number,
  meetingStartedMessage?: string,
  meetingStartedUrl?: string,
  meetingEndingSchedule: number,
  meetingEndingMessage?: string,
  meetingEndingUrl?: string,
  meetingInProgressSchedule: number,
  meetingInProgressMessage?: string,
  meetingInProgressUrl?: string
}
