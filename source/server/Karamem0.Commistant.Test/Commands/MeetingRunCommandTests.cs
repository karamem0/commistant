//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AutoMapper;
using Karamem0.Commistant.Mappings;
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
public class MeetingRunCommandTests
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
            MeetingRunning = true,
            ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
            ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
            MeetingRunSchedule = 10,
            MeetingRunMessage = "Example message",
            MeetingRunUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IBotConnectorService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(DateTime.Parse("2000-01-01T09:20:00Z"));
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns(BinaryData.Empty);
        var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<AutoMapperProfile>();
                config.AddProfile(new MeetingRunCommand.AutoMapperProfile(qrCodeService));
            }
        );
        var mapper = mapperConfig.CreateMapper();
        var logger = Substitute.For<ILogger<MeetingRunCommand>>();
        // Execute
        var target = new MeetingRunCommand(
            connectorClientService,
            dateTimeService,
            mapper,
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
    public async Task ExecuteAsync_Failure_WhenOffSchedule()
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
            MeetingRunning = true,
            ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
            ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
            MeetingRunSchedule = 10,
            MeetingRunMessage = "Example message",
            MeetingRunUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IBotConnectorService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(DateTime.Parse("2000-01-01T09:25:00Z"));
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns(BinaryData.Empty);
        var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<AutoMapperProfile>();
                config.AddProfile(new MeetingRunCommand.AutoMapperProfile(qrCodeService));
            }
        );
        var mapper = mapperConfig.CreateMapper();
        var logger = Substitute.For<ILogger<MeetingRunCommand>>();
        // Execute
        var target = new MeetingRunCommand(
            connectorClientService,
            dateTimeService,
            mapper,
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
    public async Task ExecuteAsync_Failure_WhenNotMeetingRun()
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
            MeetingRunning = false,
            ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
            ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
            MeetingRunSchedule = 10,
            MeetingRunMessage = "Example message",
            MeetingRunUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IBotConnectorService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(DateTime.Parse("2000-01-01T09:20:00Z"));
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns(BinaryData.Empty);
        var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<AutoMapperProfile>();
                config.AddProfile(new MeetingRunCommand.AutoMapperProfile(qrCodeService));
            }
        );
        var mapper = mapperConfig.CreateMapper();
        var logger = Substitute.For<ILogger<MeetingRunCommand>>();
        // Execute
        var target = new MeetingRunCommand(
            connectorClientService,
            dateTimeService,
            mapper,
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
            MeetingRunning = true,
            ScheduledStartTime = null,
            ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
            MeetingRunSchedule = 10,
            MeetingRunMessage = "Example message",
            MeetingRunUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IBotConnectorService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(DateTime.Parse("2000-01-01T09:20:00Z"));
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns(BinaryData.Empty);
        var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<AutoMapperProfile>();
                config.AddProfile(new MeetingRunCommand.AutoMapperProfile(qrCodeService));
            }
        );
        var mapper = mapperConfig.CreateMapper();
        var logger = Substitute.For<ILogger<MeetingRunCommand>>();
        // Execute
        var target = new MeetingRunCommand(
            connectorClientService,
            dateTimeService,
            mapper,
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
            MeetingRunning = true,
            ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
            ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
            MeetingRunSchedule = -1,
            MeetingRunMessage = "Example message",
            MeetingRunUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IBotConnectorService>();
        _ = connectorClientService
            .SendActivityAsync(new Uri("https://www.example.com/"), Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(DateTime.Parse("2000-01-01T09:20:00Z"));
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns(BinaryData.Empty);
        var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<AutoMapperProfile>();
                config.AddProfile(new MeetingRunCommand.AutoMapperProfile(qrCodeService));
            }
        );
        var mapper = mapperConfig.CreateMapper();
        var logger = Substitute.For<ILogger<MeetingRunCommand>>();
        // Execute
        var target = new MeetingRunCommand(
            connectorClientService,
            dateTimeService,
            mapper,
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
