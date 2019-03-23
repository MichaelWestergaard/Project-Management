using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace project_management
{
    class Utilities
    {
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"+\@.+\..+");
        }
    }
}
