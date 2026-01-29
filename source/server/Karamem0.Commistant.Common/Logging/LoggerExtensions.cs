//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Extensions.Logging;

namespace Karamem0.Commistant.Logging;

public static class LoggerExtensions
{

    private static readonly Action<ILogger, Exception?> methodExecuting = LoggerMessage.Define(
        LogLevel.Information,
        new EventId(2001),
        "メソッドを実行します。"
    );

    public static void MethodExecuting(this ILogger logger, Exception? exception = null)
    {
        methodExecuting.Invoke(logger, exception);
    }

    private static readonly Action<ILogger, Exception?> methodExecuted = LoggerMessage.Define(
        LogLevel.Information,
        new EventId(2002),
        "メソッドを実行しました。"
    );

    public static void MethodExecuted(this ILogger logger, Exception? exception = null)
    {
        methodExecuted.Invoke(logger, exception);
    }

    private static readonly Action<ILogger, string?, string?, Exception?> meetingStarted = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(2101),
        "会議を開始しました。ConversationId: {ConversationId}, MeetingId: {MeetingId}"
    );

    public static void MeetingStarted(
        this ILogger logger,
        string? conversationId = null,
        string? meetingId = null,
        Exception? exception = null
    )
    {
        meetingStarted.Invoke(
            logger,
            conversationId,
            meetingId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> meetingEnded = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(2102),
        "会議を終了しました。ConversationId: {ConversationId}, MeetingId: {MeetingId}"
    );

    public static void MeetingEnded(
        this ILogger logger,
        string? conversationId = null,
        string? meetingId = null,
        Exception? exception = null
    )
    {
        meetingEnded.Invoke(
            logger,
            conversationId,
            meetingId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, Exception?> meetingStartedMessageNotifying =
        LoggerMessage.Define<string?, string?, string?>(
            LogLevel.Information,
            new EventId(2103),
            "会議開始後メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void MeetingStartedMessageNotifying(
        this ILogger logger,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingStartedMessageNotifying.Invoke(
            logger,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, Exception?> meetingStartedMessageNotified =
        LoggerMessage.Define<string?, string?, string?>(
            LogLevel.Information,
            new EventId(2104),
            "会議開始後メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void MeetingStartedMessageNotified(
        this ILogger logger,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingStartedMessageNotified.Invoke(
            logger,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, Exception?> meetingEndingMessageNotifying =
        LoggerMessage.Define<string?, string?, string?>(
            LogLevel.Information,
            new EventId(2105),
            "会議終了前メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void MeetingEndingMessageNotifying(
        this ILogger logger,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingEndingMessageNotifying.Invoke(
            logger,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, Exception?> meetingEndingMessageNotified =
        LoggerMessage.Define<string?, string?, string?>(
            LogLevel.Information,
            new EventId(2106),
            "会議終了前メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void MeetingEndingMessageNotified(
        this ILogger logger,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingEndingMessageNotified.Invoke(
            logger,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, Exception?> meetingInProgressMessageNotifying =
        LoggerMessage.Define<string?, string?, string?>(
            LogLevel.Information,
            new EventId(2107),
            "会議中メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void MeetingInProgressMessageNotifying(
        this ILogger logger,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingInProgressMessageNotifying.Invoke(
            logger,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, Exception?> meetingInProgressMessageNotified =
        LoggerMessage.Define<string?, string?, string?>(
            LogLevel.Information,
            new EventId(2108),
            "会議中メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
        );

    public static void MeetingInProgressMessageNotified(
        this ILogger logger,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingInProgressMessageNotified.Invoke(
            logger,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> settingsUpdating = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(2109),
        "設定を変更します。ConversationId: {ConversationId}"
    );

    public static void SettingsUpdating(
        this ILogger logger,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsUpdating.Invoke(
            logger,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> settingsUpdated = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(2110),
        "設定を変更しました。ConversationId: {ConversationId}"
    );

    public static void SettingsUpdated(
        this ILogger logger,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsUpdated.Invoke(
            logger,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> settingsUpdateCancelled = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(2111),
        "設定の変更をキャンセルしました。設定は変更されていません。ConversationId: {ConversationId}"
    );

    public static void SettingsUpdateCancelled(
        this ILogger logger,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsUpdateCancelled.Invoke(
            logger,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> settingsInitializing = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(2112),
        "設定を初期化します。ConversationId: {ConversationId}"
    );

    public static void SettingsInitializing(
        this ILogger logger,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsInitializing.Invoke(
            logger,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> settingsInitialized = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(2113),
        "設定を初期化しました。ConversationId: {ConversationId}"
    );

    public static void SettingsInitialized(
        this ILogger logger,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsInitialized.Invoke(
            logger,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> settingsInitializeCancelled = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(2114),
        "設定の初期化をキャンセルしました。設定は変更されていません。ConversationId: {ConversationId}"
    );

    public static void SettingsInitializeCancelled(
        this ILogger logger,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsInitializeCancelled.Invoke(
            logger,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> membersAdded = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(2115),
        "メンバーが追加されました。ConversationId: {ConversationId}"
    );

    public static void MembersAdded(
        this ILogger logger,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        membersAdded.Invoke(
            logger,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> membersRemoved = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(2116),
        "メンバーが削除されました。ConversationId: {ConversationId}"
    );

    public static void MembersRemoved(
        this ILogger logger,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        membersRemoved.Invoke(
            logger,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, Exception?> messageReceived = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(2117),
        "メッセージを受信しました。ConversationId: {ConversationId}"
    );

    public static void MessageReceived(
        this ILogger logger,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        messageReceived.Invoke(
            logger,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, Exception?> methodFailed = LoggerMessage.Define(
        LogLevel.Error,
        new EventId(4001),
        "メソッドの実行に失敗しました。"
    );

    public static void MethodFailed(this ILogger logger, Exception? exception = null)
    {
        methodFailed.Invoke(logger, exception);
    }

    private static readonly Action<ILogger, Exception?> authorizationFailed = LoggerMessage.Define(
        LogLevel.Error,
        new EventId(4002),
        "認証に失敗しました。"
    );

    public static void AuthorizationFailed(this ILogger logger, Exception? exception = null)
    {
        authorizationFailed.Invoke(logger, exception);
    }

    private static readonly Action<ILogger, Exception?> unhandledErrorOccurred = LoggerMessage.Define(
        LogLevel.Critical,
        new EventId(5001),
        "予期しない問題が発生しました。"
    );

    public static void UnhandledErrorOccurred(this ILogger logger, Exception? exception = null)
    {
        unhandledErrorOccurred.Invoke(logger, exception);
    }

}
