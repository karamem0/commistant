//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Logging;

public static class LoggerExtensions
{

    private static readonly Action<ILogger, string?, Exception?> unhandledError =
        LoggerMessage.Define<string?>(
            LogLevel.Error,
            new EventId(1),
            "[{MemberName}] 予期しない問題が発生しました。"
        );

    public static void UnhandledError(
        this ILogger logger,
        Exception? exception,
        [CallerMemberName()] string? memberName = null
    )
    {
        unhandledError.Invoke(logger, memberName, exception);
    }

    private static readonly Action<ILogger, string?, string, string?, Exception?> meetingStarted =
        LoggerMessage.Define<string?, string, string?>(
            LogLevel.Information,
            new EventId(1001),
            "[{MemberName}] 会議を開始しました。ConversationId: {ConversationId}, MeetingId: {MeetingId}"
        );

    public static void MeetingStarted(
        this ILogger logger,
        IActivity activity,
        string? meetingId,
        [CallerMemberName()] string? memberName = null
    )
    {
        meetingStarted.Invoke(logger, memberName, activity.Conversation.Id, meetingId, null);
    }

    private static readonly Action<ILogger, string?, string, string?, Exception?> meetingEnded =
        LoggerMessage.Define<string?, string, string?>(
            LogLevel.Information,
            new EventId(1002),
            "[{MemberName}] 会議を終了しました。ConversationId: {ConversationId}, MeetingId: {MeetingId}"
        );

    public static void MeetingEnded(
        this ILogger logger,
        IActivity activity,
        string? meetingId,
        [CallerMemberName()] string? memberName = null
    )
    {
        meetingEnded.Invoke(logger, memberName, activity.Conversation.Id, meetingId, null);
    }

    private static readonly Action<ILogger, string?, Exception?> timerStarted =
        LoggerMessage.Define<string?>(
            LogLevel.Information,
            new EventId(1003),
            "[{MemberName}] タイマーを開始しました。"
        );

    public static void TimerStarted(this ILogger logger, [CallerMemberName()] string? memberName = null)
    {
        timerStarted.Invoke(logger, memberName, null);
    }

    private static readonly Action<ILogger, string?, Exception?> timerEnded =
        LoggerMessage.Define<string?>(
            LogLevel.Information,
            new EventId(1004),
            "[{MemberName}] タイマーを終了しました。"
        );

    public static void TimerEnded(this ILogger logger, [CallerMemberName()] string? memberName = null)
    {
        timerEnded.Invoke(logger, memberName, null);
    }

    private static readonly Action<ILogger, string?, string, string?, Exception?> timerExecuting =
        LoggerMessage.Define<string?, string, string?>(
            LogLevel.Information,
            new EventId(1005),
            "[{MemberName}] タイマーを実行しています。ConversationId: {ConversationId}, MeetingId: {MeetingId}"
        );

    public static void TimerExecuting(
        this ILogger logger,
        ConversationReference reference,
        string? meetingId,
        [CallerMemberName()] string? memberName = null
    )
    {
        timerExecuting.Invoke(logger, memberName, reference.Conversation.Id, meetingId, null);
    }

    private static readonly Action<ILogger, string?, string, string?, Exception?> timerExecuted =
        LoggerMessage.Define<string?, string, string?>(
            LogLevel.Information,
            new EventId(1006),
            "[{MemberName}] タイマーを実行しました。ConversationId: {ConversationId}, MeetingId: {MeetingId}"
        );

    public static void TimerExecuted(
        this ILogger logger,
        ConversationReference reference,
        string? meetingId,
        [CallerMemberName()] string? memberName = null
    )
    {
        timerExecuted.Invoke(logger, memberName, reference.Conversation.Id, meetingId, null);
    }

    private static readonly Action<ILogger, string?, string, string?, string?, Exception?> startMeetingMessageNotifying =
        LoggerMessage.Define<string?, string, string?, string?>(
            LogLevel.Information,
            new EventId(1007),
            "[{MemberName}] 会議開始メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void StartMeetingMessageNotifying(
        this ILogger logger,
        ConversationReference reference,
        ConversationProperty property,
        [CallerMemberName()] string? memberName = null
    )
    {
        startMeetingMessageNotifying.Invoke(
            logger,
            memberName,
            reference.Conversation.Id,
            property.StartMeetingMessage,
            property.StartMeetingUrl,
            null
        );
    }

    private static readonly Action<ILogger, string?, string, string?, string?, Exception?> startMeetingMessageNotified =
        LoggerMessage.Define<string?, string, string?, string?>(
            LogLevel.Information,
            new EventId(1008),
            "[{MemberName}] 会議開始メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void StartMeetingMessageNotified(
        this ILogger logger,
        ConversationReference reference,
        ConversationProperty property,
        [CallerMemberName()] string? memberName = null
    )
    {
        startMeetingMessageNotified.Invoke(
            logger,
            memberName,
            reference.Conversation.Id,
            property.StartMeetingMessage,
            property.StartMeetingUrl,
            null
        );
    }

    private static readonly Action<ILogger, string?, string, string?, string?, Exception?> endMeetingMessageNotifying =
        LoggerMessage.Define<string?, string, string?, string?>(
            LogLevel.Information,
            new EventId(1009),
            "[{MemberName}] 会議終了メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void EndMeetingMessageNotifying(
        this ILogger logger,
        ConversationReference reference,
        ConversationProperty property,
        [CallerMemberName()] string? memberName = null
    )
    {
        endMeetingMessageNotifying.Invoke(
            logger,
            memberName,
            reference.Conversation.Id,
            property.EndMeetingMessage,
            property.EndMeetingUrl,
            null
        );
    }

    private static readonly Action<ILogger, string?, string, string?, string?, Exception?> endMeetingMessageNotified =
        LoggerMessage.Define<string?, string, string?, string?>(
            LogLevel.Information,
            new EventId(1010),
            "[{MemberName}] 会議終了メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void EndMeetingMessageNotified(
        this ILogger logger,
        ConversationReference reference,
        ConversationProperty property,
        [CallerMemberName()] string? memberName = null
    )
    {
        endMeetingMessageNotified.Invoke(
            logger,
            memberName,
            reference.Conversation.Id,
            property.EndMeetingMessage,
            property.EndMeetingUrl,
            null
        );
    }

    private static readonly Action<ILogger, string?, string, string?, string?, Exception?> inMeetingMessageNotifying =
        LoggerMessage.Define<string?, string, string?, string?>(
            LogLevel.Information,
            new EventId(1011),
            "[{MemberName}] 会議中メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void InMeetingMessageNotifying(
        this ILogger logger,
        ConversationReference reference,
        ConversationProperty property,
        [CallerMemberName()] string? memberName = null
    )
    {
        inMeetingMessageNotifying.Invoke(
            logger,
            memberName,
            reference.Conversation.Id,
            property.InMeetingMessage,
            property.InMeetingUrl,
            null
        );
    }

    private static readonly Action<ILogger, string?, string, string?, string?, Exception?> inMeetingMessageNotified =
        LoggerMessage.Define<string?, string, string?, string?>(
            LogLevel.Information,
            new EventId(1012),
            "[{MemberName}] 会議中メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void InMeetingMessageNotified(
        this ILogger logger,
        ConversationReference reference,
        ConversationProperty property,
        [CallerMemberName()] string? memberName = null
    )
    {
        inMeetingMessageNotified.Invoke(
            logger,
            memberName,
            reference.Conversation.Id,
            property.InMeetingMessage,
            property.InMeetingUrl,
            null
        );
    }

    private static readonly Action<ILogger, string?, string, Exception?> settingsUpdating =
        LoggerMessage.Define<string?, string>(
            LogLevel.Information,
            new EventId(1013),
            "[{MemberName}] 設定を変更します。ConversationId: {ConversationId}"
        );

    public static void SettingsUpdating(
        this ILogger logger,
        IActivity activity,
        [CallerMemberName()] string? memberName = null
    )
    {
        settingsUpdating.Invoke(logger, memberName, activity.Conversation.Id, null);
    }

    private static readonly Action<ILogger, string?, string, Exception?> settingsUpdated =
        LoggerMessage.Define<string?, string>(
            LogLevel.Information,
            new EventId(1014),
            "[{MemberName}] 設定を変更しました。ConversationId: {ConversationId}");

    public static void SettingsUpdated(
        this ILogger logger,
        IActivity activity,
        [CallerMemberName()] string? memberName = null
    )
    {
        settingsUpdated.Invoke(logger, memberName, activity.Conversation.Id, null);
    }

    private static readonly Action<ILogger, string?, string, Exception?> settingsResetting =
        LoggerMessage.Define<string?, string>(
            LogLevel.Information,
            new EventId(1015),
            "[{MemberName}] 設定を初期化します。ConversationId: {ConversationId}"
        );

    public static void SettingsResetting(
        this ILogger logger,
        IActivity activity,
        [CallerMemberName()] string? memberName = null
    )
    {
        settingsResetting.Invoke(logger, memberName, activity.Conversation.Id, null);
    }

    private static readonly Action<ILogger, string?, string, Exception?> settingsReseted =
        LoggerMessage.Define<string?, string>(
            LogLevel.Information,
            new EventId(1016),
            "[{MemberName}] 設定を初期化しました。ConversationId: {ConversationId}"
        );

    public static void SettingsReseted(
        this ILogger logger,
        IActivity activity,
        [CallerMemberName()] string? memberName = null
    )
    {
        settingsReseted.Invoke(logger, memberName, activity.Conversation.Id, null);
    }

    private static readonly Action<ILogger, string?, string, Exception?> settingsCancelled =
        LoggerMessage.Define<string?, string>(
            LogLevel.Information,
            new EventId(1017),
            "[{MemberName}] キャンセルしました。設定は変更されていません。ConversationId: {ConversationId}"
        );

    public static void SettingsCancelled(
        this ILogger logger,
        IActivity activity,
        [CallerMemberName()] string? memberName = null
    )
    {
        settingsCancelled.Invoke(logger, memberName, activity.Conversation.Id, null);
    }

}
