using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Malshinon.Tools
{
    public static class CryptoHelper
    {
        // For demonstration only! In production, store the key securely.
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes for AES-128
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("6543210987654321"); // 16 bytes

        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                // Try to decode as Base64 (encrypted)
                var buffer = Convert.FromBase64String(cipherText);
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msDecrypt = new MemoryStream(buffer))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CryptoHelper.Decrypt] Error: {ex.Message}. Returning plain text.");
                return cipherText;
            }
        }
    }
} 