using AppointmentManagement.Application.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;

namespace AppointmentManagement.Application.Services.Common
{
    public class EncryptionService
    {
        private readonly string _encryptionKey;
        private readonly AppSettings _appSettings;

        public EncryptionService(AppSettings appSettings)
        {
            _appSettings = appSettings;
            _encryptionKey = _appSettings.EncryptionSettings.EncryptionKey; // Replace with a secure key from config
        }

        /// <summary>
        /// Hashes a password using BCrypt.
        /// </summary>
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verifies a password against its hashed value.
        /// </summary>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        /// <summary>
        /// Encrypts a plain text string using AES encryption.
        /// </summary>
        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
            aes.IV = new byte[16]; // Initialization Vector

            using var encryptor = aes.CreateEncryptor();
            byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Decrypts an AES-encrypted string back to plain text.
        /// </summary>
        public string Decrypt(string encryptedText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
            aes.IV = new byte[16]; // Initialization Vector

            using var decryptor = aes.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
