using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class PeopleController : Controller
    {
        private MainModel db = new MainModel();

        // GET: People
        public ActionResult Index()
        {
            var output = db.People.ToList();
            if (ControllerContext.IsChildAction || Request.IsAjaxRequest())
                return PartialView(output);
            return View(output);
        }

        // GET: People/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Person person = db.People.Find(id);
        //    if (person == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(person);
        //}

        // GET: People/Create
        public ActionResult Create()
        {
            if (ControllerContext.IsChildAction || Request.IsAjaxRequest())
                return PartialView();
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] Person person, HttpPostedFileBase Image)
        {

            if (Image != null)
            {
                byte[] my_buffer = new byte[Image.ContentLength];
                Image.InputStream.BeginRead(my_buffer, 0, Image.ContentLength, null, null);
                person.Image = my_buffer;
                person.ContentType = Image.ContentType;
            }


            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (ControllerContext.IsChildAction || Request.IsAjaxRequest())
                return PartialView(person);
            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            if (ControllerContext.IsChildAction || Request.IsAjaxRequest())
                return PartialView(person);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Person person, HttpPostedFileBase Image)
        {

            var entry = db.Entry(person);
            entry.State = EntityState.Modified;
            // var entry = db.People.Attach(person);

            if (Image != null)
            {
                byte[] buffer = new byte[Image.ContentLength];
                Image.InputStream.Read(buffer, 0, Image.ContentLength);
                person.Image = buffer;
                person.ContentType = Image.ContentType;
            }
            else
            {
                entry.Property("ContentType").IsModified = false;
                entry.Property("Image").IsModified = false;
            }
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (ControllerContext.IsChildAction || Request.IsAjaxRequest())
                return PartialView(person);
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            if (ControllerContext.IsChildAction || Request.IsAjaxRequest())
                return PartialView(person);
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Download(int Id)
        {
            var person = db.People.Find(Id);
            if (person.Image == null)
                return HttpNotFound();
            var mem_stream = new MemoryStream(person.Image);
            return File(mem_stream, person.ContentType);
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
