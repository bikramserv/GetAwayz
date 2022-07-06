using System;
using System.Text;

namespace AreaManager.Core.Extensions
{
    public static class StringExtensions
    {
        public static string GetInitialsConcatenated(this string userName, int maxNumber = 3)
        {
            if (string.IsNullOrEmpty(userName)) { return string.Empty; }

            var parts = userName.Split(new char[] { '_', ' ', '-' });
            var concatenated = new StringBuilder();
            foreach (var part in parts)
            {
                concatenated.Append(part.Substring(0, 1).ToUpperInvariant());
            }
            return concatenated.ToString();
        }
    }
}
