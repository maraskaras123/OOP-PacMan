using System.Security.Cryptography;
using System.Text;

namespace PacMan.Shared.Helpers
{
    public static class RandomGenerator
    {
        private static readonly char[] Chars = "abcdefghijkmnopqrstwxyz1234567890".ToCharArray();

        public static string GenerateRandomText(int length)
        {
            var data = new byte[4 * length];
            RandomNumberGenerator.Fill(data);

            var result = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % Chars.Length;
                result.Append(Chars[idx]);
            }

            return result.ToString();
        }
    }
}