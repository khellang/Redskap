#if NETSTANDARD
namespace Redskap
{
    // .NET Standard polyfills for newer string methods.
    internal static class StringExtensions
    {
        public static bool Contains(this string theString, char value) => theString.IndexOf(value) >= 0;
    }
}
#endif