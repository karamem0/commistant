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

public static class InitializeViewCard
{

    public static string Create(InitializeViewCardData rootData)
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
        return template.Expand(rootData);
    }

}
