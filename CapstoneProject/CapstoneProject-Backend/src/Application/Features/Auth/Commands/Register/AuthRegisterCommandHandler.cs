using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Domain.Settings;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public class AuthRegisterCommandHandler: IRequestHandler<AuthRegisterCommand, AuthRegisterDto>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IJwtService _jwtService;
    private readonly IApplicationDbContext _applicationDbContext;

    public AuthRegisterCommandHandler(IAuthenticationService authenticationService, IJwtService jwtService, IApplicationDbContext applicationDbContext)
    {
        _authenticationService = authenticationService;
        _jwtService = jwtService;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<AuthRegisterDto> Handle(AuthRegisterCommand request, CancellationToken cancellationToken)
    {
        var createUserDto = new CreateUserDto(request.FirstName, request.LastName, request.Email, request.Password);

        var userId = await _authenticationService.CreateUserAsync(createUserDto, cancellationToken);

        var emailToken = await _authenticationService.GenerateEmailActivationTokenAsync(userId, cancellationToken);

        var fullName = $"{request.FirstName} {request.LastName}";

        var jwtDto = _jwtService.Generate(userId, request.Email, request.FirstName, request.LastName);
        
        // Create notification settings for the user
        var notificationSettings = new NotificationSettings()
        {
            UserId = userId,
            PushNotification = false,
            EmailNotification = false,
            EmailAddress = request.Email
        };
        
        await _applicationDbContext.NotificationSettings.AddAsync(notificationSettings, cancellationToken);
        
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return new AuthRegisterDto(request.Email, fullName, jwtDto.AccessToken);
    }
}