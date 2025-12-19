//
// Copyright (c) 2022-2025 karamem0
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

    private static readonly Action<ILogger, string?, Exception?> unhandledErrorOccurred = LoggerMessage.Define<string?>(
        LogLevel.Critical,
        new EventId(1),
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

    private static readonly Action<ILogger, string?, Exception?> methodFailed = LoggerMessage.Define<string?>(
        LogLevel.Error,
        new EventId(101),
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
        new EventId(102),
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

    private static readonly Action<ILogger, string?, string?, string?, Exception?> meetingStarted = LoggerMessage.Define<string?, string?, string?>(
        LogLevel.Information,
        new EventId(1001),
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
        new EventId(1002),
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

    private static readonly Action<ILogger, string?, Exception?> methodExecuting = LoggerMessage.Define<string?>(
        LogLevel.Information,
        new EventId(1003),
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
        new EventId(1004),
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

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingStartMessageNotifying = LoggerMessage.Define<string?, string?, string?, string?>(
        LogLevel.Information,
        new EventId(1005),
        "[{MethodName}] 会議開始メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
    );

    public static void MeetingStartMessageNotifying(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingStartMessageNotifying.Invoke(
            logger,
            methodName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingStartMessageNotified = LoggerMessage.Define<string?, string?, string?, string?>(
        LogLevel.Information,
        new EventId(1006),
        "[{MethodName}] 会議開始メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
    );

    public static void MeetingStartMessageNotified(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingStartMessageNotified.Invoke(
            logger,
            methodName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingEndMessageNotifying = LoggerMessage.Define<string?, string?, string?, string?>(
        LogLevel.Information,
        new EventId(1007),
        "[{MethodName}] 会議終了メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
    );

    public static void MeetingEndMessageNotifying(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingEndMessageNotifying.Invoke(
            logger,
            methodName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingEndMessageNotified = LoggerMessage.Define<string?, string?, string?, string?>(
        LogLevel.Information,
        new EventId(1008),
        "[{MethodName}] 会議終了メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
    );

    public static void MeetingEndMessageNotified(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingEndMessageNotified.Invoke(
            logger,
            methodName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingRunMessageNotifying = LoggerMessage.Define<string?, string?, string?, string?>(
        LogLevel.Information,
        new EventId(1009),
        "[{MethodName}] 会議中メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
    );

    public static void MeetingRunMessageNotifying(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingRunMessageNotifying.Invoke(
            logger,
            methodName,
            conversationId,
            message,
            url,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, string?, string?, Exception?> meetingRunMessageNotified = LoggerMessage.Define<string?, string?, string?, string?>(
        LogLevel.Information,
        new EventId(1010),
        "[{MethodName}] 会議中メッセージを送信しました。ConversationId: {ConversationId}, Message: {Message}, Url {Url}"
    );

    public static void MeetingRunMessageNotified(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        string? message = null,
        string? url = null,
        Exception? exception = null
    )
    {
        meetingRunMessageNotified.Invoke(
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
        new EventId(1011),
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
        new EventId(1012),
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

    private static readonly Action<ILogger, string?, string?, Exception?> settingsInitializing = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(1013),
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
        new EventId(1014),
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

    private static readonly Action<ILogger, string?, string?, Exception?> settingsCancelled = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(1015),
        "[{MethodName}] キャンセルしました。設定は変更されていません。ConversationId: {ConversationId}"
    );

    public static void SettingsCancelled(
        this ILogger logger,
        [CallerMemberName()] string? methodName = null,
        string? conversationId = null,
        Exception? exception = null
    )
    {
        settingsCancelled.Invoke(
            logger,
            methodName,
            conversationId,
            exception
        );
    }

    private static readonly Action<ILogger, string?, string?, Exception?> membersAdded = LoggerMessage.Define<string?, string?>(
        LogLevel.Information,
        new EventId(1017),
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
        new EventId(1019),
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

}
