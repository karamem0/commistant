//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Agents.Builder;
using Microsoft.Agents.Extensions.Teams.Connector;
using Microsoft.Agents.Extensions.Teams.Models;
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
