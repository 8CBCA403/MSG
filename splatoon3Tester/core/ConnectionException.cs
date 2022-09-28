using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace splatoon3Tester.core
{
    public class ConnectionException : Exception
    {
        public ConnectionException(String s): base(s)
        {
        }

        public ConnectionException(String s, Result rc):base(s)
        {
            //TODO parse the rc module and desc.
        }

        public ConnectionException(Result rc): base(rc.ToString())
        {
            //TODO parse the rc module and desc
        }
    }
}
