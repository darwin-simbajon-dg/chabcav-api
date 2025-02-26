using chabcav.domain.Services;
using System;
using System.Security.Cryptography;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // 128-bit
    private const int KeySize = 32;  // 256-bit
    private const int Iterations = 10000; // PBKDF2 iterations

    public string HashPassword(string password)
    {
        // Generate a cryptographic random salt
        var salt = new byte[SaltSize];
        //using (var rng = RandomNumberGenerator.Create())
        //{
        //    rng.GetBytes(salt);
        //}

        // Derive the key using PBKDF2
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
        {
            var hash = pbkdf2.GetBytes(KeySize);

            // Combine salt and hash as a single string
            var result = new byte[SaltSize + KeySize];
            Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, result, SaltSize, KeySize);

            // Return as a base64 string
            return Convert.ToBase64String(result);
        }
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        // Decode the base64 string
        var decoded = Convert.FromBase64String(hashedPassword);

        // Extract the salt from the decoded hash
        var salt = new byte[SaltSize];
        Buffer.BlockCopy(decoded, 0, salt, 0, SaltSize);

        // Extract the stored hash
        var storedHash = new byte[KeySize];
        Buffer.BlockCopy(decoded, SaltSize, storedHash, 0, KeySize);

        // Derive the key for the provided password
        using (var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, Iterations, HashAlgorithmName.SHA256))
        {
            var computedHash = pbkdf2.GetBytes(KeySize);

            // Compare the computed hash with the stored hash
            return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
        }
    }
}
