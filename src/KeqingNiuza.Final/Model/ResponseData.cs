using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Model
{
    internal class ResponseData
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }



    internal class ResponseData<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
