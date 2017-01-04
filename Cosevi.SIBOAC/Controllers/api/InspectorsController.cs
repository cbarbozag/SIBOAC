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
    public class InspectorsController : ApiController
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: api/Inspectors
        public IQueryable<Inspector> GetINSPECTOR()
        {
            return db.INSPECTOR.Where(a=>a.Id!=null && a.Id.Trim().Length>0);
        }

        // GET: api/Inspectors/5
        [ResponseType(typeof(Inspector))]
        public IHttpActionResult GetInspector(string id)
        {
            Inspector inspector = db.INSPECTOR.Find(id);
            if (inspector == null)
            {
                return NotFound();
            }

            return Ok(inspector);
        }

        // PUT: api/Inspectors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInspector(string id, Inspector inspector)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inspector.Id)
            {
                return BadRequest();
            }

            db.Entry(inspector).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InspectorExists(id))
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

        // POST: api/Inspectors
        [ResponseType(typeof(Inspector))]
        public IHttpActionResult PostInspector(Inspector inspector)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.INSPECTOR.Add(inspector);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (InspectorExists(inspector.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = inspector.Id }, inspector);
        }

        // DELETE: api/Inspectors/5
        [ResponseType(typeof(Inspector))]
        public IHttpActionResult DeleteInspector(string id)
        {
            Inspector inspector = db.INSPECTOR.Find(id);
            if (inspector == null)
            {
                return NotFound();
            }

            db.INSPECTOR.Remove(inspector);
            db.SaveChanges();

            return Ok(inspector);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InspectorExists(string id)
        {
            return db.INSPECTOR.Count(e => e.Id == id) > 0;
        }
    }
}