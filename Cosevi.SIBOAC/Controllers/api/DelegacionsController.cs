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
    public class DelegacionsController : ApiController
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: api/Delegacions
        public IQueryable<Delegacion> GetDELEGACION()
        {
            return db.DELEGACION;
        }

        // GET: api/Delegacions/5
        [ResponseType(typeof(Delegacion))]
        public IHttpActionResult GetDelegacion(string id)
        {
            Delegacion delegacion = db.DELEGACION.Find(id);
            if (delegacion == null)
            {
                return NotFound();
            }

            return Ok(delegacion);
        }

        // PUT: api/Delegacions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDelegacion(string id, Delegacion delegacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != delegacion.Id)
            {
                return BadRequest();
            }

            db.Entry(delegacion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DelegacionExists(id))
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

        // POST: api/Delegacions
        [ResponseType(typeof(Delegacion))]
        public IHttpActionResult PostDelegacion(Delegacion delegacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DELEGACION.Add(delegacion);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DelegacionExists(delegacion.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = delegacion.Id }, delegacion);
        }

        // DELETE: api/Delegacions/5
        [ResponseType(typeof(Delegacion))]
        public IHttpActionResult DeleteDelegacion(string id)
        {
            Delegacion delegacion = db.DELEGACION.Find(id);
            if (delegacion == null)
            {
                return NotFound();
            }

            db.DELEGACION.Remove(delegacion);
            db.SaveChanges();

            return Ok(delegacion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DelegacionExists(string id)
        {
            return db.DELEGACION.Count(e => e.Id == id) > 0;
        }
    }
}