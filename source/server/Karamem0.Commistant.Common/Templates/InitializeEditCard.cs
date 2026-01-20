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

public static class InitializeEditCard
{

    public static string Create(InitializeCardData rootData)
    {
        var template = new AdaptiveCardTemplate(
            $$"""
                {
                  "type": "AdaptiveCard",
                  "version": "1.3",
                  "body": [
                    {
                      "type": "TextBlock",
                      "id": "message",
                      "text": "すべての設定を初期化します。よろしいですか？",
                      "wrap": true
                    }
                  ],
                  "actions": [
                    {
                      "type": "Action.Submit",
                      "id": "yes",
                      "data": {
                        "button": "{{ButtonTypes.Yes}}"
                      },
                      "title": "はい"
                    },
                    {
                      "type": "Action.Submit",
                      "id": "no",
                      "data": {
                        "button": "{{ButtonTypes.No}}"
                      },
                      "title": "いいえ"
                    }
                  ]
                }
              """
        );
        return template.Expand(rootData);
    }

}
