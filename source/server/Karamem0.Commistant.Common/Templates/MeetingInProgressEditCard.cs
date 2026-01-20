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

public static class MeetingInProgressEditCard
{

    public static string Create(MeetingInProgressEditCardData rootData)
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
                          "title": "5 分ごと",
                          "value": "5"
                        },
                        {
                          "title": "10 分ごと",
                          "value": "10"
                        },
                        {
                          "title": "15 分ごと",
                          "value": "15"
                        },
                        {
                          "title": "30 分ごと",
                          "value": "30"
                        },
                        {
                          "title": "60 分ごと",
                          "value": "60"
                        }
                      ],
                      "placeholder": "通知を繰り返す間隔",
                      "label": "スケジュール"
                    },
                    {
                      "type": "Input.Text",
                      "id": "message",
                      "placeholder": "会議中に表示されるメッセージ",
                      "value": "${message}",
                      "isMultiline": true,
                      "label": "メッセージ"
                    },
                    {
                      "type": "Input.Text",
                      "id": "url",
                      "placeholder": "会議中に表示されるリンクの URL",
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
