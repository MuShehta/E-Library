using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Library.Model
{
    public class Buy
    {
        public int id { get; set; }
        public User user { get; set; }
        public Book book { get; set; }
        public DateTime date { get; set; }
    }

    public class IBuy
    {
        public int id { get; set; }
        public int user { get; set; }
        public int book { get; set; }
        public DateTime date { get; set; }
    }

}
