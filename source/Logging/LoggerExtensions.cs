//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

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
            LoggerMessage.Define(LogLevel.Error, new EventId(1), "予期しない問題が発生しました。");

        public static void UnhandledError(this ILogger logger, Exception? exception)
        {
            unhandledError.Invoke(logger, exception);
        }

        private static readonly Action<ILogger, string, Exception?> meetingStarted =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1001), "会議を開始しました。ID: {meetingId}");

        public static void MeetingStarted(this ILogger logger, string meetingId)
        {
            meetingStarted.Invoke(logger, meetingId, null);
        }

        private static readonly Action<ILogger, string, Exception?> meetingEnded =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1002), "会議を終了しました。ID: {meetingId}");

        public static void MeetingEnded(this ILogger logger, string meetingId)
        {
            meetingEnded.Invoke(logger, meetingId, null);
        }

        private static readonly Action<ILogger, string, Exception?> timerStarted =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1003), "タイマーを開始しました。ID: {meetingId}");

        public static void TimerStarted(this ILogger logger, string meetingId)
        {
            timerStarted.Invoke(logger, meetingId, null);
        }

        private static readonly Action<ILogger, string, Exception?> timerEnded =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1004), "タイマーを終了しました。ID: {meetingId}");

        public static void TimerEnded(this ILogger logger, string meetingId)
        {
            timerEnded.Invoke(logger, meetingId, null);
        }

        private static readonly Action<ILogger, string, Exception?> timerExecuting =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1005), "タイマーを実行しています。ID: {meetingId}");

        public static void TimerExecuting(this ILogger logger, string meetingId)
        {
            timerExecuting.Invoke(logger, meetingId, null);
        }

        private static readonly Action<ILogger, string, Exception?> timerExecuted =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1006), "タイマーを実行しました: {meetingId}");

        public static void TimerExecuted(this ILogger logger, string meetingId)
        {
            timerExecuted.Invoke(logger, meetingId, null);
        }

        private static readonly Action<ILogger, string, Exception?> startMeetingMessageSending =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1007), "会議開始メッセージを送信します: {meetingId}");

        public static void StartMeetingMessageSending(this ILogger logger, string message)
        {
            startMeetingMessageSending.Invoke(logger, message, null);
        }

        private static readonly Action<ILogger, string, Exception?> startMeetingUrlSending =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1008), "会議開始メッセージを送信します: {meetingId}");

        public static void StartMeetingUrlSending(this ILogger logger, string url)
        {
            startMeetingUrlSending.Invoke(logger, url, null);
        }

        private static readonly Action<ILogger, string, Exception?> endMeetingMessageSending =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1009), "会議終了メッセージを送信します: {meetingId}");

        public static void EndMeetingMessageSending(this ILogger logger, string message)
        {
            endMeetingMessageSending.Invoke(logger, message, null);
        }

        private static readonly Action<ILogger, string, Exception?> endMeetingUrlSending =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1010), "会議終了メッセージを送信します: {meetingId}");

        public static void EndMeetingUrlSending(this ILogger logger, string url)
        {
            endMeetingUrlSending.Invoke(logger, url, null);
        }

        private static readonly Action<ILogger, string, Exception?> inMeetingMessageSending =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1011), "会議中メッセージを送信します: {meetingId}");

        public static void InMeetingMessageSending(this ILogger logger, string message)
        {
            inMeetingMessageSending.Invoke(logger, message, null);
        }

        private static readonly Action<ILogger, string, Exception?> inMeetingUrlSending =
            LoggerMessage.Define<string>(LogLevel.Information, new EventId(1012), "会議中メッセージを送信します: {meetingId}");

        public static void InMeetingUrlSending(this ILogger logger, string url)
        {
            inMeetingUrlSending.Invoke(logger, url, null);
        }

    }

}
