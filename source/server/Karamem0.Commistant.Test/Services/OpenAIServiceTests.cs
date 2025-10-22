//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

#pragma warning disable OPENAI001

using Karamem0.Commistant.Models;
using NSubstitute;
using NUnit.Framework;
using OpenAI.Chat;
using System.ClientModel;
using System.ClientModel.Primitives;
using System.Threading;

namespace Karamem0.Commistant.Services.Tests;

[Category("Karamem0.Commistant.Services")]
public class OpenAIServiceTests
{

    [Test()]
    public async Task GetCommandOptionsAsync_Success()
    {
        // Setup
        var options = new CommandOptions()
        {
            Type = "会議開始後",
            Value = new CommandOptionsValue()
            {
                Schedule = 5,
                Message = "Hello World!",
                Url = "https://www.exmple.com"
            }
        };
        var chatCompletion = OpenAIChatModelFactory.ChatCompletion(
            finishReason: ChatFinishReason.ToolCalls,
            toolCalls:
            [
                ChatToolCall.CreateFunctionToolCall(
                    "ab3d750e-8b79-4987-b40c-422a0ec98d02",
                    "MeetingStart",
                    BinaryData.FromObjectAsJson(options)
                )
            ]
        );
        var pipelineResponse = Substitute.For<PipelineResponse>();
        var chatClient = Substitute.For<ChatClient>();
        _ = chatClient
            .CompleteChatAsync(
                Arg.Any<IEnumerable<ChatMessage>>(),
                Arg.Any<ChatCompletionOptions>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(ClientResult.FromValue(chatCompletion, pipelineResponse));
        // Execute
        var target = new OpenAIService(chatClient);
        var actual = await target.GetCommandOptionsAsync("会議開始後");
        // Assert
        Assert.That(actual, Is.EqualTo(options));
    }

    [Test()]
    public async Task GetCommandOptionsAsync_Failure_WhenStopped()
    {
        // Setup
        var chatCompletion = OpenAIChatModelFactory.ChatCompletion(finishReason: ChatFinishReason.Stop);
        var pipelineResponse = Substitute.For<PipelineResponse>();
        var chatClient = Substitute.For<ChatClient>();
        _ = chatClient
            .CompleteChatAsync(
                Arg.Any<IEnumerable<ChatMessage>>(),
                Arg.Any<ChatCompletionOptions>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(ClientResult.FromValue(chatCompletion, pipelineResponse));
        // Execute
        var target = new OpenAIService(chatClient);
        var actual = await target.GetCommandOptionsAsync("会議開始後");
        // Assert
        Assert.That(actual, Is.Null);
    }

}

#pragma warning restore OPENAI001
