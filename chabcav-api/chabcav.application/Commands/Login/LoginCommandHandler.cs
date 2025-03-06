using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IUserRepository _userRepository;

        public LoginCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            var user = await _userRepository.AuthenticateAsync(request.Email, request.Password);

            var token = GenerateJwtToken(user);

            return token;
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DGTechLab01092025ChabacanoLearningSystem1235"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Sid, user.Id.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
