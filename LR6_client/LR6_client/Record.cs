using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR6_client
{
    class Record
    {
        public string mark { get; set; }
        public int price { get; set; }
        public string name { get; set; }
        public  Record(string mark, int price, string name)
        {
            this.mark = mark;
            this.price = price;
            this.name = name;
        }
        public Record() { }
    }
}
