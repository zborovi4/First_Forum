﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

        public async Task<ActionResult> EditForum(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                using (ForumContext db = new ForumContext())
                {
                    Forum forum = await db.Forum.FindAsync(id);
                    if (forum == null)
                    {
                        return HttpNotFound();
                    }
                    return View(forum);
                }

            }
            catch (Exception)
            {

                throw;
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditForum([Bind(Include = "Forum_id,Name")] Forum forum)
        {
            try
            {
                using (ForumContext db = new ForumContext())
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(forum).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return View(forum);
        }

        public async Task<ActionResult> DeleteForum(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                using (ForumContext db = new ForumContext())
                {
                    Forum forum = await db.Forum.FindAsync(id);
                    if (forum == null)
                    {
                        return HttpNotFound();
                    }
                    return View(forum);
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        // POST: Forum/Delete/5
        [HttpPost, ActionName("DeleteForum")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using (ForumContext db = new ForumContext())
                {
                    Forum forum = await db.Forum.FindAsync(id);
                    db.Forum.Remove(forum);
                    await db.SaveChangesAsync();
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