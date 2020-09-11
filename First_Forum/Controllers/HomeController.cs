using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using First_Forum.Models;

namespace First_Forum.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<ForumsInfo> forumsInfo;
            try
            {
                using (ForumContext db = new ForumContext())
                {

                    forumsInfo = db.Forum.Select(f => new ForumsInfo
                    {
                        Id = f.Forum_id,
                        Board = f.Name,
                        Threads = db.Forum_post.Count(t => t.Forum_id == f.Forum_id && t.Topic.Length > 0),
                        Posts = db.Forum_post.Count(t => t.Forum_id == f.Forum_id)
                    }).ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(forumsInfo);
        }
        public ActionResult AddForum()
        {
            return PartialView("AddForum");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddForum(Forum forum)
        {
            try
            {
                using (ForumContext db = new ForumContext())
                {
                    db.Entry(forum).State = EntityState.Added;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}