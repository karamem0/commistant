//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import { defineMessages } from 'react-intl';
import parentMessages from '../messages';

const messages = {
  ...parentMessages,
  ...defineMessages({
    InTime: { defaultMessage: '予定時刻' },
    MeetingEnding: { defaultMessage: '会議終了前' },
    MeetingInProgress: { defaultMessage: '会議中' },
    MeetingStarted: { defaultMessage: '会議開始後' },
    Message: { defaultMessage: 'メッセージ' },
    Minutes: { defaultMessage: '{value} 分' },
    MinutesAfter: { defaultMessage: '{value} 分後' },
    MinutesBefore: { defaultMessage: '{value} 分前' },
    None: { defaultMessage: 'なし' },
    Save: { defaultMessage: '保存' },
    SaveFailed: { defaultMessage: '設定を保存できませんでした。' },
    SaveSucceeded: { defaultMessage: '設定を保存しました。' },
    Schedule: { defaultMessage: 'スケジュール' },
    Url: { defaultMessage: 'URL' }
  })
};

export default messages;
