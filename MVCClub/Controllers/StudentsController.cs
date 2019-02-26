using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCClub;
using System.Configuration;
using System.Data.SqlClient;
using MVCClub.Models;

namespace MVCClub.Controllers
{
    public class StudentsController : Controller
    {
        private CLUBDBEntities db = new CLUBDBEntities();

        // GET: Students
        public ActionResult Index()
        {

            
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            int clubID = 0;

            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlConnection conn = new SqlConnection(strcon);
                conn.Open();
                String sql = "Select ClubID from MemberShip where StudentID=" + id;
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                dr.Read();

                clubID = dr.GetInt32(0);
               
                conn.Close();
            }
            catch (Exception ex)
            {

            }

            MemberDetails oMemberDetails = new MemberDetails();

            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlConnection conn = new SqlConnection(strcon);
                conn.Open();
                String sql = "Select Club.ClubName,Club.OpenDate,Center.Location from Club,Center where Club.ClubID = Center.ClubID  and Club.ClubID=" + clubID;
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                dr.Read();

                oMemberDetails.club = dr.GetString(0);
                oMemberDetails.OpenDate = dr.GetDateTime(1).ToString ();
                oMemberDetails.Location = dr.GetString(2);

                conn.Close();
            }
            catch (Exception ex)
            {

            }


            ViewBag.ClubDetail = oMemberDetails;
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,LastName,FirstName,Phone,Email,RegistrationDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,LastName,FirstName,Phone,Email,RegistrationDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
