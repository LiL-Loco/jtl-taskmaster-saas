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
using Microsoft.Extensions.Configuration;using System.IdentityModel.Tokens.Jwt; using System.Security.Claims; using System.Text; using Microsoft.IdentityModel.Tokens; using JTLTaskMaster.Application.Common.Interfaces; using JTLTaskMaster.Domain.Entities;  namespace JTLTaskMaster.Infrastructure.Authentication;  public class JwtService : ITokenService {     private readonly JwtSettings _jwtSettings;      public JwtService(IOptions<JwtSettings> jwtSettings)     {         _jwtSettings = jwtSettings.Value;     }      public string GenerateToken(User user)     {         var claims = new List<Claim>         {             new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),             new(JwtRegisteredClaimNames.Email, user.Email),             new("tenant_id", user.TenantId.ToString())         };          foreach (var role in user.Roles)         {             claims.Add(new Claim(ClaimTypes.Role, role));         }          var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);          var token = new JwtSecurityToken(             issuer: _jwtSettings.Issuer,             audience: _jwtSettings.Audience,             claims: claims,             expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),             signingCredentials: creds         );          return new JwtSecurityTokenHandler().WriteToken(token);     }      public string GenerateRefreshToken()     {         return Convert.ToBase64String(Guid.NewGuid().ToByteArray());     }      public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)     {         var tokenValidationParameters = new TokenValidationParameters         {             ValidateAudience = true,             ValidateIssuer = true,             ValidateIssuerSigningKey = true,             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),             ValidateLifetime = false,             ValidIssuer = _jwtSettings.Issuer,             ValidAudience = _jwtSettings.Audience         };          var tokenHandler = new JwtSecurityTokenHandler();         var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);         return principal;     } }
