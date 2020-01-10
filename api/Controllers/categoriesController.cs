using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using api.Models;
using api.Models.DTOs;
using api.Models.Utils;

namespace api.Controllers
{
    /// <summary>
    /// Clase que representa el controlador para el servicio de categories
    /// </summary>
    public class categoriesController : ApiController
    {
        private perfilticEntities db = new perfilticEntities();

        /// <summary>
        /// MEtodo que permite obtener toda la informacion basica de todas las categorias de la BD
        /// </summary>
        /// <returns>una list de categorias</returns>
        // GET: api/categories
        public List<CategoryDTO> Getcategory()
        {
            List<CategoryDTO> listCategoriesDTO = new List<CategoryDTO>();
            

            foreach (category categoryEntityTemp in db.category)
            {
                //List<product> listProductsEntity = db.product.Where(x => x.idCategory == categoryEntityTemp.idCategory).ToList();
                //List<ProductDTO> listProductsDTO = new List<ProductDTO>();
                /*
                foreach (product productEntityTemp in listProductsEntity)
                {
                    listProductsDTO.Add(Converters.ConverterProductEntityToProductDTO(productEntityTemp));
                }   
                */

                listCategoriesDTO.Add(Converters.ConverterCategoryEntityToCategoryDTO(categoryEntityTemp));
            }

            return listCategoriesDTO;
        }

        /// <summary>
        /// Metodo que permite obtener la informacion basica de una categoria sin sus productos
        /// </summary>
        /// <param name="id">int que representa el id de la categoria que se buscara</param>
        /// <returns>un estado 200 con el objeto correspondiente o un 404 en caso contrario</returns>
        // GET: api/categories/5
        [ResponseType(typeof(CategoryDTO))]
        [Route("api/categories/{id}")]
        public IHttpActionResult Getcategory(int id)
        {
            category category = db.category.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(Converters.ConverterCategoryEntityToCategoryDTO(category));
        }

        /// <summary>
        /// Permite obtener la categoria indicada con todos sus productos y si es una categoria padre se mostraran 
        /// todos los productos de sus categorias hijas.
        /// </summary>
        /// <param name="id">int que representa la categoria que se va a buscar</param>
        /// <returns>un estado 200 con el objeto correspondiente o un 404 en caso contrario</returns>
        [ResponseType(typeof(CategoryDTO))]
        [Route("api/categories/products/{id}")]
        public IHttpActionResult GetcategoryAndProducts(int id)
        {
            category category = db.category.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            List<ProductDTO> listProductsDTO = new List<ProductDTO>();
            listChildrenCategories = new List<int>();
            getChildrenCategories(id);
   
            foreach (int idCategoryTemp in listChildrenCategories)
            {
                foreach (product productEntityTemp in db.product.Where(x => x.idCategory == idCategoryTemp))
                {
                    if (listProductsDTO.Where(x => x.idProduct == productEntityTemp.idProduct).Count() == 0)
                    {
                        listProductsDTO.Add(Converters.ConverterProductEntityToProductDTO(productEntityTemp));
                    }
                    
                }
            }

            CategoryDTO categoryDTO = Converters.ConverterCategoryEntityToCategoryDTO(category);

            categoryDTO.ListProducts = listProductsDTO;

            return Ok(categoryDTO);
        }

        /// <summary>
        /// Objeto de tipo list que me permite guardar las categorias hijas.
        /// </summary>
        private List<int> listChildrenCategories;

        /// <summary>
        /// Metodo que permite obtenre una lista de las categorias hijas de la categoria especifica en el parametro
        /// </summary>
        /// <param name="idCategoriaPadre">int que representa la categoria a la cual se le buscaran las categorias hijas</param>
        private void getChildrenCategories(int idCategoriaPadre)
        {
            listChildrenCategories.Add(idCategoriaPadre);
            List<int> list = (from c in db.category
                             where c.fatherCategory == idCategoriaPadre
                             select c.idCategory).ToList();

            if (list.Count == 0)
            {
                return;
            }
            else
            {
                listChildrenCategories.AddRange(list);
                foreach (int idcategoryTemp in list)
                {
                    getChildrenCategories(idcategoryTemp);
                }
            }
        }

        /// <summary>
        /// metodo que permite modificar una categoria
        /// </summary>
        /// <param name="id">identificador de la categoria que se quiere modificar</param>
        /// <param name="category">objeto que contiene la informacion a modificar</param>
        /// <returns>un estado 204 sin contendio</returns>
        // PUT: api/categories/5
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/categories/{id}")]
        public IHttpActionResult Putcategory(int id,[FromBody]category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.idCategory)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!categoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Metodo que permite agregar una categoria a la BD
        /// </summary>
        /// <param name="category">informacion de la categoria que se quiere agregar</param>
        /// <returns>un estado 201 con contenido el cual es la informacion agregada</returns>
        // POST: api/categories
        [ResponseType(typeof(CategoryDTO))]
        public IHttpActionResult Postcategory(category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.category.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.idCategory }, Converters.ConverterCategoryEntityToCategoryDTO(category));
        }

        /// <summary>
        /// metodo que permite eliminar una categoria
        /// </summary>
        /// <param name="id">int que representa el idenficador de la categoria que se quiere eliminar</param>
        /// <returns>un estado 200 con la informacion de la categoria eliminada</returns>
        // DELETE: api/categories/5
        [ResponseType(typeof(CategoryDTO))]
        [Route("api/categories/{id}")]
        public IHttpActionResult Deletecategory(int id)
        {
            category category = db.category.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.category.Remove(category);
            db.SaveChanges();

            return Ok(Converters.ConverterCategoryEntityToCategoryDTO(category));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool categoryExists(int id)
        {
            return db.category.Count(e => e.idCategory == id) > 0;
        }
    }
}