using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class SaveSlot
    {
        public string Source;
        public string Destination;
        public string Order;
        public SaveSlot(string src, string dst, string ordr)
        {
            this.Source = src;
            this.Destination = dst;
            this.Order = ordr;
        }
    }
}
