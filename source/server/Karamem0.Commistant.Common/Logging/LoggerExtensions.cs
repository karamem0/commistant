//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Logging
{

    public static class LoggerExtensions
    {

        private static readonly Action<ILogger, Exception?> unhandledError =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(1),
                "予期しない問題が発生しました。");

        public static void UnhandledError(this ILogger logger, Exception? exception)
        {
            unhandledError.Invoke(logger, exception);
        }

        private static readonly Action<ILogger, string, string?, Exception?> meetingStarted =
            LoggerMessage.Define<string, string?>(
                LogLevel.Information,
                new EventId(1001),
                "会議を開始しました。ConversationId: {ConversationId}, MeetingId: {MeetingId}");

        public static void MeetingStarted(this ILogger logger, IActivity activity, string? meetingId)
        {
            meetingStarted.Invoke(
                logger,
                activity.Conversation.Id,
                meetingId,
                null);
        }

        private static readonly Action<ILogger, string, string?, Exception?> meetingEnded =
            LoggerMessage.Define<string, string?>(
                LogLevel.Information,
                new EventId(1002),
                "会議を終了しました。ConversationId: {ConversationId}, MeetingId: {MeetingId}");

        public static void MeetingEnded(this ILogger logger, IActivity activity, string? meetingId)
        {
            meetingEnded.Invoke(
                logger,
                activity.Conversation.Id,
                meetingId,
                null);
        }

        private static readonly Action<ILogger, Exception?> timerStarted =
            LoggerMessage.Define(
                LogLevel.Information,
                new EventId(1003),
                "タイマーを開始しました。");

        public static void TimerStarted(this ILogger logger)
        {
            timerStarted.Invoke(
                logger,
                null);
        }

        private static readonly Action<ILogger, Exception?> timerEnded =
            LoggerMessage.Define(
                LogLevel.Information,
                new EventId(1004),
                "タイマーを終了しました。");

        public static void TimerEnded(this ILogger logger)
        {
            timerEnded.Invoke(
                logger,
                null);
        }

        private static readonly Action<ILogger, string, string?, Exception?> timerExecuting =
            LoggerMessage.Define<string, string?>(
                LogLevel.Information,
                new EventId(1005),
                "タイマーを実行しています。ConversationId: {ConversationId}, MeetingId: {MeetingId}");

        public static void TimerExecuting(this ILogger logger, ConversationReference reference, string? meetingId)
        {
            timerExecuting.Invoke(
                logger,
                reference.Conversation.Id,
                meetingId,
                null);
        }

        private static readonly Action<ILogger, string, string?, Exception?> timerExecuted =
            LoggerMessage.Define<string, string?>(
                LogLevel.Information,
                new EventId(1006),
                "タイマーを実行しました。ConversationId: {ConversationId}, MeetingId: {MeetingId}");

        public static void TimerExecuted(this ILogger logger, ConversationReference reference, string? meetingId)
        {
            timerExecuted.Invoke(
                logger,
                reference.Conversation.Id,
                meetingId,
                null);
        }

        private static readonly Action<ILogger, string, string?, Exception?> startMeetingMessageSending =
            LoggerMessage.Define<string, string?>(
                LogLevel.Information,
                new EventId(1007),
                "会議開始メッセージを送信します。ConversationId: {ConversationId}, Message: {Message}");

        public static void StartMeetingMessageSending(this ILogger logger, ConversationReference reference, string? message)
        {
            startMeetingMessageSending.Invoke(
                logger,
                reference.Conversation.Id,
                message,
                null);
        }

        private static readonly Action<ILogger, string, string?, Exception?> startMeetingUrlSending =
            LoggerMessage.Define<string, string?>(
                LogLevel.Information,
                new EventId(1008),
                "会議開始 URL を送信します。ConversationId: {ConversationId}, Url: {Url}");

        public static void StartMeetingUrlSending(this ILogger logger, ConversationReference reference, string? url)
        {
            startMeetingUrlSending.Invoke(
                logger,
                reference.Conversation.Id,
                url,
                null);
        }

        private static readonly Action<ILogger, string, string?, Exception?> endMeetingMessageSending =
            LoggerMessage.Define<string, string?>(
                LogLevel.Information,
                new EventId(1009),
                "会議終了メッセージを送信します。ConversationId: {ConversationId}, Message:  {Message}");

        public static void EndMeetingMessageSending(this ILogger logger, ConversationReference reference, string? message)
        {
            endMeetingMessageSending.Invoke(
                logger,
                reference.Conversation.Id,
                message,
                null);
        }

        private static readonly Action<ILogger, string, string?, Exception?> endMeetingUrlSending =
            LoggerMessage.Define<string, string?>(
                LogLevel.Information,
                new EventId(1010),
                "会議終了 URL を送信します。ConversationId: {ConversationId}, Url: {Url}");

        public static void EndMeetingUrlSending(this ILogger logger, ConversationReference reference, string? url)
        {
            endMeetingUrlSending.Invoke(
                logger,
                reference.Conversation.Id,
                url,
                null);
        }

        private static readonly Action<ILogger, string, string?, Exception?> inMeetingMessageSending =
            LoggerMessage.Define<string, string?>(
                LogLevel.Information,
                new EventId(1011),
                "会議中メッセージを送信します。ConversationId: {ConversationId}, Message:  {Message}");

        public static void InMeetingMessageSending(this ILogger logger, ConversationReference reference, string? message)
        {
            inMeetingMessageSending.Invoke(
                logger,
                reference.Conversation.Id,
                message,
                null);
        }

        private static readonly Action<ILogger, string, string?, Exception?> inMeetingUrlSending =
            LoggerMessage.Define<string, string?>(
                LogLevel.Information,
                new EventId(1012),
                "会議中 URL を送信します。ConversationId: {ConversationId}, Url: {Url}");

        public static void InMeetingUrlSending(this ILogger logger, ConversationReference reference, string? url)
        {
            inMeetingUrlSending.Invoke(
                logger,
                reference.Conversation.Id,
                url,
                null);
        }

        private static readonly Action<ILogger, string, Exception?> settingsUpdating =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(1013),
                "設定を変更します。ConversationId: {ConversationId}");

        public static void SettingsUpdating(this ILogger logger, IActivity activity)
        {
            settingsUpdating.Invoke(
                logger,
                activity.Conversation.Id,
                null);
        }

        private static readonly Action<ILogger, string, Exception?> settingsUpdated =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(1014),
                "設定を変更しました。ConversationId: {ConversationId}");

        public static void SettingsUpdated(this ILogger logger, IActivity activity)
        {
            settingsUpdated.Invoke(
                logger,
                activity.Conversation.Id,
                null);
        }

        private static readonly Action<ILogger, string, Exception?> settingsResetting =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(1015),
                "設定を初期化します。ConversationId: {ConversationId}");

        public static void SettingsResetting(this ILogger logger, IActivity activity)
        {
            settingsResetting.Invoke(
                logger,
                activity.Conversation.Id,
                null);
        }

        private static readonly Action<ILogger, string, Exception?> settingsReseted =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(1016),
                "設定を初期化しました。ConversationId: {ConversationId}");

        public static void SettingsReseted(this ILogger logger, IActivity activity)
        {
            settingsReseted.Invoke(
                logger,
                activity.Conversation.Id,
                null);
        }

    }

}
