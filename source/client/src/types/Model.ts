//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

export interface CommandSettings {
  isOrganizer: boolean,
  meetingEndingMessage?: string,
  meetingEndingSchedule: number,
  meetingEndingUrl?: string,
  meetingInProgressMessage?: string,
  meetingInProgressSchedule: number,
  meetingInProgressUrl?: string,
  meetingStartedMessage?: string,
  meetingStartedSchedule: number,
  meetingStartedUrl?: string
}
