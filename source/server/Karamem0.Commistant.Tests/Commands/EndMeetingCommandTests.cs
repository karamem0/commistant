//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Logging;
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
public class EndMeetingCommandTests
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
        var conversationProperty = new ConversationProperty()
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
            EndMeetingSended = false,
            EndMeetingSchedule = 10,
            EndMeetingMessage = "Example message",
            EndMeetingUrl = "https://www.example.com/",
        };
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
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<EndMeetingCommand>>();
        logger.EndMeetingMessageNotifying(conversationReference, conversationProperty);
        logger.EndMeetingMessageNotified(conversationReference, conversationProperty);
        // Execute
        var command = new EndMeetingCommand(
            dateTimeService,
            connectorClientService,
            qrCodeService,
            logger
        );
        await command.ExecuteAsync(conversationProperty, conversationReference);
        // Assert
        Assert.That(conversationProperty.EndMeetingSended, Is.EqualTo(true));
        _ = connectorClientService
            .Received()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .Received()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_WhenAfterSchedule_ShouldSucceed()
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
        var conversationProperty = new ConversationProperty()
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
            EndMeetingSended = false,
            EndMeetingSchedule = 10,
            EndMeetingMessage = "Example message",
            EndMeetingUrl = "https://www.example.com/",
        };
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
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<EndMeetingCommand>>();
        logger.EndMeetingMessageNotifying(conversationReference, conversationProperty);
        logger.EndMeetingMessageNotified(conversationReference, conversationProperty);
        // Execute
        var command = new EndMeetingCommand(
            dateTimeService,
            connectorClientService,
            qrCodeService,
            logger
        );
        await command.ExecuteAsync(conversationProperty, conversationReference);
        // Assert
        Assert.That(conversationProperty.EndMeetingSended, Is.EqualTo(true));
        _ = connectorClientService
            .Received()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .Received()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_WhenBeforeSchedule_ShouldSkip()
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
        var conversationProperty = new ConversationProperty()
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
            EndMeetingSended = false,
            EndMeetingSchedule = 10,
            EndMeetingMessage = "Example message",
            EndMeetingUrl = "https://www.example.com/",
        };
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
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<EndMeetingCommand>>();
        logger.EndMeetingMessageNotifying(conversationReference, conversationProperty);
        logger.EndMeetingMessageNotified(conversationReference, conversationProperty);
        // Execute
        var command = new EndMeetingCommand(
            dateTimeService,
            connectorClientService,
            qrCodeService,
            logger
        );
        await command.ExecuteAsync(conversationProperty, conversationReference);
        // Assert
        Assert.That(conversationProperty.EndMeetingSended, Is.EqualTo(false));
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
        var conversationProperty = new ConversationProperty()
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
            EndMeetingSended = true,
            EndMeetingSchedule = 10,
            EndMeetingMessage = "Example message",
            EndMeetingUrl = "https://www.example.com/",
        };
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
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<EndMeetingCommand>>();
        logger.EndMeetingMessageNotifying(conversationReference, conversationProperty);
        logger.EndMeetingMessageNotified(conversationReference, conversationProperty);
        // Execute
        var command = new EndMeetingCommand(
            dateTimeService,
            connectorClientService,
            qrCodeService,
            logger
        );
        await command.ExecuteAsync(conversationProperty, conversationReference);
        // Assert
        Assert.That(conversationProperty.EndMeetingSended, Is.EqualTo(true));
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_WhenAfterSended_ShouldSkip()
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
        var conversationProperty = new ConversationProperty()
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
            EndMeetingSended = true,
            EndMeetingSchedule = 10,
            EndMeetingMessage = "Example message",
            EndMeetingUrl = "https://www.example.com/",
        };
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
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<EndMeetingCommand>>();
        logger.EndMeetingMessageNotifying(conversationReference, conversationProperty);
        logger.EndMeetingMessageNotified(conversationReference, conversationProperty);
        // Execute
        var command = new EndMeetingCommand(
            dateTimeService,
            connectorClientService,
            qrCodeService,
            logger
        );
        await command.ExecuteAsync(conversationProperty, conversationReference);
        // Assert
        Assert.That(conversationProperty.EndMeetingSended, Is.EqualTo(true));
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
        var conversationProperty = new ConversationProperty()
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
            EndMeetingSended = false,
            EndMeetingSchedule = -1,
            EndMeetingMessage = "Example message",
            EndMeetingUrl = "https://www.example.com/",
        };
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
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<EndMeetingCommand>>();
        logger.EndMeetingMessageNotifying(conversationReference, conversationProperty);
        logger.EndMeetingMessageNotified(conversationReference, conversationProperty);
        // Execute
        var command = new EndMeetingCommand(
            dateTimeService,
            connectorClientService,
            qrCodeService,
            logger
        );
        await command.ExecuteAsync(conversationProperty, conversationReference);
        // Assert
        Assert.That(conversationProperty.EndMeetingSended, Is.EqualTo(false));
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
        var conversationProperty = new ConversationProperty()
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
            ScheduledEndTime = null,
            EndMeetingSended = false,
            EndMeetingSchedule = 10,
            EndMeetingMessage = "Example message",
            EndMeetingUrl = "https://www.example.com/",
        };
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
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns([]);
        var logger = Substitute.For<ILogger<EndMeetingCommand>>();
        logger.EndMeetingMessageNotifying(conversationReference, conversationProperty);
        logger.EndMeetingMessageNotified(conversationReference, conversationProperty);
        // Execute
        var command = new EndMeetingCommand(
            dateTimeService,
            connectorClientService,
            qrCodeService,
            logger
        );
        await command.ExecuteAsync(conversationProperty, conversationReference);
        // Assert
        Assert.That(conversationProperty.EndMeetingSended, Is.EqualTo(false));
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

}
