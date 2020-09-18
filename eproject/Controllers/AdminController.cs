using eproject.Models;
using eproject.Security;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace eproject.Controllers
{
    public class AdminController : Controller
    {
        private context db = new context();
      
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult index([Bind(Include = "email,pass")] User ouser)
        {
            if (ouser.email == null || ouser.pass == null)
            {
                return View(ouser);
            }
            var user = db.user.Where(m => m.email == ouser.email).SingleOrDefault();
            if (user != null && pass.VerifyHashedPassword(user.pass, ouser.pass))
            {
                Session["userId"] = user.id;
                Session["email"] = user.email;
                return RedirectToAction("user", "admin");
            }
            ModelState.AddModelError("", "email or password error");
            return View(ouser);
        }
        //custom authorize
        [AuthorizeUser(role = "ROLE_ADMIN")]
        public ActionResult user(string search)
        {
            if(search != null && search != "")
            {
                return View(db.user.Where(c => c.role == "ROLE_USER" && (c.email.ToLower().Contains(search.ToLower()) || c.name.ToLower().Contains(search.ToLower())) ));
            }
            return View(db.user.Where(c => c.role == "ROLE_USER"));
        }
        [AuthorizeUser(role = "ROLE_ADMIN")]
        public ActionResult edituser(Guid id,bool enabled)
        {
            var user = db.user.Find(id);
            user.enabled = enabled;
            db.SaveChanges();
            return RedirectToAction("user");
        }
        [AuthorizeUser(role = "ROLE_ADMIN")]
        public ActionResult recipe(string search)
        {
            if (search != null && search != "")
            {
                return View(db.recipe.Where(c => c.name.ToLower().Contains(search.ToLower()) || c.category.ToLower().Contains(search.ToLower())));
            }
            return View(db.recipe);
        }
        [AuthorizeUser(role = "ROLE_ADMIN")]
        public ActionResult editRecipe(Guid id, bool enabled)
        {
            var recipe = db.recipe.Find(id);
            recipe.enabled = enabled;
            db.SaveChanges();
            return RedirectToAction("recipe");
        }
        //Son
        [AuthorizeUser(role = "ROLE_ADMIN")]
        public ActionResult contestIndex(string sortOrder, string currentFilter, string search, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "name" : "";
            ViewBag.CreateDateSort = sortOrder == "datecreate" ? "createdate" : "datecreate";
            ViewBag.StartDateSort = sortOrder == "datestart" ? "stardate" : "datestart";
            ViewBag.EndDateSort = sortOrder == "dateend" ? "enddate" : "dateend";

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var contests = from c in db.contest
                           select c;

            if (!String.IsNullOrEmpty(search))
            {
                contests = contests.Where(c => c.name.Contains(search));
            }

            switch (sortOrder)
            {
                case "datecreate":
                    contests = contests.OrderBy(c => c.createAt);
                    break;
                case "name":
                    contests = contests.OrderBy(c => c.name);
                    break;
                case "":
                    contests = contests.OrderByDescending(c => c.name);
                    break;
                case "datestart":
                    contests = contests.OrderBy(c => c.startDate);
                    break;
                case "stardate":
                    contests = contests.OrderByDescending(c => c.startDate);
                    break;
                case "dateend":
                    contests = contests.OrderBy(c => c.endDate);
                    break;
                case "enddate":
                    contests = contests.OrderByDescending(c => c.endDate);
                    break;
                default:
                    contests = contests.OrderByDescending(c => c.createAt);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(contests.ToPagedList(pageNumber, pageSize));
        }

        // GET: Contest/Create
        [AuthorizeUser(role = "ROLE_ADMIN")]
        public ActionResult contestCreate()
        {
            return View();
        }

        // POST: Contest/Create
        [AuthorizeUser(role = "ROLE_ADMIN")]
        [HttpPost]
        public ActionResult contestCreate(Contest newcontest)
        {
            if (ModelState.IsValid)
            {
                var contest = db.contest.Where(c => c.name.Equals(newcontest.name));
                if (contest.Count() != 0)
                {
                    ViewBag.Msg = "Contest name already exist";
                }
                else
                {
                    if (newcontest.startDate < DateTime.Today)
                    {
                        ViewBag.Msg = "Start date cannot be less than current date";
                    }
                    else
                    {
                        if (newcontest.endDate <= newcontest.startDate)
                        {
                            ViewBag.Msg = "End date cannot be equal or less than start date";
                        }
                        else
                        {
                            db.contest.Add(newcontest);
                            newcontest.createAt = DateTime.Today;
                            db.SaveChanges();
                            return RedirectToAction("contestIndex","admin");
                        }
                    }
                }
            }
            return View(newcontest);
        }


        // GET: Contest/Details
        [AuthorizeUser(role = "ROLE_ADMIN")]
        public ActionResult contestDetails(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = db.contest.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }

        // GET: Contest/Edit
        [AuthorizeUser(role = "ROLE_ADMIN")]
        public ActionResult contestEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = db.contest.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }

        // POST: Contest/Edit
        [AuthorizeUser(role = "ROLE_ADMIN")]
        [HttpPost]
        public ActionResult contestEdit([Bind(Include = "id,name,content,createAt,startDate,endDate")] Contest editcontest)
        {
            if (ModelState.IsValid)
            {
                var contest = db.contest.Where(c => c.name == editcontest.name && c.id != editcontest.id);
                if (contest.Count() != 0)
                {
                    ViewBag.Msg = "Contest name was exist";
                }
                else
                {
                    if (editcontest.startDate < editcontest.createAt)
                    {
                        ViewBag.Msg = "Start date cannot be less than create date";
                    }
                    else
                    {
                        if (editcontest.endDate <= editcontest.startDate)
                        {
                            ViewBag.Msg = "End date cannot be equal or less than start date";
                        }
                        else
                        {
                            db.Entry(editcontest).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            return View(editcontest);
        }

        // GET: Contests/Delete
        [AuthorizeUser(role = "ROLE_ADMIN")]
        public ActionResult contestDelete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = db.contest.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }

        // POST: Contests/Delete
        [AuthorizeUser(role = "ROLE_ADMIN")]
        [HttpPost, ActionName("contestDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult contestDeleteConfirmed(Guid id)
        {
            Contest contest = db.contest.Find(id);
            db.contest.Remove(contest);
            db.SaveChanges();
            return RedirectToAction("contestIndex");
        }
    }
}