using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult> Index(int id)
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
            catch (Exception)
            {

                throw;
            }
            return View(topicsInfo);
        }

        // GET: Forum/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TopicsInfo topicsInfo = await db.TopicsInfoes.FindAsync(id);
        //    if (topicsInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(topicsInfo);
        //}

        // GET: Forum/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Forum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Topic,Body")] Forum_post forum_Post)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now;
                var zeroDate = DateTime.MinValue.AddHours(now.Hour).AddMinutes(now.Minute).AddSeconds(now.Second).AddMilliseconds(now.Millisecond);
                int uniqueId = (int)(zeroDate.Ticks / 10000);

                forum_Post.Author = "Vasilisa";
                forum_Post.Date = DateTime.Now;
                forum_Post.Post_id = 1;
                forum_Post.Topic_id = uniqueId;
                forum_Post.Forum_id = 3;
                db.Forum_post.Add(forum_Post);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", 3);
            }

            return View(forum_Post);
        }


        // GET: Forum/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TopicsInfo topicsInfo = await db.TopicsInfoes.FindAsync(id);
        //    if (topicsInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(topicsInfo);
        //}

        // POST: Forum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    TopicsInfo topicsInfo = await db.TopicsInfoes.FindAsync(id);
        //    //db.TopicsInfoes.Remove(topicsInfo);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
