using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.model
{
  public  class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string pwd { get; set; }
        public string cookie { get; set; }
        public int retrycount { get; set; }
        public int status { get; set; }
    }
}
