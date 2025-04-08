using chabcav.domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.application.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, string>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var verifyOTP = await _userRepository.VerifyOTP(request.EmailAddress, request.Otp);
                if (!verifyOTP)
                {
                    return "Invalid OTP";
                }
                var result = await _userRepository.ResetPassword(request.EmailAddress, request.CurrentPassword, request.NewPassword);

                return "Reset Password Succesfully Changed";
            }
            catch (Exception ex)
            {

                return "Reset Password Failed";
            }
            
        }

        
    }
}
