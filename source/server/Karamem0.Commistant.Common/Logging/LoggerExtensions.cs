//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Karamem0.Commistant.Logging;

public static class LoggerExtensions
{

    private static readonly Action<ILogger, string?, Exception?> methodExecuting = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(2001),
        "[{MethodName}] メソッドを実行しています。"
    );

    public static void MethodExecuting(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        Exception? exception = null
    )
    {
        methodExecuting.Invoke(
            logger,
            methodName,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> methodExecuted = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(2002),
        "[{MethodName}] メソッドを実行しました。"
    );

    public static void MethodExecuted(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        Exception? exception = null
    )
    {
        methodExecuted.Invoke(
            logger,
            methodName,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, Exception?> meetingStarted = LoggerMessage.Define<string?, string?, string?>(
        LogLevel.Information,
        new EventId(2101),
        "[{MethodName}] 会議を開始しました。ConversationId: {ConversationId}, MeetingId: {MeetingId}"
    );

    public static void MeetingStarted(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? meetingId = null,
        Exception? exception = null
    )
    {
        meetingStarted.Invoke(
            logger,
            methodName,
            conversationId,
            meetingId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, Exception?> meetingEnded = LoggerMessage.Define<string?, string?, string?>(
        LogLevel.Information,
        new EventId(2102),
        "[{MethodName}] 会議を終了しました。ConversationId: {ConversationId}, MeetingId: {MeetingId}"
    );

    public static void MeetingEnded(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? meetingId = null,
        Exception? exception = null
    )
    {
        meetingEnded.Invoke(
            logger,
            methodName,
            conversationId,
            meetingId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingStartedMessageNotifying =
        LoggerMessage.Define<string?, string?, string?, string?>(
            LogLevel.Information,
            new EventId(2103),
            "[{MethodName}] 会議開始メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void MeetingStartedMessageNotifying(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingStartedMessageNotifying.Invoke(
            logger,
            methodName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingStartedMessageNotified =
        LoggerMessage.Define<string?, string?, string?, string?>(
            LogLevel.Information,
            new EventId(2104),
            "[{MethodName}] 会議開始メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void MeetingStartedMessageNotified(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingStartedMessageNotified.Invoke(
            logger,
            methodName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingEndingMessageNotifying =
        LoggerMessage.Define<string?, string?, string?, string?>(
            LogLevel.Information,
            new EventId(2105),
            "[{MethodName}] 会議終了メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void MeetingEndingMessageNotifying(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingEndingMessageNotifying.Invoke(
            logger,
            methodName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingEndingMessageNotified =
        LoggerMessage.Define<string?, string?, string?, string?>(
            LogLevel.Information,
            new EventId(2106),
            "[{MethodName}] 会議終了メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void MeetingEndingMessageNotified(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingEndingMessageNotified.Invoke(
            logger,
            methodName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingInProgressMessageNotifying =
        LoggerMessage.Define<string?, string?, string?, string?>(
            LogLevel.Information,
            new EventId(2107),
            "[{MethodName}] 会議中メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void MeetingInProgressMessageNotifying(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingInProgressMessageNotifying.Invoke(
            logger,
            methodName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingInProgressMessageNotified =
        LoggerMessage.Define<string?, string?, string?, string?>(
            LogLevel.Information,
            new EventId(2108),
            "[{MethodName}] 会議中メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void MeetingInProgressMessageNotified(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingInProgressMessageNotified.Invoke(
            logger,
            methodName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> settingsUpdating = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(2109),
        "[{MethodName}] 設定を変更します。ConversationId: {ConversationId}"
    );

    public static void SettingsUpdating(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsUpdating.Invoke(
            logger,
            methodName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> settingsUpdated = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(2110),
        "[{MethodName}] 設定を変更しました。ConversationId: {ConversationId}"
    );

    public static void SettingsUpdated(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsUpdated.Invoke(
            logger,
            methodName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> settingsUpdateCancelled = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(2111),
        "[{MethodName}] 設定の変更をキャンセルしました。設定は変更されていません。ConversationId: {ConversationId}"
    );

    public static void SettingsUpdateCancelled(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsUpdateCancelled.Invoke(
            logger,
            methodName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> settingsInitializing = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(2112),
        "[{MethodName}] 設定を初期化します。ConversationId: {ConversationId}"
    );

    public static void SettingsInitializing(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsInitializing.Invoke(
            logger,
            methodName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> settingsInitialized = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(2113),
        "[{MethodName}] 設定を初期化しました。ConversationId: {ConversationId}"
    );

    public static void SettingsInitialized(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsInitialized.Invoke(
            logger,
            methodName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> settingsInitializeCancelled = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(2114),
        "[{MethodName}] 設定の初期化をキャンセルしました。設定は変更されていません。ConversationId: {ConversationId}"
    );

    public static void SettingsInitializeCancelled(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsInitializeCancelled.Invoke(
            logger,
            methodName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> membersAdded = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(2115),
        "[{MethodName}] メンバーが追加されました。ConversationId: {ConversationId}"
    );

    public static void MembersAdded(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        membersAdded.Invoke(
            logger,
            methodName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> membersRemoved = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(2116),
        "[{MethodName}] メンバーが削除されました。ConversationId: {ConversationId}"
    );

    public static void MembersRemoved(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        membersRemoved.Invoke(
            logger,
            methodName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> messageReceived = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(2117),
        "[{MethodName}] メッセージを受信しました。ConversationId: {ConversationId}"
    );

    public static void MessageReceived(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        messageReceived.Invoke(
            logger,
            methodName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> methodFailed = LoggerMessage.Define<string?>(
        LogLevel.Error,
        new EventId(4001),
        "[{MethodName}] メソッドの実行に失敗しました。"
    );

    public static void MethodFailed(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        Exception? exception = null
    )
    {
        methodFailed.Invoke(
            logger,
            methodName,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> authorizationFailed = LoggerMessage.Define<string?>(
        LogLevel.Error,
        new EventId(4002),
        "[{MethodName}] 認証に失敗しました。"
    );

    public static void AuthorizationFailed(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        Exception? exception = null
    )
    {
        authorizationFailed.Invoke(
            logger,
            methodName,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> unhandledErrorOccurred = LoggerMessage.Define<string?>(
        LogLevel.Critical,
        new EventId(5001),
        "[{MethodName}] 予期しない問題が発生しました。"
    );

    public static void UnhandledErrorOccurred(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        Exception? exception = null
    )
    {
        unhandledErrorOccurred.Invoke(
            logger,
            methodName,
            exception
        );
    }

}
