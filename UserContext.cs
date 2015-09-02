using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iIECaB
{
    public class UserContext
    {
        public bool Authenticated { get; set; }
        public string userName { get; set; }
        public int MyProperty { get; set; }
        public int UserRefno { get; set; }
    }
}