//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

import { defineMessages } from 'react-intl';

const messages = defineMessages({
  AppName: { defaultMessage: 'Commistant' },
  AppDescription: { defaultMessage: 'Commistant は Microsoft Teams 会議によるコミュニティ イベントをサポートするアシスタント ボットです。会議の開始時、終了時、または会議中に定型のメッセージ通知を送信します。通知にはテキストおよび QR コードつきの URL を添付することができます。' },
  EndMeeting: { defaultMessage: '会議終了前' },
  InMeeting: { defaultMessage: '会議中' },
  InTime: { defaultMessage: '予定時刻' },
  Message: { defaultMessage: 'メッセージ' },
  Minutes: { defaultMessage: '{value} 分' },
  MinutesAfter: { defaultMessage: '{value} 分後' },
  MinutesBefore: { defaultMessage: '{value} 分前' },
  None: { defaultMessage: 'なし' },
  Save: { defaultMessage: '保存' },
  SaveFailed: { defaultMessage: '設定を保存できませんでした。' },
  SaveSucceeded: { defaultMessage: '設定を保存しました。' },
  Schedule: { defaultMessage: 'スケジュール' },
  StartMeeting: { defaultMessage: '会議開始前' },
  Url: { defaultMessage: 'URL' }
});

export default messages;
