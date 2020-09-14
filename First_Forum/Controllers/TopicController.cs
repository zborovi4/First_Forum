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

namespace First_Forum.Controllers
{
    public class TopicController : Controller
    {
        private ForumContext db = new ForumContext();

        // GET: Topic
        public async Task<ActionResult> Index(int? id)
        {
            var forum_posts = db.Forum_post.Select(p => new Post
            { 
                Id = p.Id,
                Author = p.Author,
                Topic = p.Topic,
                Body = p.Body,
                Date = p.Date,
                Post_id = p.Post_id,
                Topic_id = p.Topic_id,
                Forum_id = p.Forum_id
            }).Where(p => p.Topic_id == id);

            TempData["forum"] = forum_posts.FirstOrDefault().Forum_id;
            TempData["topic"] = id;
            return View(await forum_posts.ToListAsync());
        }
        
        // GET: Topic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Topic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Body")] Forum_post forum_post)
        {
            if (ModelState.IsValid)
            {
                forum_post.Author = "Lesya";
                forum_post.Date = DateTime.Now;
                forum_post.Forum_id = (int)TempData["forum"];
                forum_post.Topic_id = (int)TempData["topic"];
                forum_post.Post_id = db.Forum_post.Count(p => p.Topic_id == forum_post.Topic_id) + 1;

                db.Forum_post.Add(forum_post);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(forum_post);
        }

        // GET: Topic/Edit/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Author,Topic,Body,Date,Post_id,Topic_id,Forum_id")] Forum_post forum_post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forum_post).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(forum_post);
        }

        // GET: Topic/Delete/5
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
            }
            
            await db.SaveChangesAsync();
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
