using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace billing_system
{
    internal static class AppSession
    {
        public static User? CurrentUser { get; set; }
    }
}

