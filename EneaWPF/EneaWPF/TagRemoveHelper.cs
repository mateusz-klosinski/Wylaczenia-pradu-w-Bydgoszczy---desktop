using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EneaWPF
{
    static class TagRemoveHelper
    {
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);


        public static string CutTagsFromHtml(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }


    }
}

