//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards.Templating;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Types;

namespace Karamem0.Commistant.Templates;

public static class MeetingStartedEditCard
{

    public static string Create(MeetingStartedEditCardData rootData)
    {
        var template = new AdaptiveCardTemplate(
            $$"""
                {
                  "type": "AdaptiveCard",
                  "version": "1.3",
                  "body": [
                    {
                      "type": "Input.ChoiceSet",
                      "id": "schedule",
                      "value": "${schedule}",
                      "style": "compact",
                      "isMultiSelect": false,
                      "choices": [
                        {
                          "title": "なし",
                          "value": "-1"
                        },
                        {
                          "title": "予定時刻",
                          "value": "0"
                        },
                        {
                          "title": "5 分後",
                          "value": "5"
                        },
                        {
                          "title": "10 分後",
                          "value": "10"
                        },
                        {
                          "title": "15 分後",
                          "value": "15"
                        }
                      ],
                      "placeholder": "通知を表示する時間",
                      "label": "スケジュール"
                    },
                    {
                      "type": "Input.Text",
                      "id": "message",
                      "placeholder": "会議開始後に表示されるメッセージ",
                      "value": "${message}",
                      "isMultiline": true,
                      "label": "メッセージ"
                    },
                    {
                      "type": "Input.Text",
                      "id": "url",
                      "placeholder": "会議開始後に表示されるリンクの URL",
                      "value": "${url}",
                      "style": "url",
                      "label": "URL"
                    }
                  ],
                  "actions": [
                    {
                      "type": "Action.Submit",
                      "id": "submit",
                      "data": {
                        "button": "{{ButtonTypes.Submit}}"
                      },
                      "title": "保存"
                    },
                    {
                      "type": "Action.Submit",
                      "id": "cancel",
                      "data": {
                        "button": "{{ButtonTypes.Cancel}}"
                      },
                      "title": "キャンセル"
                    }
                  ]
                }
              """
        );
        return template.Expand(rootData);
    }

}
