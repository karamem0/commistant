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
                    "会議が開始した後に通知するスケジュール、テキスト、および URL を設定します。",
                    MeetingStartedFunctionTool.Create()
                ),
                ChatTool.CreateFunctionTool(
                    "MeetingEnding",
                    "会議が終了する前に通知するスケジュール、テキスト、および URL を設定します。",
                    MeetingEndingFunctionTool.Create()
                ),
                ChatTool.CreateFunctionTool(
                    "MeetingInProgress",
                    "会議中に通知するスケジュール、テキスト、および URL を設定します。",
                    MeetingInProgressFunctionTool.Create()
                ),
                ChatTool.CreateFunctionTool(
                    "Initialize",
                    "この会議のすべての設定を初期化します。",
                    InitializeFunctionTool.Create()
                ),
                ChatTool.CreateFunctionTool(
                    "Help",
                    "ヘルプ情報を表示します。",
                    HelpFunctionTool.Create()
                )
            }
        };
        var chatCompletion = await this.chatClient.CompleteChatAsync(
            [
                new SystemChatMessage(
                    """
                    あなたはユーザーからの入力から JSON を生成する AI アシスタントです。
                    ユーザーの入力のみを使用し自分の知識を使用してはいけません。
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
