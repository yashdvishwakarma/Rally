using MediatR;
using RallyAPI.SharedKernel.Results;

namespace RallyAPI.Users.Application.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken)
    : IRequest<Result<RefreshTokenResponse>>;

public sealed record RefreshTokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAt);