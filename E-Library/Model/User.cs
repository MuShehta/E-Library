using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Library.Model
{
    public class User
    {
        public int id { get; set; }
        [Required]
        public string first_name { get; set; }
        [Required]
        public string last_name { get; set; }
        [Required]
        public string user_name { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        public string member_type { get; set; }
        public List<Buy> buy { get; set; }

    }
}
