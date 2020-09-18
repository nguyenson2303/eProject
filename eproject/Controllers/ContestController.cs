using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eproject.Models;
using PagedList;



namespace eproject.Controllers
{
    public class ContestController : Controller
    {
        private context db = new context();
        // GET: Contest/Sorting/Searching/Paging 
        public ActionResult Index(string sortOrder, string currentFilter, string search, int? page)
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

        // GET: Contest/Details
        public ActionResult Details(Guid? id)
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
    }
}