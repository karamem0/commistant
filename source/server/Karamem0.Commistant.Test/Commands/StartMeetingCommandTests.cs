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
public class StartMeetingCommandTests
{

    [Test()]
    public async Task ExecuteAsync_Success_WhenOnSchedule()
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
            StartMeetingSended = false,
            StartMeetingSchedule = 10,
            StartMeetingMessage = "Example message",
            StartMeetingUrl = "https://www.example.com/",
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
                    10,
                    0,
                    DateTimeKind.Utc
                )
            );
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<StartMeetingCommand>>();
        // Execute
        var target = new StartMeetingCommand(
            connectorClientService,
            dateTimeService,
            qrCodeService,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.StartMeetingSended, Is.EqualTo(true));
        _ = connectorClientService
            .Received()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .Received()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_Success_WhenAfterSchedule()
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
            StartMeetingSended = false,
            StartMeetingSchedule = 10,
            StartMeetingMessage = "Example message",
            StartMeetingUrl = "https://www.example.com/",
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
                    15,
                    0,
                    DateTimeKind.Utc
                )
            );
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<StartMeetingCommand>>();
        // Execute
        var target = new StartMeetingCommand(
            connectorClientService,
            dateTimeService,
            qrCodeService,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.StartMeetingSended, Is.EqualTo(true));
        _ = connectorClientService
            .Received()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .Received()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_Failure_WhenBeforeSchedule()
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
            StartMeetingSended = false,
            StartMeetingSchedule = 10,
            StartMeetingMessage = "Example message",
            StartMeetingUrl = "https://www.example.com/",
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
                    5,
                    0,
                    DateTimeKind.Utc
                )
            );
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<StartMeetingCommand>>();
        // Execute
        var target = new StartMeetingCommand(
            connectorClientService,
            dateTimeService,
            qrCodeService,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.StartMeetingSended, Is.EqualTo(false));
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_Failure_WhenNotInMeeting()
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
            StartMeetingSended = false,
            StartMeetingSchedule = 10,
            StartMeetingMessage = "Example message",
            StartMeetingUrl = "https://www.example.com/",
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
                    10,
                    0,
                    DateTimeKind.Utc
                )
            );
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<StartMeetingCommand>>();
        // Execute
        var target = new StartMeetingCommand(
            connectorClientService,
            dateTimeService,
            qrCodeService,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.StartMeetingSended, Is.EqualTo(false));
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_Failure_WhenAfterSended()
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
            StartMeetingSended = true,
            StartMeetingSchedule = 10,
            StartMeetingMessage = "Example message",
            StartMeetingUrl = "https://www.example.com/",
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
                    10,
                    0,
                    DateTimeKind.Utc
                )
            );
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<StartMeetingCommand>>();
        // Execute
        var target = new StartMeetingCommand(
            connectorClientService,
            dateTimeService,
            qrCodeService,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.StartMeetingSended, Is.EqualTo(true));
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_Failure_WhenInvalidSchedule()
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
            StartMeetingSended = false,
            StartMeetingSchedule = -1,
            StartMeetingMessage = "Example message",
            StartMeetingUrl = "https://www.example.com/",
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
                    10,
                    0,
                    DateTimeKind.Utc
                )
            );
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<StartMeetingCommand>>();
        // Execute
        var target = new StartMeetingCommand(
            connectorClientService,
            dateTimeService,
            qrCodeService,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.StartMeetingSended, Is.EqualTo(false));
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_Failure_WhenNotScheduled()
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
            StartMeetingSended = false,
            StartMeetingSchedule = 10,
            StartMeetingMessage = "Example message",
            StartMeetingUrl = "https://www.example.com/",
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
                    10,
                    0,
                    DateTimeKind.Utc
                )
            );
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<StartMeetingCommand>>();
        // Execute
        var target = new StartMeetingCommand(
            connectorClientService,
            dateTimeService,
            qrCodeService,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.StartMeetingSended, Is.EqualTo(false));
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

}
