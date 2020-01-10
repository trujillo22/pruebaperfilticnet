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
    public class usersController : ApiController
    {
        private perfilticEntities db = new perfilticEntities();

        // GET: api/users
        public IQueryable<user> Getuser()
        {
            return db.user;
        }

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

        // GET: api/users/ejemplo@abc.com/123456
        [ResponseType(typeof(user))]
        public IHttpActionResult GetuserbyCredentials(string username, string pass)
        {
            user user = db.user.Where(x => x.user1.ToLower().Equals(username.ToLower()) && x.password.Equals(pass)).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putuser(string username, user user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!user.user1.ToLower().Equals(username.ToLower()))
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

        // DELETE: api/users/ejemplo@gmail.com
        [ResponseType(typeof(user))]
        public IHttpActionResult Deleteuser(string username)
        {
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