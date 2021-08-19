using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DKAC.Common
{
    public class SecurityUtility
    {
        public static string EncryptMd5(string yourString)
        {
            var myByte = MD5.Create().ComputeHash(Encoding.Default.GetBytes(yourString));
            var myEncryptString = string.Empty;
            for (var i = 0; i < 16; i++)
                myEncryptString += myByte[i].ToString("x2");
            return myEncryptString;
        }

        public static string EncryptBase64(string originalString, string key)
        {
            try
            {
                var bytes = Encoding.ASCII.GetBytes(key);
                if (string.IsNullOrEmpty(originalString))
                    throw new ArgumentNullException("The string which needs to be encrypted can not be null.");

                var cryptoProvider = new DESCryptoServiceProvider
                {
                    Padding = PaddingMode.PKCS7,
                    Mode = CipherMode.ECB,
                    Key = bytes,
                    IV = bytes
                };
                var memoryStream = new MemoryStream();
                var cryptoStream =
                    new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(), CryptoStreamMode.Write);

                var writer = new StreamWriter(cryptoStream);
                writer.Write(originalString);
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();

                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
            catch
            {
                throw new Exception("EncryptBase64 Exception.");
            }
        }

        public static string DecryptBase64(string cryptedString, string key)
        {
            try
            {
                var x = Encoding.ASCII.GetBytes(key);
                if (string.IsNullOrEmpty(cryptedString))
                    throw new ArgumentNullException("The string which needs to be decrypted can not be null.");

                var cryptoProvider = new DESCryptoServiceProvider
                {
                    Padding = PaddingMode.PKCS7,
                    Mode = CipherMode.ECB,
                    Key = x,
                    IV = x
                };
                var memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
                var cryptoStream =
                    new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(), CryptoStreamMode.Read);
                var reader = new StreamReader(cryptoStream);

                return reader.ReadToEnd();
            }
            catch
            {
                throw new Exception("EncryptBase64 Exception.");
            }
        }
    }
}