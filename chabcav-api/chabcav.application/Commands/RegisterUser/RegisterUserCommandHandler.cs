using chabcav.domain.Entities;
using chabcav.domain.Events;
using chabcav.domain.Interfaces;
using chabcav.domain.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMediator _mediator;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IMediator mediator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Check if email already exists
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists.");

            // Hash password
            var hashedPassword = _passwordHasher.HashPassword(request.Password);

            // Create user
            var user = new User(request.Username, request.Email, hashedPassword);
            await _userRepository.AddAsync(user);

            // Raise domain event
            //await _mediator.Publish(new UserRegisteredEvent(user.Id, user.Email), cancellationToken);

            return user.Id;
        }
    }

}
