using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Web.Models;

namespace Web.Controllers
{
    public class PeopleApiController : ApiController
    {
        private MainModel db = new MainModel();

        // GET: api/PeopleApi
        [HttpGet]
        public IQueryable<Person> GetPeople()
        {
            return db.People;
        }

        // GET: api/PeopleApi/5
        [HttpGet]
        [ResponseType(typeof(Person))]
        public IHttpActionResult GetPerson(int id)
        {
            Person person = db.People.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/PeopleApi/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult PutPerson(int id, Person person)
        {
            person.Id = id; // You can fully remove input parameter 'id'
            var entry = db.Entry(person);
            
            if (person.Base64Image == null)
            {
                entry.Property("Image").IsModified = false;
                entry.Property("ContentType").IsModified = false;
            }
            else
            {
                person.Image = Convert.FromBase64String(person.Base64Image.Aggregate("", (current, next) => current + next));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.Id)
            {
                return BadRequest();
            }

            entry.State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PeopleApi
        [HttpPost]
        [ResponseType(typeof(Person))]
        public IHttpActionResult PostPerson(Person person)
        {
            if (person.Base64Image != null)
                person.Image = Convert.FromBase64String(person.Base64Image);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.People.Add(person);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = person.Id }, person);
        }

        // DELETE: api/PeopleApi/5
        [ResponseType(typeof(Person))]
        public IHttpActionResult DeletePerson(int id)
        {
            Person person = db.People.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            db.People.Remove(person);
            db.SaveChanges();

            return Ok(person);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonExists(int id)
        {
            return db.People.Count(e => e.Id == id) > 0;
        }
    }
}