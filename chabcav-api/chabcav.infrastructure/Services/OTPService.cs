using chabcav.infrastructure.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Services
{
    public class OTPService : IOTPService
    {
        public async Task<string> GenerateOTP()
        {
            var otp = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(otp);
            }
            int otpNumber = BitConverter.ToInt32(otp, 0) % 1000000;
            otpNumber = Math.Abs(otpNumber);

            return otpNumber.ToString("D6");
        }
    }
}
