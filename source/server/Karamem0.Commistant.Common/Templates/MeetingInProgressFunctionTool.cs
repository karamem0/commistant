//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

namespace Karamem0.Commistant.Templates;

public static class MeetingInProgressFunctionTool
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
                    "description": "This must be \"会議中\"."
                  },
                  "value": {
                    "type": "object",
                    "description": "The settings about message to send during the meeting.",
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
