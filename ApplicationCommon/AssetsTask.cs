using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Name3.ApplicationCommon
{
    struct AssetsTask
    {
        public string Url { set; get; }
        public string Place { set; get; }
        public Action OnDownloadFaild { set; get; }
        public Action OnDownloadDone { set; get; }
    }
}
