using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCClub;
using System.Data.SqlClient;
using System.Configuration;
using MVCClub.Models;

namespace MVCClub.Controllers
{
    public class ClubsController : Controller
    {
        private CLUBDBEntities db = new CLUBDBEntities();

        // GET: Clubs
        public ActionResult Index()
        {
            var clubs = db.Clubs.Include(c => c.MemberShip).Include(c => c.President).Include(c => c.Center);
            return View(clubs.ToList());
        }

        // GET: Clubs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            int StudentID = 0;
            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlConnection conn = new SqlConnection(strcon);
                conn.Open();
                String sql = "Select StudentID from MemberShip where MembershipID=" + club.MemberShip .MembershipID ;
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                dr.Read();

                StudentID = dr.GetInt32(0);

                conn.Close();
            }
            catch (Exception ex)
            {

            }

            StudentDetails oStudentDetails = new StudentDetails();

            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlConnection conn = new SqlConnection(strcon);
                conn.Open();
                String sql = "Select [LastName] ,[FirstName],[Phone],[Email],[RegistrationDate] from Student where StudentID=" + StudentID;
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                dr.Read();

                oStudentDetails.LastName = dr.GetString(0);
                oStudentDetails.FirstName  = dr.GetString(1);
                oStudentDetails.Phone = dr.GetString(2);
                oStudentDetails.Email  = dr.GetString(3);
                oStudentDetails.RegistrationDate  = dr.GetDateTime(4).ToString ();

                conn.Close();
            }
            catch (Exception ex)
            {

            }


           
            var lstPresident = new List<PresidentDetails>();
            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlConnection conn = new SqlConnection(strcon);
                conn.Open();
                String sql = "Select FirstName,LastName from President where PresidentID=" + id;
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PresidentDetails oPresidentDetails = new Models.PresidentDetails();
                    oPresidentDetails.FirstName = dr.GetString(0);
                    oPresidentDetails.LastName  = dr.GetString(1);
                    lstPresident.Add(oPresidentDetails);
                }

                conn.Close();
            }
            catch (Exception ex)
            {

            }


            ViewBag.lstPresidentDetails = lstPresident;
            ViewBag.SDetails = oStudentDetails;


            return View(club);
        }

        // GET: Clubs/Create
        public ActionResult Create()
        {
            ViewBag.ClubID = new SelectList(db.MemberShips, "MembershipID", "MembershipID");
            ViewBag.ClubID = new SelectList(db.Presidents, "PresidentID", "LastName");
            ViewBag.ClubID = new SelectList(db.Centers, "ClubID", "Location");
            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClubID,ClubName,OpenDate")] Club club)
        {
            if (ModelState.IsValid)
            {
                db.Clubs.Add(club);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClubID = new SelectList(db.MemberShips, "MembershipID", "MembershipID", club.ClubID);
            ViewBag.ClubID = new SelectList(db.Presidents, "PresidentID", "LastName", club.ClubID);
            ViewBag.ClubID = new SelectList(db.Centers, "ClubID", "Location", club.ClubID);
            return View(club);
        }

        // GET: Clubs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClubID = new SelectList(db.MemberShips, "MembershipID", "MembershipID", club.ClubID);
            ViewBag.ClubID = new SelectList(db.Presidents, "PresidentID", "LastName", club.ClubID);
            ViewBag.ClubID = new SelectList(db.Centers, "ClubID", "Location", club.ClubID);
            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClubID,ClubName,OpenDate")] Club club)
        {
            if (ModelState.IsValid)
            {
                db.Entry(club).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClubID = new SelectList(db.MemberShips, "MembershipID", "MembershipID", club.ClubID);
            ViewBag.ClubID = new SelectList(db.Presidents, "PresidentID", "LastName", club.ClubID);
            ViewBag.ClubID = new SelectList(db.Centers, "ClubID", "Location", club.ClubID);
            return View(club);
        }

        // GET: Clubs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Club club = db.Clubs.Find(id);
            db.Clubs.Remove(club);
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
