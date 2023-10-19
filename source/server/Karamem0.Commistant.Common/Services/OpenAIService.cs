//
// Copyright (c) 2023 karamem0
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

        Task<ChatMessage?> ChatCompletionAsync(string text, CancellationToken cancellationToken = default);

    }

    public class OpenAIService : IOpenAIService
    {

        private readonly OpenAIClient openAIClient;

        public OpenAIService(OpenAIClient openAIClient)
        {
            this.openAIClient = openAIClient;
        }

        public async Task<ChatMessage?> ChatCompletionAsync(string text, CancellationToken cancellationToken = default)
        {
            var chatCompletionsOptions = new ChatCompletionsOptions();
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
            chatCompletionsOptions.Messages.Add(new ChatMessage(ChatRole.User, text));
            var chatCompletions = await this.openAIClient.GetChatCompletionsAsync(
                "gpt-35-turbo-0613",
                chatCompletionsOptions,
                cancellationToken
            );
            return chatCompletions.Value.Choices.Select(_ => _.Message).FirstOrDefault();
        }

    }

}

