/*
 * Helper class for encryptions.
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace RijndaelManagedEncryption
{
    public class RijndaelManagedEncryption
    {
        /// <summary>.
        /// Keep this InputKey safe and prevent someone from decoding it some way!!
        /// </summary>
        internal const string InputKey = "c069ccfc-036d-4b9d-80df-bb75955932fc";

        /// <summary>
        /// Encrypt the given text and give the byte array back as a base64 string.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="salt">The password salt.</param>
        /// <returns>The encrypted text.</returns>
        public static string EncryptRijndael(string text, string salt)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            var aesAlg = NewRijndaelManaged(salt);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        /// <summary>
        /// Checks if a string is base64 encoded.
        /// </summary>
        /// <param name="base64String">The base64 encoded string</param>
        /// <returns>Base64 encoded string</returns>
        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) &&
                   Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        /// <summary>
        /// Decrypts the given text
        /// </summary>
        /// <param name="cipherText">The encrypted base64 text.</param>
        /// <param name="salt">The password salt.</param>
        /// <returns>The decrypted text.</returns>
        public static string DecryptRijndael(string cipherText, string salt)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");

            if (!IsBase64String(cipherText))
                throw new Exception("The cipherText input parameter is not base64 encoded");

            string text;

            var aesAlg = NewRijndaelManaged(salt);
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cipher = Convert.FromBase64String(cipherText);

            using (var msDecrypt = new MemoryStream(cipher))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        text = srDecrypt.ReadToEnd();
                    }
                }
            }

            return text;
        }

        /// <summary>
        /// Create a new RijndaelManaged class and initialize it.
        /// </summary>
        /// <param name="salt">The password salt.</param>
        /// <returns></returns>
        private static RijndaelManaged NewRijndaelManaged(string salt)
        {
            if (salt == null) throw new ArgumentNullException("salt");
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var key = new Rfc2898DeriveBytes(InputKey, saltBytes);

            var aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            return aesAlg;
        }
    }
}