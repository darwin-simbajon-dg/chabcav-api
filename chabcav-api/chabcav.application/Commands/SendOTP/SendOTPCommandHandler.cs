using chabcav.application.Commands.ResetPassword;
using chabcav.domain.Interfaces;
using chabcav.infrastructure.Data.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.SendOTP
{


    public class SendOTPCommandHandler : IRequestHandler<SendOTPCommand, bool>
    {
        
        private readonly IEmailService _emailService;
        private readonly IOTPService _oTPService;
        private readonly IUserRepository _userRepository;

        public SendOTPCommandHandler(IEmailService emailService, IOTPService oTPService, IUserRepository userRepository)
        {
            _emailService = emailService;
            _oTPService = oTPService;
            _userRepository = userRepository;
        }


        public async Task<bool> Handle(SendOTPCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var otp = await _oTPService.GenerateOTP();
                var body = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n" +
                               "    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\"" +
                               " content=\"width=device-width, initial-scale=1.0\">\r\n  " +
                               "  <title>Reset Password</title>\r\n</head>\r\n<body>\r\n" +
                               "    <h1>OTP Verification</h1>\r\n  " +
                               $"  <p>Your requested reset password to verify your request enter this OTP: {otp}</p>\r\n " +
                               "   \r\n</body>\r\n</html>";

                body = body.Replace("{emailaddress}", request.EmailAddress);

                await _emailService.SendEmail(request.EmailAddress, body);

                await _userRepository.SaveOTP(request.EmailAddress, otp);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


    }


}


