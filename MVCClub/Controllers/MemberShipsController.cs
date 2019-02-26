using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCClub;

namespace MVCClub.Controllers
{
    public class MemberShipsController : Controller
    {
        private CLUBDBEntities db = new CLUBDBEntities();

        // GET: MemberShips
        public ActionResult Index()
        {
            var memberShips = db.MemberShips.Include(m => m.Club).Include(m => m.Student);
            return View(memberShips.ToList());
        }

        // GET: MemberShips/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberShip memberShip = db.MemberShips.Find(id);
            if (memberShip == null)
            {
                return HttpNotFound();
            }
            return View(memberShip);
        }

        // GET: MemberShips/Create
        public ActionResult Create()
        {
            ViewBag.MembershipID = new SelectList(db.Clubs, "ClubID", "ClubName");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName");
            return View();
        }

        // POST: MemberShips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MembershipID,ClubID,StudentID,ExpireDate,RegistrationFee")] MemberShip memberShip)
        {
            if (ModelState.IsValid)
            {
                db.MemberShips.Add(memberShip);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MembershipID = new SelectList(db.Clubs, "ClubID", "ClubName", memberShip.MembershipID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", memberShip.StudentID);
            return View(memberShip);
        }

        // GET: MemberShips/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberShip memberShip = db.MemberShips.Find(id);
            if (memberShip == null)
            {
                return HttpNotFound();
            }
            ViewBag.MembershipID = new SelectList(db.Clubs, "ClubID", "ClubName", memberShip.MembershipID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", memberShip.StudentID);
            return View(memberShip);
        }

        // POST: MemberShips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MembershipID,ClubID,StudentID,ExpireDate,RegistrationFee")] MemberShip memberShip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberShip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MembershipID = new SelectList(db.Clubs, "ClubID", "ClubName", memberShip.MembershipID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", memberShip.StudentID);
            return View(memberShip);
        }

        // GET: MemberShips/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberShip memberShip = db.MemberShips.Find(id);
            if (memberShip == null)
            {
                return HttpNotFound();
            }
            return View(memberShip);
        }

        // POST: MemberShips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MemberShip memberShip = db.MemberShips.Find(id);
            db.MemberShips.Remove(memberShip);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
