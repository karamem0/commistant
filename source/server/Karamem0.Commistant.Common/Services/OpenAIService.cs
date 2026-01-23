//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Karamem0.Commistant.Serialization;
using Karamem0.Commistant.Templates;
using OpenAI.Chat;
using System.Threading;

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
            Tools =
            {
                ChatTool.CreateFunctionTool(
                    "MeetingStarted",
                    "Update the schedule, text, and URL of messages sent the start of the meeting.",
                    MeetingStartedFunctionTool.Create()
                ),
                ChatTool.CreateFunctionTool(
                    "MeetingEnding",
                    "Update the schedule, text, and URL of messages sent the end of the meeting.",
                    MeetingEndingFunctionTool.Create()
                ),
                ChatTool.CreateFunctionTool(
                    "MeetingInProgress",
                    "Update the schedule, text, and URL of messages sent during the meeting.",
                    MeetingInProgressFunctionTool.Create()
                ),
                ChatTool.CreateFunctionTool(
                    "Initialize",
                    "Initialize all settings.",
                    InitializeFunctionTool.Create()
                ),
            }
        };
        var chatCompletion = await this.chatClient.CompleteChatAsync(
            [
                new SystemChatMessage(
                    """
                    You are an AI assistant generating JSON.
                    You can only use user input and cannot use your own knowledge.
                    """
                ),
                new UserChatMessage(text)
            ],
            chatCompletionsOptions,
            cancellationToken
        );
        if (chatCompletion.Value.FinishReason == ChatFinishReason.ToolCalls)
        {
            return JsonConverter.Deserialize<CommandOptions>(
                chatCompletion
                    .Value.ToolCalls.Select(item => item.FunctionArguments)
                    .First()
                    .ToString()
            );
        }
        return null;
    }

}
