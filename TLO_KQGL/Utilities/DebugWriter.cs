using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace TLO_KQGL.Utilities
{
    public class DebugWriter:TextWriter
    {
        public override void WriteLine(string value)
        {
            Debug.WriteLine(value);
        }
        public override void WriteLine(string format, params object[] arg)
        {
            Debug.WriteLine(format, arg);
        }
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

    }
}