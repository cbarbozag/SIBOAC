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
using Cosevi.SIBOAC.Models;

namespace Cosevi.SIBOAC.Controllers.api
{
    public class AutoridadJudicialController : ApiController
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: api/SIBOACAutoridadJudicial
        public IQueryable<Autoridad> GetAUTORIDAD()
        {
            return db.AUTORIDAD;
        }

        // GET: api/SIBOACAutoridadJudicial/5
        [ResponseType(typeof(Autoridad))]
        public IHttpActionResult GetAutoridad(string id)
        {
            Autoridad autoridad = db.AUTORIDAD.Find(id);
            if (autoridad == null)
            {
                return NotFound();
            }

            return Ok(autoridad);
        }

        // PUT: api/SIBOACAutoridadJudicial/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAutoridad(string id, Autoridad autoridad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != autoridad.Id)
            {
                return BadRequest();
            }

            db.Entry(autoridad).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutoridadExists(id))
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

        // POST: api/SIBOACAutoridadJudicial
        [ResponseType(typeof(Autoridad))]
        public IHttpActionResult PostAutoridad(Autoridad autoridad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AUTORIDAD.Add(autoridad);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AutoridadExists(autoridad.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = autoridad.Id }, autoridad);
        }

        // DELETE: api/SIBOACAutoridadJudicial/5
        [ResponseType(typeof(Autoridad))]
        public IHttpActionResult DeleteAutoridad(string id)
        {
            Autoridad autoridad = db.AUTORIDAD.Find(id);
            if (autoridad == null)
            {
                return NotFound();
            }

            db.AUTORIDAD.Remove(autoridad);
            db.SaveChanges();

            return Ok(autoridad);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AutoridadExists(string id)
        {
            return db.AUTORIDAD.Count(e => e.Id == id) > 0;
        }
    }
}