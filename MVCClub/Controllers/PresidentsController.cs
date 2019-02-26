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
    public class PresidentsController : Controller
    {
        private CLUBDBEntities db = new CLUBDBEntities();

        // GET: Presidents
        public ActionResult Index()
        {
            var presidents = db.Presidents.Include(p => p.Club);

            var clublist = new List<string>();

            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlConnection conn = new SqlConnection(strcon);
                conn.Open();
                String sql = "Select PresidentID,ClubName from PresidentClub";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string data = dr.GetInt32(0).ToString () + "-" + dr.GetString(1);
                    clublist.Add(data);
                }
                conn.Close();
            }
            catch (Exception ex)
            {

            }

            ViewBag.clist = clublist;
            return View(presidents.ToList());
        }

        // GET: Presidents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            President president = db.Presidents.Find(id);
            if (president == null)
            {
                return HttpNotFound();
            }

            int clubID = 0;
            

            
            var lstclub = new List<MemberDetails>();
            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlConnection conn = new SqlConnection(strcon);
                conn.Open();
                String sql = "Select ClubName,Location from PresidentClub where PresidentID=" + id;
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MemberDetails oMemberDetails = new MemberDetails();
                    oMemberDetails.club = dr.GetString(0);
                    oMemberDetails.OpenDate = getClubdate(oMemberDetails.club); 
                    oMemberDetails.Location = dr.GetString(1);
                    oMemberDetails.NoofMember = 1;
                    lstclub.Add(oMemberDetails);
                }

                conn.Close();
            }
            catch (Exception ex)
            {

            }


            ViewBag.ClubDetail = lstclub;

            return View(president);
        }


        string getClubdate(string cname)
        {
            string opdate = "";
            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlConnection conn = new SqlConnection(strcon);
                conn.Open();
                String sql = "Select OpenDate from Club where ClubName='" + cname.Trim () + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                dr.Read();

                opdate = dr.GetDateTime(0).ToString ();

                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return opdate;
        }
        // GET: Presidents/Create
        public ActionResult Create()
        {
            ViewBag.PresidentID = new SelectList(db.Clubs, "ClubID", "ClubName");
            return View();
        }

        // POST: Presidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PresidentID,LastName,FirstName,Phone")] President president)
        {
            if (ModelState.IsValid)
            {
                db.Presidents.Add(president);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PresidentID = new SelectList(db.Clubs, "ClubID", "ClubName", president.PresidentID);
            return View(president);
        }

        // GET: Presidents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            President president = db.Presidents.Find(id);
            if (president == null)
            {
                return HttpNotFound();
            }
            ViewBag.PresidentID = new SelectList(db.Clubs, "ClubID", "ClubName", president.PresidentID);
            return View(president);
        }

        [HttpPost]
       
        public ActionResult GetAssignLoc(int? Pid, string sid,string SportsList)
        {
            

            ViewBag.Prdid = sid;
            ViewBag.ClubNames = SportsList;
            return View();
        }


        [HttpPost]
        public ActionResult AssigPresident(int? id,string Prdid, string SportsList,string LocID)
        {
            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlConnection conn = new SqlConnection(strcon);
                conn.Open();
                String sql = "insert into PresidentClub values(" + Prdid + ",'" + SportsList + "','" + LocID + "')";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

            }

            return View();
        }


        // GET: Clubs/Edit/5
        public ActionResult AssignClub(int? id)
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

            var clublist = new List<string>();

            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlConnection conn = new SqlConnection(strcon);
                conn.Open();
                String sql = "Select ClubName from Club";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr=  cmd.ExecuteReader();

                while (dr.Read ())
                {
                    clublist.Add(dr.GetString(0));
                }
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            string PresedentName = "";
            try
            {
                string strcon = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlConnection conn = new SqlConnection(strcon);
                conn.Open();
                String sql = "Select FirstName,LastName from President where PresidentID=" + id;
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                 dr.Read();
                 PresedentName=dr.GetString(0) + " " + dr.GetString(1);
                
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            //ViewBag.MembershipID = new SelectList(db.Clubs, "ClubID", "ClubName", memberShip.MembershipID);
            //ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", memberShip.StudentID);

            ViewBag.clist = clublist;
           
            ViewBag.Pname = PresedentName;

            ViewBag.Pid = id;
            return View( );
        }
        // POST: Presidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PresidentID,LastName,FirstName,Phone")] President president)
        {
            if (ModelState.IsValid)
            {
                db.Entry(president).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PresidentID = new SelectList(db.Clubs, "ClubID", "ClubName", president.PresidentID);
            return View(president);
        }

        // GET: Presidents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            President president = db.Presidents.Find(id);
            if (president == null)
            {
                return HttpNotFound();
            }
            return View(president);
        }

        // POST: Presidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            President president = db.Presidents.Find(id);
            db.Presidents.Remove(president);
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
