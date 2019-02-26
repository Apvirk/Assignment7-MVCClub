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
using MVCClub;

namespace MVCClub.Controllers
{
    public class MemberShips1Controller : ApiController
    {
        private CLUBDBEntities db = new CLUBDBEntities();

        // GET: api/MemberShips1
        public IQueryable<MemberShip> GetMemberShips()
        {
            return db.MemberShips;
        }

        // GET: api/MemberShips1/5
        [ResponseType(typeof(MemberShip))]
        public IHttpActionResult GetMemberShip(int id)
        {
            MemberShip memberShip = db.MemberShips.Find(id);
            if (memberShip == null)
            {
                return NotFound();
            }

            return Ok(memberShip);
        }

        // PUT: api/MemberShips1/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMemberShip(int id, MemberShip memberShip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != memberShip.MembershipID)
            {
                return BadRequest();
            }

            db.Entry(memberShip).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberShipExists(id))
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

        // POST: api/MemberShips1
        [ResponseType(typeof(MemberShip))]
        public IHttpActionResult PostMemberShip(MemberShip memberShip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MemberShips.Add(memberShip);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MemberShipExists(memberShip.MembershipID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = memberShip.MembershipID }, memberShip);
        }

        // DELETE: api/MemberShips1/5
        [ResponseType(typeof(MemberShip))]
        public IHttpActionResult DeleteMemberShip(int id)
        {
            MemberShip memberShip = db.MemberShips.Find(id);
            if (memberShip == null)
            {
                return NotFound();
            }

            db.MemberShips.Remove(memberShip);
            db.SaveChanges();

            return Ok(memberShip);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberShipExists(int id)
        {
            return db.MemberShips.Count(e => e.MembershipID == id) > 0;
        }
    }
}