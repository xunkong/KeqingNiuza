using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Core.XunkongApi
{
    internal class XunkongException : Exception
    {

        public int Code { get; private set; }


        public XunkongException() { }


        public XunkongException(int code, string message = null) : base(message)
        {
            Code = code;
        }

    }
}
