using Asp.NETMVCCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Asp.NETMVCCRUD.Controllers
{
    public class ProveedoresController : Controller
    {
        DBModel db = new DBModel();

        // GET: Proveedores
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
                List<Supplier> empList = db.Suppliers.ToList<Supplier>();
                return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            db.Configuration.LazyLoadingEnabled = false;
            if (id == 0)

                return View(new Supplier());
            else
            {
                using (DBModel db = new DBModel())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    return View(db.Suppliers.Where(x => x.SupplierID == id).FirstOrDefault<Supplier>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Supplier sup)
        {

            using (DBModel db = new DBModel())
            {
                db.Configuration.LazyLoadingEnabled = false;
                if (sup.SupplierID== 0)
                {
                    sup.FecIns = DateTime.Now;
                    sup.Active = true;
                    db.Suppliers.Add(sup);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    sup.FecUpd= DateTime.Now;
                    
                    db.Entry(sup).State = EntityState.Modified;
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
                Supplier sup = db.Suppliers.Where(x => x.SupplierID == id).FirstOrDefault<Supplier>();
                db.Suppliers.Remove(sup);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}