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

public static class ResetViewCard
{

    public static AdaptiveCard Create(ResetViewCardData rootData)
    {
        var template = new AdaptiveCardTemplate(
            """
              {
                "type": "AdaptiveCard",
                "version": "1.3",
                "body": [
                  {
                    "type": "FactSet",
                    "facts": [
                      {
                        "title": "応答",
                        "value": "${value}"
                      }
                    ]
                  }
                ]
              }
            """
        );
        var card = AdaptiveCard.FromJson(template.Expand(rootData));
        return card.Card;
    }

}
