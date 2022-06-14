using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Library.Model
{
    public class BookForImage
    {
        public int id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string author { get; set; }
        [Required]
        public string publisher { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public int available_amount { get; set; }
        [Required]
        public int sold_amount { get; set; }
        [Required]
        public IFormFile file { get; set; }
        public List<Buy> buy { get; set; }
    }
}
