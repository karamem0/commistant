//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.AI.OpenAI;
using Karamem0.Commistant.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Services
{

    public interface IOpenAIService
    {

        Task<ChatResponseMessage?> ChatCompletionAsync(string text, CancellationToken cancellationToken = default);

    }

    public class OpenAIService(OpenAIClient openAIClient, string openAIModelName) : IOpenAIService
    {

        private readonly OpenAIClient openAIClient = openAIClient;

        private readonly string openAIModelName = openAIModelName;

        public async Task<ChatResponseMessage?> ChatCompletionAsync(string text, CancellationToken cancellationToken = default)
        {
            var chatCompletionsOptions = new ChatCompletionsOptions(
                this.openAIModelName,
                new[]
                {
                    new ChatRequestUserMessage(text)
                }
            );
            chatCompletionsOptions.Functions.Add(new FunctionDefinition()
            {
                Name = "StartMeeting",
                Description = "Update the schedule, text, and URL of messages sent the start of the meeting.",
                Parameters = BinaryData.FromString(StringResources.StartMeetingJson)
            });
            chatCompletionsOptions.Functions.Add(new FunctionDefinition()
            {
                Name = "EndMeeting",
                Description = "Update the schedule, text, and URL of messages sent the end of the meeting.",
                Parameters = BinaryData.FromString(StringResources.EndMeetingJson)
            });
            chatCompletionsOptions.Functions.Add(new FunctionDefinition()
            {
                Name = "InMeeting",
                Description = "Update the schedule, text, and URL of messages sent during the meeting.",
                Parameters = BinaryData.FromString(StringResources.InMeetingJson)
            });
            chatCompletionsOptions.Functions.Add(new FunctionDefinition()
            {
                Name = "Reset",
                Description = "Reset all settings.",
                Parameters = BinaryData.FromString(StringResources.ResetJson)
            });
            chatCompletionsOptions.FunctionCall = FunctionDefinition.Auto;
            chatCompletionsOptions.Temperature = 0.3f;
            var chatCompletions = await this.openAIClient.GetChatCompletionsAsync(
                chatCompletionsOptions,
                cancellationToken
            );
            return chatCompletions.Value.Choices.Select(_ => _.Message).FirstOrDefault();
        }

    }

}

