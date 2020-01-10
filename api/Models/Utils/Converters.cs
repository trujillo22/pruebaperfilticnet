using api.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models.Utils
{
    public class Converters
    {
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