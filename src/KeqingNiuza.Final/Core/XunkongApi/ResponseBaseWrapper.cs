using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Core.XunkongApi
{
    internal class ResponseBaseWrapper<TData>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public TData Data { get; set; }


        public ResponseBaseWrapper() { }

    }

}
