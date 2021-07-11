using System;

namespace chapter09.lib.Helpers
{
    //The new HashingExtensions class converts our byte array to a SHA1 string
    public static class HashingExtension
    {
        public static string ToSHA1(this byte[] data)
        {
            System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();

            byte[] hash = sha1.ComputeHash(data);

            return Convert.ToBase64String(hash);
        }
    }
}