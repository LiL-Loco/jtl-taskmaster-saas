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
using Microsoft.Extensions.Configuration;namespace JTLTaskMaster.Infrastructure.Authentication;  public class JwtSettings {     public string Key { get; set; } = string.Empty;     public string Issuer { get; set; } = string.Empty;     public string Audience { get; set; } = string.Empty;     public int ExpiryMinutes { get; set; } }
