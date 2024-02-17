//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

export interface ConversationProperty {
  isOrganizer: boolean,
  startMeetingSchedule: number,
  startMeetingMessage?: string,
  startMeetingUrl?: string,
  endMeetingSchedule: number,
  endMeetingMessage?: string,
  endMeetingUrl?: string,
  inMeetingSchedule: number,
  inMeetingMessage?: string,
  inMeetingUrl?: string
}
