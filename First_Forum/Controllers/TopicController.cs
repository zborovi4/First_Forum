using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using First_Forum.Models;
using System.Web.Security;

namespace First_Forum.Controllers
{
    [HandleError]
    public class TopicController : Controller
    {
        private ForumContext db = new ForumContext();

        // GET: Topic
        public ActionResult Index(int? id)
        {
            try
            {
                List<Post> forum_posts;
                using (ForumContext fc = new ForumContext())
                {
                    forum_posts = fc.Forum_post.Select(p => new Post
                    {
                        Id = p.Id,
                        Author = p.Author,
                        Topic = p.Topic,
                        Body = p.Body,
                        Date = p.Date,
                        Post_id = p.Post_id,
                        Topic_id = p.Topic_id,
                        Forum_id = p.Forum_id
                    }).Where(p => p.Topic_id == id).ToList();

                    int f = forum_posts.FirstOrDefault().Forum_id;
                    TempData["forum"] = f;
                    TempData["topic"] = id;
                }
                ViewBag.Name = User.Identity.Name;
                ViewBag.AdminRole = User.IsInRole("admin") ? true : false;
                return View(forum_posts);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            return HttpNotFound();

        }

        // GET: Topic/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Topic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Body")] Forum_post forum_post)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (ForumContext fc = new ForumContext())
                    {
                       
                            forum_post.Author = User.Identity.Name;
                            forum_post.Date = DateTime.Now;
                            forum_post.Forum_id = (int)TempData["forum"];
                            forum_post.Topic_id = (int)TempData["topic"];
                            forum_post.Post_id = fc.Forum_post.Count(p => p.Topic_id == forum_post.Topic_id) + 1;

                            fc.Forum_post.Add(forum_post);
                            fc.SaveChanges();
                    }
                    int id = forum_post.Topic_id;
                    return RedirectToAction("Index", new { id = id });

                }


                catch (Exception ex)
                {
                    return View(ex.Message);
                }
            }
            ModelState.AddModelError("", "Error");
            return View(forum_post);
            
        }

        // GET: Topic/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum_post forum_post = await db.Forum_post.FindAsync(id);
            if (forum_post == null)
            {
                return HttpNotFound();
            }
            return View(forum_post);
        }

        // POST: Topic/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Author,Topic,Body,Date,Post_id,Topic_id,Forum_id")] Forum_post forum_post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forum_post).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = forum_post.Topic_id });
            }
            return null;
        }

        // GET: Topic/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum_post forum_post = await db.Forum_post.FindAsync(id);
            if (forum_post == null)
            {
                return HttpNotFound();
            }
            return View(forum_post);
        }

        // POST: Topic/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Forum_post forum_post = await db.Forum_post.FindAsync(id);
            if(forum_post.Post_id > 1)
            {
                db.Forum_post.Remove(forum_post);
            }
            else
            {
                var posts = await db.Forum_post.Where(p => p.Topic_id == forum_post.Topic_id).ToListAsync();
                db.Forum_post.RemoveRange(posts);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Forum", new { id = forum_post.Forum_id });
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction("Index", new { id = forum_post.Topic_id });
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
