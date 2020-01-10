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

namespace api.Controllers
{
    /// <summary>
    /// Clase que representa el controlador para el servicio de users
    /// </summary>
    public class usersController : ApiController
    {
        private perfilticEntities db = new perfilticEntities();

        /// <summary>
        /// Metodo que permite obtener todos los usuarios de la bd
        /// </summary>
        /// <returns>una list que contiene todos los usuarios de la bd</returns>
        // GET: api/users
        public IQueryable<user> Getuser()
        {
            return db.user;
        }

        /// <summary>
        /// Metodoq ue permite obtner un usuario por su nombre de usuario
        /// </summary>
        /// <param name="username">string que representa el nombre de usuario</param>
        /// <returns>una estado 200 con la informacion del usuario correspondiente al nombre de usario o un 400 en caso contrario</returns>
        // GET: api/users/ejemplo@abd.com
        [ResponseType(typeof(user))]
        public IHttpActionResult Getuser(string username)
        {
            user user = db.user.Where(x => x.user1.ToLower().Equals(username.ToLower())).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Metodo que permite obtener un usuario por sus credenciales, nombre de usuario y password
        /// </summary>
        /// <param name="username">string que representa el nombre de usuario</param>
        /// <param name="pass">string que repersenta el password</param>
        /// <returns>un estado 200 con la informacion del usuario encontrado, o un 400 en caso contrario</returns>
        // GET: api/users/ejemplo@abc.com/123456
        [ResponseType(typeof(user))]
        [Route("api/users/{username}/{pass}")]
        [HttpGet]
        public IHttpActionResult GetuserbyCredentials(string username, string pass)
        {
            user user = db.user.Where(x => x.user1.ToLower().Equals(username.ToLower()) && x.password.Equals(pass)).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Metodo que permite modificar un usuario en la bd
        /// </summary>
        /// <param name="username">string que representa al nombre de usuario</param>
        /// <param name="user">obeto que contien toda la informacion del usuario a modificar</param>
        /// <returns>un estado 204</returns>
        // PUT: api/users/ejemplo@abd.com
        [ResponseType(typeof(void))]
        [Route("api/users/{username}")]
        public IHttpActionResult Putuser(string username, [FromBody]user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!user.user1.ToLower().ToString().Equals(username.ToLower().ToString()))
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(username))
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
        /// metodo que permite agrear un usuario al BD
        /// </summary>
        /// <param name="user">objeto que contiene la informacion del usuario para agregar</param>
        /// <returns>un estado 201 con contendio el cual es la inforamcion del usario agregado</returns>
        // POST: api/users
        [ResponseType(typeof(user))]
        public IHttpActionResult Postuser(user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!userExists(user.user1))
            {
                db.user.Add(user);
                db.SaveChanges();
            }
            else
            {
                return BadRequest();
            }          

            return CreatedAtRoute("DefaultApi", new { username = user.user1 }, user);
        }

        /// <summary>
        /// Metodo que permite eliminar un usuario
        /// </summary>
        /// <param name="username">string que representa el nombre de usuario a eliminar</param>
        /// <returns>un estado 200 con la informacion del usuario eliminado</returns>
        // DELETE: api/users/ejemplo@gmail.com
        [ResponseType(typeof(user))]
        [Route("api/users/delete/{username}")]
        [HttpDelete]
        public IHttpActionResult Deleteuser(string username)
        {
            //user user = db.user.Where(x => x.user1.ToString().ToLower().Equals(username.ToString().ToLower())).FirstOrDefault();
            user user = db.user.Where(x => x.user1.ToLower().Equals(username.ToLower())).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            db.user.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool userExists(string username)
        {
            return db.user.Count(e => e.user1.ToLower().Equals(username.ToLower())) > 0;
        }
    }
}