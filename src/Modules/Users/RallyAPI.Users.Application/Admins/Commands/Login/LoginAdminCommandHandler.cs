using MediatR;
using RallyAPI.SharedKernel.Results;
using RallyAPI.Users.Application.Abstractions;
using RallyAPI.Users.Domain.ValueObjects;

namespace RallyAPI.Users.Application.Admins.Commands.Login;

internal sealed class LoginAdminCommandHandler
    : IRequestHandler<LoginAdminCommand, Result<LoginAdminResponse>>
{
    private readonly IAdminRepository _adminRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginAdminCommandHandler(
        IAdminRepository adminRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _adminRepository = adminRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<LoginAdminResponse>> Handle(
        LoginAdminCommand request,
        CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return Result.Failure<LoginAdminResponse>(emailResult.Error);

        var admin = await _adminRepository.GetByEmailAsync(
            emailResult.Value,
            cancellationToken);

        if (admin is null)
            return Result.Failure<LoginAdminResponse>(
                Error.Validation("Invalid email or password."));

        var isValidPassword = _passwordHasher.Verify(request.Password, admin.PasswordHash);
        if (!isValidPassword)
            return Result.Failure<LoginAdminResponse>(
                Error.Validation("Invalid email or password."));

        if (!admin.IsActive)
            return Result.Failure<LoginAdminResponse>(
                Error.Validation("Admin account is inactive."));

        var token = _jwtProvider.GenerateAdminToken(admin);

        return new LoginAdminResponse(
            admin.Id,
            admin.Name,
            admin.Role.ToString(),
            token);
    }
}