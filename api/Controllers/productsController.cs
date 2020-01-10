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
    public class productsController : ApiController
    {
        private perfilticEntities db = new perfilticEntities();

        /// <summary>
        /// Metodo que permite obtener todos los productos de la BD
        /// </summary>
        /// <returns>una list de productos</returns>
        // GET: api/products
        public List<ProductDTO> Getproduct()
        {
            List<ProductDTO> listProductsDTO = new List<ProductDTO>();

            foreach (product productEntityTemp in db.product)
            {
                listProductsDTO.Add(Converters.ConverterProductEntityToProductDTO(productEntityTemp));
            }

            return listProductsDTO;
        }

        // GET: api/products/5
        [ResponseType(typeof(product))]
        public IHttpActionResult Getproduct(int id)
        {
            product product = db.product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putproduct(int id, product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.idProduct)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productExists(id))
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

        // POST: api/products
        [ResponseType(typeof(product))]
        public IHttpActionResult Postproduct(product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.product.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.idProduct }, product);
        }

        // DELETE: api/products/5
        [ResponseType(typeof(product))]
        public IHttpActionResult Deleteproduct(int id)
        {
            product product = db.product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.product.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool productExists(int id)
        {
            return db.product.Count(e => e.idProduct == id) > 0;
        }
    }
}