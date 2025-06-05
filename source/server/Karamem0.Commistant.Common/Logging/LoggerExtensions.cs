//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

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

    private static readonly Action<ILogger, string?, Exception?> unhandledErrorOccurred = LoggerMessage.Define<string?>(
        LogLevel.Critical,
        new EventId(1),
        "[{MemberName}] 予期しない問題が発生しました。"
    );

    public static void UnhandledErrorOccurred(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        Exception? exception = null
    )
    {
        unhandledErrorOccurred.Invoke(
            logger,
            memberName,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> functionFailed = LoggerMessage.Define<string?>(
        LogLevel.Error,
        new EventId(101),
        "[{MemberName}] 関数の実行に失敗しました。"
    );

    public static void FunctionFailed(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        Exception? exception = null
    )
    {
        functionFailed.Invoke(
            logger,
            memberName,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, Exception?> meetingStarted = LoggerMessage.Define<string?, string?, string?>(
        LogLevel.Information,
        new EventId(1001),
        "[{MemberName}] 会議を開始しました。ConversationId: {ConversationId}, MeetingId: {MeetingId}"
    );

    public static void MeetingStarted(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        string? meetingId = null,
        Exception? exception = null
    )
    {
        meetingStarted.Invoke(
            logger,
            memberName,
            conversationId,
            meetingId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, Exception?> meetingEnded = LoggerMessage.Define<string?, string?, string?>(
        LogLevel.Information,
        new EventId(1002),
        "[{MemberName}] 会議を終了しました。ConversationId: {ConversationId}, MeetingId: {MeetingId}"
    );

    public static void MeetingEnded(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        string? meetingId = null,
        Exception? exception = null
    )
    {
        meetingEnded.Invoke(
            logger,
            memberName,
            conversationId,
            meetingId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> functionExecuting = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(1003),
        "[{MemberName}] 関数を実行しています。"
    );

    public static void FunctionExecuting(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        Exception? exception = null
    )
    {
        functionExecuting.Invoke(
            logger,
            memberName,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> functionExecuted = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(1004),
        "[{MemberName}] 関数を実行しました。"
    );

    public static void FunctionExecuted(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        Exception? exception = null
    )
    {
        functionExecuted.Invoke(
            logger,
            memberName,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingStartMessageNotifying = LoggerMessage.Define<string?, string?, string?, string?>(
        LogLevel.Information,
        new EventId(1005),
        "[{MemberName}] 会議開始メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
    );

    public static void MeetingStartMessageNotifying(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingStartMessageNotifying.Invoke(
            logger,
            memberName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingStartMessageNotified = LoggerMessage.Define<string?, string?, string?, string?>(
        LogLevel.Information,
        new EventId(1006),
        "[{MemberName}] 会議開始メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
    );

    public static void MeetingStartMessageNotified(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingStartMessageNotified.Invoke(
            logger,
            memberName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingEndMessageNotifying = LoggerMessage.Define<string?, string?, string?, string?>(
        LogLevel.Information,
        new EventId(1007),
        "[{MemberName}] 会議終了メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
    );

    public static void MeetingEndMessageNotifying(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingEndMessageNotifying.Invoke(
            logger,
            memberName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingEndMessageNotified = LoggerMessage.Define<string?, string?, string?, string?>(
        LogLevel.Information,
        new EventId(1008),
        "[{MemberName}] 会議終了メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
    );

    public static void MeetingEndMessageNotified(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingEndMessageNotified.Invoke(
            logger,
            memberName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingRunMessageNotifying = LoggerMessage.Define<string?, string?, string?, string?>(
        LogLevel.Information,
        new EventId(1009),
        "[{MemberName}] 会議中メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
    );

    public static void MeetingRunMessageNotifying(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingRunMessageNotifying.Invoke(
            logger,
            memberName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingRunMessageNotified = LoggerMessage.Define<string?, string?, string?, string?>(
        LogLevel.Information,
        new EventId(1010),
        "[{MemberName}] 会議中メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
    );

    public static void MeetingRunMessageNotified(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingRunMessageNotified.Invoke(
            logger,
            memberName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> settingsUpdating = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(1011),
        "[{MemberName}] 設定を変更します。ConversationId: {ConversationId}"
    );

    public static void SettingsUpdating(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsUpdating.Invoke(
            logger,
            memberName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> settingsUpdated = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(1012),
        "[{MemberName}] 設定を変更しました。ConversationId: {ConversationId}"
    );

    public static void SettingsUpdated(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsUpdated.Invoke(
            logger,
            memberName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> settingsResetting = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(1013),
        "[{MemberName}] 設定を初期化します。ConversationId: {ConversationId}"
    );

    public static void SettingsResetting(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsResetting.Invoke(
            logger,
            memberName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> settingsReseted = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(1014),
        "[{MemberName}] 設定を初期化しました。ConversationId: {ConversationId}"
    );

    public static void SettingsReseted(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsReseted.Invoke(
            logger,
            memberName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> settingsCancelled = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(1015),
        "[{MemberName}] キャンセルしました。設定は変更されていません。ConversationId: {ConversationId}"
    );

    public static void SettingsCancelled(
        this ILogger logger,
        [CallerMemberName()] string? memberName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsCancelled.Invoke(
            logger,
            memberName,
            conversationId,
            exception
        );
    }

}
