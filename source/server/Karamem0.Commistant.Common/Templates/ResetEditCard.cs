//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards;
using AdaptiveCards.Templating;
using Karamem0.Commistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Templates;

public static class ResetEditCard
{

    public static AdaptiveCard Create(ResetEditCardData rootData)
    {
        var template = new AdaptiveCardTemplate(
            """
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
                      "button": "Yes"
                    },
                    "title": "はい"
                  },
                  {
                    "type": "Action.Submit",
                    "id": "no",
                    "data": {
                      "button": "No"
                    },
                    "title": "いいえ"
                  }
                ]
              }
            """
        );
        var card = AdaptiveCard.FromJson(template.Expand(rootData));
        return card.Card;
    }

}
