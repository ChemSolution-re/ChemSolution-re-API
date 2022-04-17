using System.Text;

namespace ChemSolution_re_API.Services.JWT.Settings
{
    internal static class RandomKey
    {
        public static string CreateKey(int length)
        {
            Random random = new Random();
            return Enumerable.Range(0, length)
                .Select(_ => random.Next(0, 255))
                .Aggregate((new StringBuilder()), (s, i) => s.Append((char)i))
                .ToString();
        }

    }
}
