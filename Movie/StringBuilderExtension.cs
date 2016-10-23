using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie
{
    public static class StringBuilderExtension
    {
        public static void AppendWithPreceedingTab(this StringBuilder sb, string text,int countOfTab)
        {
            sb.Append(new String('\t', countOfTab));
            sb.Append(text);
        }
        public static void AppendLineWithPreceedingTab(this StringBuilder sb, string text, int countOfTab)
        {
            sb.AppendWithPreceedingTab(text, countOfTab);
            sb.AppendLine();
        }

      
    }
}
