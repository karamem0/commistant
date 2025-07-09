//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Templates;

public static class ResetFunctionTool
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
