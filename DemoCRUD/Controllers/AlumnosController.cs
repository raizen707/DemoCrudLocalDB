using DemoCRUD.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace DemoCRUD.Controllers
{
    public class AlumnosController : Controller
    {
        private UniversidadEntities db = new UniversidadEntities();

        // GET: Alumnos
        public ActionResult Index()
        {
            //return View(db.Alumnoes.ToList());
            IEnumerable<Alumno> Alumno = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:60689/api/");
                var responseTask = client.GetAsync("AlumnosApi");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTast = result.Content.ReadAsAsync<IList<Alumno>>();
                    readTast.Wait();
                    Alumno = readTast.Result;
                }
                else {
                    Alumno = Enumerable.Empty<Alumno>();
                    ModelState.AddModelError(string.Empty, "No hay datos");
                }
            };
            return View(Alumno.ToList());

        }

        // GET: Alumnos/Details/5
        public ActionResult Details(int? id)
        {
            //return View(db.Alumnoes.ToList());
            Alumno Alumno = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:60689/api/");
                var responseTask = client.GetAsync("AlumnosApi/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTast = result.Content.ReadAsAsync<Alumno>();
                    readTast.Wait();
                    Alumno = readTast.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No hay datos");
                }
            };
            return View(Alumno);
        }

        // GET: Alumnos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Alumnos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,nombres,apellidos,telefono,email,NIT")] Alumno alumno)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Alumnoes.Add(alumno);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //return View(alumno);
            //return View(db.Alumnoes.ToList());
             using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:60689/api/");
                var responseTask = client.PostAsJsonAsync("AlumnosApi", alumno);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No hay datos");
                }
            };
            return View(alumno);
        }

        // GET: Alumnos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumnoes.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        // POST: Alumnos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,nombres,apellidos,telefono,email,NIT")] Alumno alumno)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("http://localhost:60689/api/");
                var responseTask = client.PutAsJsonAsync("AlumnosApi", alumno);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No hay datos");
                }
            };
            return View(alumno);
        }

        // GET: Alumnos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumnoes.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        // POST: Alumnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Alumno alumno = db.Alumnoes.Find(id);
            db.Alumnoes.Remove(alumno);
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
