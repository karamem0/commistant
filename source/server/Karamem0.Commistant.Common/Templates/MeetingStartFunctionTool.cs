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

public static class MeetingStartFunctionTool
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
                    "description": "This must be \"会議開始後\"."
                  },
                  "value": {
                    "type": "object",
                    "description": "The settings about message to send after the start of the meeting.",
                    "properties": {
                      "enabled": {
                        "type": "boolean",
                        "description": "Whether to send the message."
                      },
                      "schedule": {
                        "type": "integer",
                        "description": "The frequency in minutes."
                      },
                      "message": {
                        "type": "string",
                        "description": "The message."
                      },
                      "url": {
                        "type": "string",
                        "description": "The attached URL."
                      }
                    }
                  }
                },
                "required": [
                  "type",
                  "value"
                ]
              }
            """
        );
    }

}
