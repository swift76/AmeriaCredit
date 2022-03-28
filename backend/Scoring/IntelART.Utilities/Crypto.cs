using System;
using System.Text;
using System.Security.Cryptography;

namespace IntelART.Utilities
{
    public class Crypto
    {
        private const string EncryptionSalt = "z0$TuEjM#8";
        private const string EncryptionPassword = "{IlI:@dOs}";

        public static string HashString(string source)
        {
            if (string.IsNullOrEmpty(source))
                return null;
            HMACSHA1 hash = new HMACSHA1(new Rfc2898DeriveBytes(EncryptionPassword, Encoding.ASCII.GetBytes(EncryptionSalt)).GetBytes(16));
            return Convert.ToBase64String(hash.ComputeHash(Encoding.ASCII.GetBytes(source)));
        }
    }
}
