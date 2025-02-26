using System.Runtime.CompilerServices;

namespace CityTraffic.Extensions
{
    public static class StringExtensions
    {
        [OverloadResolutionPriority(1)]
        public static string ToString(this IEnumerable<char> chars, int uselessParam)
        {
            return new string(chars.ToArray());
        }
    }
}
