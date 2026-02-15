using System.Security.Cryptography;
using System.Text;
using Azure.Core;

namespace BWA.Utility
{
    public static class Utils
    {
        private static readonly string key = "0000000000000000000000000000000!!";
        private static readonly string iv = "000000000000000";

        public static DateTime CurrentDateTime = DateTime.UtcNow;
        public static string RootPath = string.Empty;
        public static string IpAddress = string.Empty;
        public static string LocalTime = string.Empty;

        public static string GetFullName(string salutation, string firstName, string lastName)
        {
            return $"{salutation}. {firstName} {lastName}";
        }

        public static byte[] Base64ToBytes(string base64String)
        {

            if (string.IsNullOrWhiteSpace(base64String))
                throw new ArgumentException("Base64 string cannot be null or empty.");

            try
            {
                return Convert.FromBase64String(base64String);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Invalid Base64 string format.", ex);
            }
        }

        public static string BytesToBase64(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentException("Byte array cannot be null or empty.");

            return Convert.ToBase64String(bytes);
        }

        public static string ImageFileToBase64(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Image file not found.", filePath);

            byte[] imageBytes = File.ReadAllBytes(filePath);
            return BytesToBase64(imageBytes);
        }

        public static void Base64ToImageFile(string base64String, string outputPath)
        {
            byte[] imageBytes = Base64ToBytes(base64String);
            File.WriteAllBytes(outputPath, imageBytes);
        }


        public static string Encrypt(string plainText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes("0000000000000000");
            aes.IV = Encoding.UTF8.GetBytes("0000000000000000");

            using var encryptor = aes.CreateEncryptor();
            byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string cipherText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes("0000000000000000");
            aes.IV = Encoding.UTF8.GetBytes("0000000000000000");

            using var decryptor = aes.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }

    }
}
