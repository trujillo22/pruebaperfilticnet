using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models.DTOs
{
    /// <summary>
    /// Clase que representa un Producto con sus relaciones
    /// </summary>
    public class ProductDTO
    {
        public int idProduct { get; set; }
        public string name { get; set; }
        public double weight { get; set; }
        public double price { get; set; }
        public int idCategory { get; set; }
    }
}