using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models.DTOs
{
    /// <summary>
    /// Clase que representa una categoria con sus elementos relacionados
    /// </summary>
    public class CategoryDTO
    {
        public int IdCategory { get; set; }
        public string Name { get; set; }
        public string NamePhoto { get; set; }
        public Nullable<int> FatherCategory { get; set; }
        public List<ProductDTO> ListProducts { get; set; }
    }
}