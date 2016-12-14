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
    public class BOLETAsController : ApiController
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: api/BOLETAs
        public IQueryable<BOLETA> GetBOLETA()
        {
            return db.BOLETA;
        }

        // GET: api/BOLETAs/5
        [ResponseType(typeof(BOLETA))]
        public IHttpActionResult GetBOLETA(string id)
        {
            BOLETA bOLETA = db.BOLETA.Find(id);
            if (bOLETA == null)
            {
                return NotFound();
            }

            return Ok(bOLETA);
        }

        // PUT: api/BOLETAs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBOLETA(string id, BOLETA bOLETA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bOLETA.fuente)
            {
                return BadRequest();
            }

            db.Entry(bOLETA).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BOLETAExists(id))
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

        // POST: api/BOLETAs
        [ResponseType(typeof(BOLETA))]
        public IHttpActionResult PostBOLETA(BOLETA bOLETA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BOLETA.Add(bOLETA);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BOLETAExists(bOLETA.fuente))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = bOLETA.fuente }, bOLETA);
        }

        // DELETE: api/BOLETAs/5
        [ResponseType(typeof(BOLETA))]
        public IHttpActionResult DeleteBOLETA(string id)
        {
            BOLETA bOLETA = db.BOLETA.Find(id);
            if (bOLETA == null)
            {
                return NotFound();
            }

            db.BOLETA.Remove(bOLETA);
            db.SaveChanges();

            return Ok(bOLETA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BOLETAExists(string id)
        {
            return db.BOLETA.Count(e => e.fuente == id) > 0;
        }
    }
}