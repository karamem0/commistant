//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Karamem0.Commistant.Options;
using Microsoft.Extensions.Options;
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
    public async Task GetConversationPropertiesOptionsAsync_WhenSucceeded_ShouldReturnValue()
    {
        // Setup
        var openAIOptions = Substitute.For<IOptions<AzureOpenAIOptions>>();
        _ = openAIOptions.Value.Returns(
            new AzureOpenAIOptions()
            {
                AzureOpenAIModelName = "gpt-4o-mini"
            }
        );
        var conversationPropertiesOptions = new ConversationPropertiesOptions()
        {
            Type = "会議開始後",
            Value = new ConversationPropertiesOptionsValue()
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
                    BinaryData.FromObjectAsJson(conversationPropertiesOptions)
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
        var openAIClient = Substitute.For<OpenAIClient>();
        _ = openAIClient
            .GetChatClient(Arg.Any<string>())
            .Returns(chatClient);
        // Execute
        var target = new OpenAIService(openAIClient, openAIOptions);
        var actual = await target.GetConversationPropertiesOptionsAsync("会議開始後");
        // Validate
        Assert.That(actual, Is.EqualTo(conversationPropertiesOptions));
    }

    [Test()]
    public async Task GetConversationPropertiesOptionsAsync_WhenFailed_ShouldReturnNull()
    {
        // Setup
        var openAIOptions = Substitute.For<IOptions<AzureOpenAIOptions>>();
        _ = openAIOptions.Value.Returns(
            new AzureOpenAIOptions()
            {
                AzureOpenAIModelName = "gpt-4o-mini"
            }
        );
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
        var openAIClient = Substitute.For<OpenAIClient>();
        _ = openAIClient
            .GetChatClient(Arg.Any<string>())
            .Returns(chatClient);
        // Execute
        var target = new OpenAIService(openAIClient, openAIOptions);
        var actual = await target.GetConversationPropertiesOptionsAsync("会議開始後");
        // Validate
        Assert.That(actual, Is.Null);
    }

}
