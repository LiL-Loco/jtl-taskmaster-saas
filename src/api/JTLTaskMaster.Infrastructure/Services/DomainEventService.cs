using JTLTaskMaster.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JTLTaskMaster.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;using JTLTaskMaster.Application.Common.Interfaces; using JTLTaskMaster.Domain.Common; using MediatR; using Microsoft.Extensions.Logging;  namespace JTLTaskMaster.Infrastructure.Services;  public class DomainEventService : IDomainEventService {     private readonly IPublisher _mediator;     private readonly ILogger<DomainEventService> _logger;      public DomainEventService(         IPublisher mediator,         ILogger<DomainEventService> logger)     {         _mediator = mediator;         _logger = logger;     }      public async Task PublishAsync(DomainEvent domainEvent)     {         _logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);         await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));     }      private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)     {         return (INotification)Activator.CreateInstance(             typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;     } }  // Add this class if it doesn't exist elsewhere public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : DomainEvent {     public DomainEventNotification(TDomainEvent domainEvent)     {         DomainEvent = domainEvent;     }      public TDomainEvent DomainEvent { get; } }
