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
    public class categoriesController : ApiController
    {
        private perfilticEntities db = new perfilticEntities();

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

        // PUT: api/categories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcategory(int id, category category)
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

        // POST: api/categories
        [ResponseType(typeof(category))]
        public IHttpActionResult Postcategory(category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.category.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.idCategory }, category);
        }

        // DELETE: api/categories/5
        [ResponseType(typeof(category))]
        public IHttpActionResult Deletecategory(int id)
        {
            category category = db.category.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.category.Remove(category);
            db.SaveChanges();

            return Ok(category);
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