using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models.DTOs
{
    public class CategoryDTO
    {
        public int IdCategory { get; set; }
        public string Name { get; set; }
        public string NamePhoto { get; set; }
        public List<ProductDTO> listProducts { get; set; }
    }
}