using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpectNet.NET
{
    class StringContainsMatcher : IMatcher
    {
        private string query;

        public StringContainsMatcher(string query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            this.query = query;
        }
        public bool IsMatch(string text)
        {
            return text.Contains(query);
        }
    }
}
