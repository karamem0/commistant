//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Mappings;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Mapster;
using MapsterMapper;
using Microsoft.Agents.Core.Models;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Karamem0.Commistant.Commands.Tests;

[Category("Karamem0.Commistant.Commands")]
public class MeetingStartedCommandTests
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
            MeetingInProgress = true,
            ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
            ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
            MeetingStartedSended = false,
            MeetingStartedSchedule = 10,
            MeetingStartedMessage = "Example message",
            MeetingStartedUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(DateTime.Parse("2000-01-01T09:10:00Z"));
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns(BinaryData.Empty);
        TypeAdapterConfig.GlobalSettings.Apply(new MapperConfiguration());
        TypeAdapterConfig.GlobalSettings.Apply(new MeetingStartedCommand.MapperConfiguration(qrCodeService));
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);
        var logger = Substitute.For<ILogger<MeetingStartedCommand>>();
        // Execute
        var target = new MeetingStartedCommand(
            connectorClientService,
            dateTimeService,
            mapper,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.MeetingStartedSended, Is.True);
        _ = connectorClientService
            .Received()
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>());
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
            MeetingInProgress = true,
            ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
            ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
            MeetingStartedSended = false,
            MeetingStartedSchedule = 10,
            MeetingStartedMessage = "Example message",
            MeetingStartedUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(DateTime.Parse("2000-01-01T09:15:00Z"));
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns(BinaryData.Empty);
        TypeAdapterConfig.GlobalSettings.Apply(new MapperConfiguration());
        TypeAdapterConfig.GlobalSettings.Apply(new MeetingStartedCommand.MapperConfiguration(qrCodeService));
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);
        var logger = Substitute.For<ILogger<MeetingStartedCommand>>();
        // Execute
        var target = new MeetingStartedCommand(
            connectorClientService,
            dateTimeService,
            mapper,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.MeetingStartedSended, Is.True);
        _ = connectorClientService
            .Received()
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>());
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
            MeetingInProgress = true,
            ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
            ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
            MeetingStartedSended = false,
            MeetingStartedSchedule = 10,
            MeetingStartedMessage = "Example message",
            MeetingStartedUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(DateTime.Parse("2000-01-01T09:05:00Z"));
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns(BinaryData.Empty);
        TypeAdapterConfig.GlobalSettings.Apply(new MapperConfiguration());
        TypeAdapterConfig.GlobalSettings.Apply(new MeetingStartedCommand.MapperConfiguration(qrCodeService));
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);
        var logger = Substitute.For<ILogger<MeetingStartedCommand>>();
        // Execute
        var target = new MeetingStartedCommand(
            connectorClientService,
            dateTimeService,
            mapper,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.MeetingStartedSended, Is.False);
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

    [Test()]
    public async Task ExecuteAsync_Failure_WhenNotMeetingInProgress()
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
            MeetingInProgress = false,
            ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
            ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
            MeetingStartedSended = false,
            MeetingStartedSchedule = 10,
            MeetingStartedMessage = "Example message",
            MeetingStartedUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(DateTime.Parse("2000-01-01T09:10:00Z"));
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns(BinaryData.Empty);
        TypeAdapterConfig.GlobalSettings.Apply(new MapperConfiguration());
        TypeAdapterConfig.GlobalSettings.Apply(new MeetingStartedCommand.MapperConfiguration(qrCodeService));
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);
        var logger = Substitute.For<ILogger<MeetingStartedCommand>>();
        // Execute
        var target = new MeetingStartedCommand(
            connectorClientService,
            dateTimeService,
            mapper,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.MeetingStartedSended, Is.False);
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>());
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
            MeetingInProgress = true,
            ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
            ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
            MeetingStartedSended = true,
            MeetingStartedSchedule = 10,
            MeetingStartedMessage = "Example message",
            MeetingStartedUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(DateTime.Parse("2000-01-01T09:10:00Z"));
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns(BinaryData.Empty);
        var logger = Substitute.For<ILogger<MeetingStartedCommand>>();
        TypeAdapterConfig.GlobalSettings.Apply(new MapperConfiguration());
        TypeAdapterConfig.GlobalSettings.Apply(new MeetingStartedCommand.MapperConfiguration(qrCodeService));
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);
        // Execute
        var target = new MeetingStartedCommand(
            connectorClientService,
            dateTimeService,
            mapper,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.MeetingStartedSended, Is.True);
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>());
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
            MeetingInProgress = true,
            ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
            ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
            MeetingStartedSended = false,
            MeetingStartedSchedule = -1,
            MeetingStartedMessage = "Example message",
            MeetingStartedUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(DateTime.Parse("2000-01-01T09:10:00Z"));
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns(BinaryData.Empty);
        TypeAdapterConfig.GlobalSettings.Apply(new MapperConfiguration());
        TypeAdapterConfig.GlobalSettings.Apply(new MeetingStartedCommand.MapperConfiguration(qrCodeService));
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);
        var logger = Substitute.For<ILogger<MeetingStartedCommand>>();
        // Execute
        var target = new MeetingStartedCommand(
            connectorClientService,
            dateTimeService,
            mapper,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.MeetingStartedSended, Is.False);
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>());
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
            MeetingInProgress = true,
            ScheduledStartTime = null,
            ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
            MeetingStartedSended = false,
            MeetingStartedSchedule = 10,
            MeetingStartedMessage = "Example message",
            MeetingStartedUrl = "https://www.example.com/",
        };
        var connectorClientService = Substitute.For<IConnectorClientService>();
        _ = connectorClientService
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>())
            .Returns(new ResourceResponse());
        var dateTimeService = Substitute.For<IDateTimeService>();
        _ = dateTimeService
            .GetCurrentDateTime()
            .Returns(DateTime.Parse("2000-01-01T09:10:00Z"));
        var qrCodeService = Substitute.For<IQRCodeService>();
        _ = qrCodeService
            .CreateAsync("https://www.example.com/")
            .Returns(BinaryData.Empty);
        TypeAdapterConfig.GlobalSettings.Apply(new MapperConfiguration());
        TypeAdapterConfig.GlobalSettings.Apply(new MeetingStartedCommand.MapperConfiguration(qrCodeService));
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);
        var logger = Substitute.For<ILogger<MeetingStartedCommand>>();
        // Execute
        var target = new MeetingStartedCommand(
            connectorClientService,
            dateTimeService,
            mapper,
            logger
        );
        await target.ExecuteAsync(commandSettings, conversationReference);
        // Assert
        Assert.That(commandSettings.MeetingStartedSended, Is.False);
        _ = connectorClientService
            .DidNotReceive()
            .SendActivityAsync("https://www.example.com/", Arg.Any<Activity>());
        _ = qrCodeService
            .DidNotReceive()
            .CreateAsync("https://www.example.com/");
    }

}
