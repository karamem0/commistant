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

public static class MeetingStartedNotifyCard
{

    public static string Create(MeetingStartedNotifyCardData rootData)
    {
        var template = new AdaptiveCardTemplate(
            """
            {
              "type": "AdaptiveCard",
              "version": "1.3",
              "body": [
                {
                  "type": "TextBlock",
                  "text": "${message}",
                  "wrap": true,
                  "$when": "${length(message) > 0}"
                },
                {
                  "type": "Image",
                  "size": "stretch",
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
