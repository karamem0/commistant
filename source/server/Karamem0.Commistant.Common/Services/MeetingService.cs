//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema.Teams;
using System.Threading;

namespace Karamem0.Commistant.Services;

public interface IMeetingService
{

    Task<MeetingInfo> GetMeetingInfoAsync(ITurnContext turnContext, CancellationToken cancellationToken = default);

    Task<TeamsMeetingParticipant> GetMeetingParticipantAsync(
        ITurnContext turnContext,
        string participantId,
        CancellationToken cancellationToken = default
    );

}

public class MeetingService : IMeetingService
{

    public async Task<MeetingInfo> GetMeetingInfoAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
    {
        return await TeamsInfo.GetMeetingInfoAsync(turnContext, cancellationToken: cancellationToken);
    }

    public async Task<TeamsMeetingParticipant> GetMeetingParticipantAsync(
        ITurnContext turnContext,
        string participantId,
        CancellationToken cancellationToken = default
    )
    {
        return await TeamsInfo.GetMeetingParticipantAsync(
            turnContext,
            participantId,
            cancellationToken: cancellationToken
        );
    }

}
