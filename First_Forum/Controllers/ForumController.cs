using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using First_Forum.Models;

namespace First_Forum.Controllers
{
    public class ForumController : Controller
    {

        private ForumContext db = new ForumContext();      

        // GET: Forum
        public async Task<ActionResult> Index(int? id)
        {
            List<TopicsInfo> topicsInfo;
            try
            {
                using (ForumContext db = new ForumContext())
                {
                    topicsInfo = db.Forum_post.Select(t => new TopicsInfo
                    {
                        Id = t.Id,
                        Topic_id = t.Topic_id,
                        IdForum = t.Forum_id,
                        Subject = t.Topic,
                        CreatedBy = t.Author,
                        Replies = db.Forum_post.Count(p => p.Topic_id == t.Topic_id)
                    }).Where(t => t.Subject.Length > 0 && t.IdForum == id).ToList();
                }
               
            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }
            TempData["value"] = id;
            ViewBag.Name = User.Identity.Name;
            ViewBag.AdminRole = User.IsInRole("admin") ? true : false;
            return View(topicsInfo);
        }

        // GET: Forum/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Forum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Topic,Body")] Forum_post forum_Post)
        {
            if (ModelState.IsValid)
            {
                int forum_id = (int)TempData["value"];
                var now = DateTime.Now;
                var zeroDate = DateTime.MinValue.AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second).AddMilliseconds(now.Millisecond);
                int uniqueId = (int)(zeroDate.Ticks / 10000);

                forum_Post.Author = User.Identity.Name;
                forum_Post.Date = DateTime.Now;
                forum_Post.Post_id = 1;
                forum_Post.Topic_id = uniqueId;
                forum_Post.Forum_id = forum_id;

                db.Forum_post.Add(forum_Post);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Forum", new { id = forum_id }); 
            }

            return View(forum_Post);
        }


        //GET: Forum/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum_post forum_Post = await db.Forum_post.FindAsync(id);
            if (forum_Post == null)
            {
                return HttpNotFound();
            }
            return View(forum_Post);
        }

        //POST: Forum/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Forum_post forum_Post = await db.Forum_post.FindAsync(id);
            db.Forum_post.Remove(forum_Post);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { id = forum_Post.Forum_id });
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
