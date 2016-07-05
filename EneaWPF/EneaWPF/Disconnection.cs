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
        public string Date { get; private set; }
        public string Time { get; private set; }
        public string Details { get; private set; }

        public Disconnection(string area, string date, string details)
        {
            Area = area;
            Date = date;
            Details = details;
            Time = Date.Substring(9, Date.Length - 10);
        }

        public override string ToString()
        {
            return Area;
        }

        public DateTime ConvertDate()
        {
            DateTime date = DateTime.MinValue;
            DateTime.TryParse(Date.Substring(0, 10), out date);
            return date;
        }



    }
}
