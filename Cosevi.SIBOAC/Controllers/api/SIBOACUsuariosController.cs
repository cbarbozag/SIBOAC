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
    public class SIBOACUsuariosController : ApiController
    {
        private SIBOACSecurityEntities db = new SIBOACSecurityEntities();

        // GET: api/SIBOACUsuarios
        public IQueryable<SIBOACUsuarios> GetSIBOACUsuarios()
        {
            return db.SIBOACUsuarios;
        }

        // GET: api/SIBOACUsuarios/5
        [ResponseType(typeof(SIBOACUsuarios))]
        public IHttpActionResult GetSIBOACUsuarios(int id)
        {
            SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
            if (sIBOACUsuarios == null)
            {
                return NotFound();
            }

            return Ok(sIBOACUsuarios);
        }

        // PUT: api/SIBOACUsuarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSIBOACUsuarios(int id, SIBOACUsuarios sIBOACUsuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sIBOACUsuarios.Id)
            {
                return BadRequest();
            }

            db.Entry(sIBOACUsuarios).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SIBOACUsuariosExists(id))
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

        // POST: api/SIBOACUsuarios
        [ResponseType(typeof(SIBOACUsuarios))]
        public IHttpActionResult PostSIBOACUsuarios(SIBOACUsuarios sIBOACUsuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SIBOACUsuarios.Add(sIBOACUsuarios);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SIBOACUsuariosExists(sIBOACUsuarios.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = sIBOACUsuarios.Id }, sIBOACUsuarios);
        }

        // DELETE: api/SIBOACUsuarios/5
        [ResponseType(typeof(SIBOACUsuarios))]
        public IHttpActionResult DeleteSIBOACUsuarios(int id)
        {
            SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
            if (sIBOACUsuarios == null)
            {
                return NotFound();
            }

            db.SIBOACUsuarios.Remove(sIBOACUsuarios);
            db.SaveChanges();

            return Ok(sIBOACUsuarios);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SIBOACUsuariosExists(int id)
        {
            return db.SIBOACUsuarios.Count(e => e.Id == id) > 0;
        }
    }
}