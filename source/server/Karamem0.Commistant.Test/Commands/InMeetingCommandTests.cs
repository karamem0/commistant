//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Commands.Tests;

[Category("Karamem0.Commistant.Commands")]
public class InMeetingCommandTests
{

    [Test()]
    public async Task ExecuteAsync_WhenOnSchedule_ShouldSucceed()
    {
        // Setup
        var conversationReference = new ConversationReference()
        {
            Conversation = new ConversationAccount()
            {
                Id = "1234567890",
            },
            ServiceUrl = "https://www.example.com/",
        };
        var commandSettings = new CommandSettings()
        {
            InMeeting = true,
            ScheduledStartTime = new DateTime(
                2000,
                1,
                1,
                9,
                0,
                0,
                DateTimeKind.Utc
            ),
            ScheduledEndTime = new DateTime(
                2000,
                1,
                1,
                9,
                30,
                0,
                DateTimeKind.Utc
            ),
            InMeetingSchedule = 10,
            InMeetingMessage = "Example message",
            InMeetingUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IBotConnectorService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(
                new DateTime(
                    2000,
                    1,
                    1,
                    9,
                    20,
                    0,
                    DateTimeKind.Utc
                )
            );
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<InMeetingCommand>>();
        // Execute
        var target = new InMeetingCommand(
            connectorClientService,
            dateTimeService,
            qrCodeService,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        _ = connectorClientService
            .Received()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .Received()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_WhenOffSchedule_ShouldSkip()
    {
        // Setup
        var conversationReference = new ConversationReference()
        {
            Conversation = new ConversationAccount()
            {
                Id = "1234567890",
            },
            ServiceUrl = "https://www.example.com/",
        };
        var commandSettings = new CommandSettings()
        {
            InMeeting = true,
            ScheduledStartTime = new DateTime(
                2000,
                1,
                1,
                9,
                0,
                1,
                DateTimeKind.Utc
            ),
            ScheduledEndTime = new DateTime(
                2000,
                1,
                1,
                9,
                30,
                0,
                DateTimeKind.Utc
            ),
            InMeetingSchedule = 10,
            InMeetingMessage = "Example message",
            InMeetingUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IBotConnectorService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(
                new DateTime(
                    2000,
                    1,
                    1,
                    9,
                    25,
                    0,
                    DateTimeKind.Utc
                )
            );
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<InMeetingCommand>>();
        // Execute
        var target = new InMeetingCommand(
            connectorClientService,
            dateTimeService,
            qrCodeService,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_WhenNotInMeeting_ShouldSkip()
    {
        // Setup
        var conversationReference = new ConversationReference()
        {
            Conversation = new ConversationAccount()
            {
                Id = "1234567890",
            },
            ServiceUrl = "https://www.example.com/",
        };
        var commandSettings = new CommandSettings()
        {
            InMeeting = false,
            ScheduledStartTime = new DateTime(
                2000,
                1,
                1,
                9,
                0,
                0,
                DateTimeKind.Utc
            ),
            ScheduledEndTime = new DateTime(
                2000,
                1,
                1,
                9,
                30,
                0,
                DateTimeKind.Utc
            ),
            InMeetingSchedule = 10,
            InMeetingMessage = "Example message",
            InMeetingUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IBotConnectorService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(
                new DateTime(
                    2000,
                    1,
                    1,
                    9,
                    20,
                    0,
                    DateTimeKind.Utc
                )
            );
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<InMeetingCommand>>();
        // Execute
        var target = new InMeetingCommand(
            connectorClientService,
            dateTimeService,
            qrCodeService,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_WhenInvalidSchedule_ShouldSkip()
    {
        // Setup
        var conversationReference = new ConversationReference()
        {
            Conversation = new ConversationAccount()
            {
                Id = "1234567890",
            },
            ServiceUrl = "https://www.example.com/",
        };
        var commandSettings = new CommandSettings()
        {
            InMeeting = true,
            ScheduledStartTime = null,
            ScheduledEndTime = new DateTime(
                2000,
                1,
                1,
                9,
                30,
                0,
                DateTimeKind.Utc
            ),
            InMeetingSchedule = 10,
            InMeetingMessage = "Example message",
            InMeetingUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IBotConnectorService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(
                new DateTime(
                    2000,
                    1,
                    1,
                    9,
                    20,
                    0,
                    DateTimeKind.Utc
                )
            );
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<InMeetingCommand>>();
        // Execute
        var target = new InMeetingCommand(
            connectorClientService,
            dateTimeService,
            qrCodeService,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_WhenNotScheduled_ShouldSkip()
    {
        // Setup
        var conversationReference = new ConversationReference()
        {
            Conversation = new ConversationAccount()
            {
                Id = "1234567890",
            },
            ServiceUrl = "https://www.example.com/",
        };
        var commandSettings = new CommandSettings()
        {
            InMeeting = true,
            ScheduledStartTime = new DateTime(
                2000,
                1,
                1,
                9,
                0,
                0,
                DateTimeKind.Utc
            ),
            ScheduledEndTime = new DateTime(
                2000,
                1,
                1,
                9,
                30,
                0,
                DateTimeKind.Utc
            ),
            InMeetingSchedule = -1,
            InMeetingMessage = "Example message",
            InMeetingUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IBotConnectorService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(
                new DateTime(
                    2000,
                    1,
                    1,
                    9,
                    20,
                    0,
                    DateTimeKind.Utc
                )
            );
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<InMeetingCommand>>();
        // Execute
        var target = new InMeetingCommand(
            connectorClientService,
            dateTimeService,
            qrCodeService,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

}
