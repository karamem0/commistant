//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using NSubstitute;
using NUnit.Framework;
using OpenAI;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Services.Tests;

[Category("Karamem0.Commistant.Services")]
public class OpenAIServiceTests
{

    [Test()]
    public async Task GetConversationPropertyOptionsAsync_Succeeded_ReturnsValue()
    {
        // Setup
        var modelName = "gpt-4o-mini";
        var apiKey = "api-key";
        var options = new ConversationPropertyOptions()
        {
            Type = "会議開始後",
            Value = new ConversationPropertyOptionsValue()
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
                    "StartMeeting",
                    BinaryData.FromObjectAsJson(options)
                )
            ]
        );
        var pipelineResponse = Substitute.For<PipelineResponse>();
        var chatClient = Substitute.For<ChatClient>(modelName, apiKey);
        _ = chatClient
            .CompleteChatAsync(
                Arg.Any<IEnumerable<ChatMessage>>(),
                Arg.Any<ChatCompletionOptions>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(ClientResult.FromValue(chatCompletion, pipelineResponse));
        var openAIClient = Substitute.For<OpenAIClient>(apiKey);
        _ = openAIClient.GetChatClient(modelName).Returns(chatClient);
        // Execute
        var target = new OpenAIService(openAIClient, modelName);
        var actual = await target.GetConversationPropertyOptionsAsync("会議開始後");
        // Validate
        Assert.That(actual, Is.EqualTo(options));
    }

    [Test()]
    public async Task GetConversationPropertyOptionsAsync_Failed_ReturnsNull()
    {
        // Setup
        var modelName = "gpt-4o-mini";
        var apiKey = "api-key";
        var chatCompletion = OpenAIChatModelFactory.ChatCompletion(finishReason: ChatFinishReason.Stop);
        var pipelineResponse = Substitute.For<PipelineResponse>();
        var chatClient = Substitute.For<ChatClient>(modelName, apiKey);
        _ = chatClient
            .CompleteChatAsync(
                Arg.Any<IEnumerable<ChatMessage>>(),
                Arg.Any<ChatCompletionOptions>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(ClientResult.FromValue(chatCompletion, pipelineResponse));
        var openAIClient = Substitute.For<OpenAIClient>(apiKey);
        _ = openAIClient.GetChatClient(modelName).Returns(chatClient);
        // Execute
        var target = new OpenAIService(openAIClient, modelName);
        var actual = await target.GetConversationPropertyOptionsAsync("会議開始後");
        // Validate
        Assert.That(actual, Is.Null);
    }

}
