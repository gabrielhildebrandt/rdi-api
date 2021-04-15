using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RDI.Application.Helpers
{
    public static class CardHelper
    {
        public static Guid GenerateToken(long number, int cvv)
        {
            var digits = number.ToString()[(number.ToString().Length - 4)..].ToList();

            for (var i = 0; i < cvv; i++)
            {
                digits.Insert(0, digits.Last());
                digits.RemoveAt(digits.Count - 1);
            }

            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.Default.GetBytes(string.Join("", digits)));
            return new Guid(hash);
        }
    }
}