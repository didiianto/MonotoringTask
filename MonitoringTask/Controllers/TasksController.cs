using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MonitoringTask.Models.DB;
using Microsoft.AspNet.Identity;

namespace MonitoringTask.Controllers
{
    public class TasksController : Controller
    {
        private Entities db = new Entities();


        // GET: Tasks
        public ActionResult Index(string filterName, string sortBy)
        {
            var list = db.Tasks.ToList();

            // Apply filtering based on filterName
            if (!string.IsNullOrWhiteSpace(filterName))
            {
                list = list.Where(t => t.Name.Contains(filterName)).ToList();
            }

            // Apply sorting based on sortBy
            switch (sortBy)
            {
                case "name":
                    list = list.OrderBy(t => t.Name).ToList();
                    break;
                case "duedate":
                    list = list.OrderBy(t => t.DueDate).ToList();
                    break;
                case "priority":
                    list = list.OrderBy(t => t.Priority).ToList();
                    break;
                case "pic":
                    list = list.OrderBy(t => t.PIC).ToList();
                    break;
                case "status":
                    list = list.OrderBy(t => t.Status).ToList();
                    break;
                default:
                    list = list.OrderBy(t => t.DateCreated).ToList();
                    break;
            }

            var dataList = new Models.ViewModel.TaskViewModelObj();
            var lst = new List<Models.ViewModel.TaskViewModel>();
            foreach (var task in list)
            {
                //bind
                var model = new Models.ViewModel.TaskViewModel();
                model.Id = task.Id;
                model.DateCreated = task.DateCreated;
                model.DateModified = task.DateModified;
                model.DueDate = task.DueDate;
                string priority = "";
                if (task.Priority == 1)
                    priority = "High";
                else if (task.Priority == 2)
                    priority = "Medium";
                else
                    priority = "Low";

                model.Priority = priority;
                model.Name = task.Name;
                model.Description = task.Description;
                model.DateNotif = task.DateNotif;
                model.PIC = task.PIC;
                model.Status = task.Status;
                var userCreate = db.AspNetUsers.Where(x => x.Id == task.CreatedBy.ToString()).ToList();
                if(userCreate.Any())
                    model.CreatedBy = userCreate.FirstOrDefault().UserName;
                var userModif = db.AspNetUsers.Where(x => x.Id == task.ModifiedBy.ToString()).ToList();
                if (userModif.Any())
                    model.ModifiedBy = userModif.FirstOrDefault().UserName;

                lst.Add(model);
            }

            dataList.Tasks = lst;

            return View(dataList);
        }

        // GET: Tasks/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DueDate,Priority,Name,Description,PIC")] Task task)
        {
            if (ModelState.IsValid)
            {
                task.Id = Guid.NewGuid();
                task.DateCreated = DateTime.Now;
                string userId = User.Identity.GetUserId();

                task.CreatedBy = new Guid(userId);
                task.Status = "In Progress";
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(task);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DueDate,Priority,Name,Description,PIC,Status")] Task task)
        {
            if (ModelState.IsValid)
            {
                var existingTask = db.Tasks.Find(task.Id);

                if (existingTask != null)
                {
                    string userId = User.Identity.GetUserId();
                    existingTask.ModifiedBy = new Guid(userId);
                    existingTask.DueDate = task.DueDate;
                    existingTask.Priority = task.Priority;
                    existingTask.Name = task.Name;
                    existingTask.Description = task.Description;
                    existingTask.PIC = task.PIC;
                    existingTask.DateModified = DateTime.Now;
                    existingTask.Status = task.Status;
                    db.Entry(existingTask).State = EntityState.Modified;
                    db.SaveChanges();

                    //send email notification
                    if (task.Status == "Completed")
                    {
                        existingTask.DateNotif = DateTime.Now;
                        db.SaveChanges();
                        Models.Helper.SendNotif(existingTask);
                    }
                }
                    
                return RedirectToAction("Index");
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
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
