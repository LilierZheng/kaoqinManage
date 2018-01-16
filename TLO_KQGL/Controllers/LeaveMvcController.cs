using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TLO_KQGL.Models;
using TLO_KQGL.DBAccessLayer;

namespace TLO_KQGL.Controllers
{
    public class LeaveMvcController : Controller
    {
        private TLO_KQGLDAL db = new TLO_KQGLDAL();

        // GET: /LeaveMvc/
        public ActionResult Index()
        {
            return View(db.leave.ToList());
        }

        // GET: /LeaveMvc/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.leave.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            return View(leave);
        }

        // GET: /LeaveMvc/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /LeaveMvc/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Title,Content,LeaveBeginDate,LeaveEndDate,Hours,IsCheck,IsPass,CreateUser,CreateDate,LastUpdateUser,LastUpdateDate")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                leave.ID = Guid.NewGuid();
                db.leave.Add(leave);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(leave);
        }

        // GET: /LeaveMvc/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.leave.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            return View(leave);
        }

        // POST: /LeaveMvc/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Title,Content,LeaveBeginDate,LeaveEndDate,Hours,IsCheck,IsPass,CreateUser,CreateDate,LastUpdateUser,LastUpdateDate")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leave);
        }

        // GET: /LeaveMvc/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.leave.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            return View(leave);
        }

        // POST: /LeaveMvc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Leave leave = db.leave.Find(id);
            db.leave.Remove(leave);
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
