using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EneaWPF
{
    class Disconnection
    {
        public string Area { get; private set; }
        public string Data { get; private set; }
        public string Details { get; private set; }

        public Disconnection(string area, string data, string details)
        {
            Area = area;
            Data = data;
            Details = details;
        }

        public override string ToString()
        {
            return Area;
        }

    }
}
