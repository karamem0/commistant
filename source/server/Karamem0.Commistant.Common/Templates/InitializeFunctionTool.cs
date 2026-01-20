//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

namespace Karamem0.Commistant.Templates;

public static class InitializeFunctionTool
{

    public static BinaryData Create()
    {
        return BinaryData.FromString(
            """
              {
                "type": "object",
                "properties": {
                  "type": {
                    "type": "string",
                    "description": "This must be \"初期化\"."
                  }
                }
              }
            """
        );
    }

}
