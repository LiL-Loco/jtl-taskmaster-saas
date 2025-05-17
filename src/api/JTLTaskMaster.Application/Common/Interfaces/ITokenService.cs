using JTLTaskMaster.Domain.Entities;
using System.Threading.Tasks;

namespace JTLTaskMaster.Application.Common.Interfaces;

public interface ITokenService
{
    Task<string> CreateTokenAsync(User user);
    string GenerateRefreshToken();
}