using chabcav.infrastructure.Data.Abstractions;
using chabcav.infrastructure.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var body = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n" +
                "    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\"" +
                " content=\"width=device-width, initial-scale=1.0\">\r\n  " +
                "  <title>Reset Password</title>\r\n</head>\r\n<body>\r\n" +
                "    <h1>Reset Your Password</h1>\r\n  " +
                "  <p>Click the link below to reset your password:</p>\r\n " +
                "   <a href=\"http://localhost:5173/reset-password?email={emailaddress}\">Reset Password</a>\r\n</body>\r\n</html>";

            body = body.Replace("{emailaddress}", request.Email);

            await _emailService.SendEmail(request.Email, body);

            return true;
        }


    }
}
