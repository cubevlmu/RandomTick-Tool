using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Name3.ApplicationCommon
{
    class AppInfo
    {
        public static string Version { get; } = "1.0";
        public static string Date { get; } = "20221208";
        public static Uri HelpPageUrl { set; get; } = new Uri("http://sunrisestudio.top");
    }
}
