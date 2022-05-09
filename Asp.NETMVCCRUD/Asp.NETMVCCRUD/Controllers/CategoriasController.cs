using Asp.NETMVCCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Asp.NETMVCCRUD.Controllers
{
    public class CategoriasController : Controller
    {
        DBModel db = new DBModel();
        
        // GET: Categorias
        public ActionResult Index()
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            return View();
        }

        public ActionResult GetData()
        {
            using (DBModel db = new DBModel())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<Category> empList = db.Categories.ToList<Category>();
                return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            db.Configuration.LazyLoadingEnabled = false;
            if (id == 0)
                return View(new Category());
            else
            {
                using (DBModel db = new DBModel())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return View(db.Categories.Where(x => x.CategoryID == id).FirstOrDefault<Category>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Category cat)
        {
            using (DBModel db = new DBModel())
            {
                db.Configuration.LazyLoadingEnabled = false;
                if (cat.CategoryID == 0)
                {
                    cat.FecIns = DateTime.Now;
                    cat.Active = true;
                    db.Categories.Add(cat);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    cat.FecUpd = DateTime.Now;
                    db.Entry(cat).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            
            using (DBModel db = new DBModel())
            {
                db.Configuration.LazyLoadingEnabled = false;//ver despues de terminar si es q lo hago ctm
                Category cat= db.Categories.Where(x => x.CategoryID== id).FirstOrDefault<Category>();
                cat.Active = false;
                db.Categories.Remove(cat);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}