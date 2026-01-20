//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards.Templating;
using Karamem0.Commistant.Models;

namespace Karamem0.Commistant.Templates;

public static class MeetingStartedViewCard
{

    public static string Create(MeetingStartedViewCardData rootData)
    {
        var template = new AdaptiveCardTemplate(
            """
              {
                "type": "AdaptiveCard",
                "version": "1.3",
                "body": [
                  {
                    "type": "ColumnSet",
                    "columns": [
                      {
                        "type": "Column",
                        "width": "90px",
                        "items": [
                          {
                            "type": "TextBlock",
                            "weight": "bolder",
                            "text": "スケジュール"
                          }
                        ]
                      },
                      {
                        "type": "Column",
                        "width": "stretch",
                        "items": [
                          {
                            "type": "TextBlock",
                            "text": "${schedule}"
                          }
                        ]
                      }
                    ]
                  },
                  {
                    "type": "ColumnSet",
                    "columns": [
                      {
                        "type": "Column",
                        "width": "90px",
                        "items": [
                          {
                            "type": "TextBlock",
                            "weight": "bolder",
                            "text": "メッセージ"
                          }
                        ]
                      },
                      {
                        "type": "Column",
                        "width": "stretch",
                        "items": [
                          {
                            "type": "TextBlock",
                            "text": "${message}",
                            "wrap": true
                          }
                        ]
                      }
                    ]
                  },
                  {
                    "type": "ColumnSet",
                    "columns": [
                      {
                        "type": "Column",
                        "width": "90px",
                        "items": [
                          {
                            "type": "TextBlock",
                            "weight": "bolder",
                            "text": "URL"
                          }
                        ]
                      },
                      {
                        "type": "Column",
                        "width": "stretch",
                        "items": [
                          {
                            "type": "TextBlock",
                            "text": "${url}"
                          }
                        ]
                      }
                    ]
                  },
                  {
                    "type": "Image",
                    "size": "large",
                    "url": "${if(length(qrCode) > 0, 'data:image/png;base64,' & qrCode, '')}",
                    "altText": "${url}",
                    "$when": "${length(qrCode) > 0}"
                  }
                ],
                "actions": [
                  {
                    "type": "Action.OpenUrl",
                    "url": "${url}",
                    "title": "URL を開く",
                    "$when": "${length(url) > 0}"
                  }
                ]
              }
            """
        );
        return template.Expand(rootData);
    }

}
