﻿using Application.Common.Interfaces;
using Application.Common.Localizations;
using Application.Common.Models.Auth;
using Domain.Identity;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class AuthenticationManager : IAuthenticationService
    {
       // private readonly IApplicationDbContext _applicationDbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;
        


        public AuthenticationManager(UserManager<User> userManager, SignInManager<User> signInManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
           // _applicationDbContext = applicationDbContext;

        }
        public async Task<string> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken)
        {
            var user = createUserDto.MapToUser();

            var identityResult = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!identityResult.Succeeded)
            {
                var failures = identityResult.Errors
                    .Select(x => new ValidationFailure(x.Code, x.Description));

                throw new ValidationException(failures);
            }

            return user.Id;

        }

        public async Task<string> GenerateEmailActivationTokenAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);

        }

        public Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken)
        {
            return _userManager.Users.AnyAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<JwtDto> LoginAsync(AuthLoginRequest authLoginRequest, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(authLoginRequest.Email);

            var loginResult = await _signInManager.PasswordSignInAsync(user, authLoginRequest.Password, false, false);

            if (!loginResult.Succeeded)
            {
                throw new ValidationException(CreateValidationFailure);
            }

            return _jwtService.Generate(user.Id, user.Email, user.FirstName, user.LastName);
        }

        public async Task<JwtDto> SocialLoginAsync(string email, string firstName, string lastName, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is not null)
                return _jwtService.Generate(user.Id, user.Email, user.FirstName, user.LastName);

            var userId = Guid.NewGuid().ToString();

            user = new User()
            {
                Id = userId,
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName,
                CreatedOn = DateTimeOffset.Now,
                CreatedByUserId = userId,
            };

            var identityResult = await _userManager.CreateAsync(user);

            if (!identityResult.Succeeded)
            {
                var failures = identityResult.Errors
                    .Select(x => new ValidationFailure(x.Code, x.Description));

                throw new ValidationException(failures);
            }

            return _jwtService.Generate(user.Id, user.Email, user.FirstName, user.LastName);
        }

        private List<ValidationFailure> CreateValidationFailure => new List<ValidationFailure>()
        {
                new ValidationFailure("Identity Verified","Your identity has been verified") //Email, Password

        };


    }

}
