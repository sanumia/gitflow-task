using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketsAnalysis;

public class BracketsAnalyzer
{
    public static bool IsValid(string s)
    {
        var closeToOpen = new Dictionary<char, char>
        {
            { ')', '('  },
            { '}', '{' },
            { ']', '[' }
        };

        var nestingLevel = new Dictionary<char, int>()
        {
            { '(', 0 },
            { '{', 0 },
            { '[', 0 }
        };

        foreach (char c in s)
        {
            if (closeToOpen.ContainsKey(c))
            {
                char openBracket = closeToOpen[c];
                nestingLevel[openBracket]--;
                if (nestingLevel[openBracket] < 0)
                {
                    return false;
                }
            }
            else
            {
                nestingLevel[c]++;
            }
        }
        return nestingLevel.Values.All(level => level == 0); ;
    }
}
