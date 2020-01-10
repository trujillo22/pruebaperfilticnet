using api.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models.Utils
{
    /// <summary>
    /// Clase que contiene los metodo para convertir objetos entre entidades y objetoDTO
    /// </summary>
    public class Converters
    {
        /// <summary>
        /// Metodo que me permite convertir un objeto de Category proveniente del EntityFramework a CategoryDTO
        /// </summary>
        /// <param name="categoryEntity">objeto del entityFramework que contiene la informacion</param>
        /// <returns>un objetoDTO con la informacion correspondiente</returns>
        public static CategoryDTO ConverterCategoryEntityToCategoryDTO(category categoryEntity)
        {
            CategoryDTO categoryDTO = new CategoryDTO { 
                IdCategory = categoryEntity.idCategory,
                Name = categoryEntity.name,
                NamePhoto = categoryEntity.namePhoto,
                FatherCategory = categoryEntity.fatherCategory               
            };

            return categoryDTO;
        }

        /// <summary>
        /// Metodo que me permite convertir un objeto de Product proveniente del EntityFramework a ProductDTO
        /// </summary>
        /// <param name="categoryEntity">objeto del entityFramework que contiene la informacion</param>
        /// <returns>un objetoDTO con la informacion correspondiente</returns>
        public static ProductDTO ConverterProductEntityToProductDTO(product productEntity)
        {
            ProductDTO productDTO = new ProductDTO
            {
                idProduct = productEntity.idProduct,
                name = productEntity.name,
                weight = productEntity.weight,
                price = productEntity.price,
                idCategory = productEntity.idCategory
            };

            return productDTO;
        }


    }
}