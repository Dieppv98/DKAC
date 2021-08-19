using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DKAC.Common
{
    public class Encryption
    {
        public static string EncryptPassword(string password)
        {
            // hash password
            return SecurityUtility.EncryptBase64(SecurityUtility.EncryptMd5(password), CommonConstants.KeyPassWord);
        }
        public static bool CheckPassword(string password, string hashPassword, string salt)
        {
            var flag = hashPassword.Equals(EncryptPassword(password));
            if (!flag)
            {
                List<string> PasswordFormat = new List<string>();
                PasswordFormat.Add("SHA1");
                PasswordFormat.Add("MD5");
                PasswordFormat.Add("SHA256");
                for (int n = 0; n < PasswordFormat.Count; n++)
                {
                    var algorithm = HashAlgorithm.Create(PasswordFormat[n]);
                    if (algorithm == null)
                    {
                        return false;
                    }
                    var plainTextBytes = Encoding.UTF8.GetBytes(password);
                    var saltBytes = Encoding.UTF8.GetBytes(salt);
                    var plainTextWithSaltBytes =
               new byte[plainTextBytes.Length + saltBytes.Length];

                    for (var i = 0; i < plainTextBytes.Length; i++)
                    {
                        plainTextWithSaltBytes[i] = plainTextBytes[i];
                    }
                    for (var i = 0; i < saltBytes.Length; i++)
                    {
                        plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];
                    }
                    var hashBytes = algorithm.ComputeHash(plainTextWithSaltBytes);

                    var bam = Convert.ToBase64String(hashBytes);
                    if (bam == hashPassword)
                    {
                        return true;
                    }
                }
            }
            return flag;
        }
    }
}