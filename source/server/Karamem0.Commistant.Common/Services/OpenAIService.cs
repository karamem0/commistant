//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Karamem0.Commistant.Resources;
using Newtonsoft.Json;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Services;

public interface IOpenAIService
{

    Task<CommandOptions?> GetCommandOptionsAsync(string text, CancellationToken cancellationToken = default);

}

public class OpenAIService(ChatClient chatClient) : IOpenAIService
{

    private readonly ChatClient chatClient = chatClient;

    public async Task<CommandOptions?> GetCommandOptionsAsync(string text, CancellationToken cancellationToken = default)
    {
        var chatCompletionsOptions = new ChatCompletionOptions()
        {
            Temperature = 0.3f,
            Tools =
            {
                ChatTool.CreateFunctionTool(
                    "StartMeeting",
                    "Update the schedule, text, and URL of messages sent the start of the meeting.",
                    BinaryData.FromString(StringResources.StartMeetingJson)
                ),
                ChatTool.CreateFunctionTool(
                    "EndMeeting",
                    "Update the schedule, text, and URL of messages sent the end of the meeting.",
                    BinaryData.FromString(StringResources.EndMeetingJson)
                ),
                ChatTool.CreateFunctionTool(
                    "InMeeting",
                    "Update the schedule, text, and URL of messages sent during the meeting.",
                    BinaryData.FromString(StringResources.InMeetingJson)
                ),
                ChatTool.CreateFunctionTool(
                    "Reset",
                    "Reset all settings.",
                    BinaryData.FromString(StringResources.ResetJson)
                ),
            }
        };
        var chatCompletion = await this.chatClient.CompleteChatAsync(
            [
                new SystemChatMessage(
                    string.Join(
                        " ",
                        [
                            "You are an AI assistant generating JSON.",
                            "You can only use user input and cannot use your own knowledge."
                        ]
                    )
                ),
                new UserChatMessage(text)
            ],
            chatCompletionsOptions,
            cancellationToken
        );
        if (chatCompletion.Value.FinishReason == ChatFinishReason.ToolCalls)
        {
            return JsonConvert.DeserializeObject<CommandOptions>(
                chatCompletion
                    .Value.ToolCalls.Select(item => item.FunctionArguments)
                    .First()
                    .ToString()
            );
        }
        return null;
    }

}
