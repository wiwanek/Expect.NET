using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpectNet.NET
{
    class RegexMatcher : IMatcher
    {
        private System.Text.RegularExpressions.Regex regex;

        public RegexMatcher(System.Text.RegularExpressions.Regex regex)
        {
            this.regex = regex;
        }

        public bool IsMatch(string text)
        {
            return regex.IsMatch(text);
        }
    }
}
