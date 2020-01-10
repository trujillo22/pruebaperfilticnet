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

        /// <summary>
        /// Metodo que permite obtener el producto especificado en parametro
        /// </summary>
        /// <param name="id">int que representa el identificador del producto</param>
        /// <returns>un estado 200 con el contenido del producto encontrado o un 400 en caso contrario</returns>
        // GET: api/products/5
        [ResponseType(typeof(ProductDTO))]
        [Route("api/products/{id}")]
        public IHttpActionResult Getproduct(int id)
        {
            product product = db.product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(Converters.ConverterProductEntityToProductDTO(product));
        }

        /// <summary>
        /// Metodo que permite modificar un producto
        /// </summary>
        /// <param name="id">int que representa el identificador del producto que se desea modificar</param>
        /// <param name="product">objeto que contien la informacion del producto a modificar</param>
        /// <returns>un estado 204 cuando se modifica</returns>
        // PUT: api/products/5
        [ResponseType(typeof(void))]
        [Route("api/products/{id}")]
        public IHttpActionResult Putproduct(int id, [FromBody]product product)
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
        /// <summary>
        /// Metodo que permite agregar un producto a la BD
        /// </summary>
        /// <param name="product">objeto que contiene la informacion del producto a agregar</param>
        /// <returns>un estado 201</returns>
        [ResponseType(typeof(product))]
        public IHttpActionResult Postproduct(product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.product.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.idProduct }, Converters.ConverterProductEntityToProductDTO(product));
        }

        /// <summary>
        /// metodo que permit eliminar un producto de la BD
        /// </summary>
        /// <param name="id">int que representa el idenficador a eliminar</param>
        /// <returns>un estado 200 cuando se elimina, o un 400 en caso contrario</returns>
        // DELETE: api/products/5
        [ResponseType(typeof(product))]
        [Route("api/products/{id}")]
        public IHttpActionResult Deleteproduct(int id)
        {
            product product = db.product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.product.Remove(product);
            db.SaveChanges();

            return Ok(Converters.ConverterProductEntityToProductDTO(product));
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