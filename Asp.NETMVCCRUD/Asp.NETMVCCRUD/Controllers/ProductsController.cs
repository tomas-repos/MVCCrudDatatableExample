using Asp.NETMVCCRUD.Models;
using Asp.NETMVCCRUD.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asp.NETMVCCRUD.Controllers
{
    public class ProductsController : Controller
    {
        DBModel db = new DBModel();

        // GET: Products
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData()
        {
            using (DBModel db = new DBModel())
            {
                
                db.Configuration.LazyLoadingEnabled = false;
                /*if ()
                {
                    
                }*/
                List<Product> empList= new List<Product>();
                
                empList= db.Products.OrderBy(f => f.FecUpd).ToList();
                
                //foreach (var item in empList)
                //{
                //    var valor=item.Discontinued.ToString();
                //    if (valor=="0")
                //    {
                        
                        
                //    }
                    
                //}
                return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            
            LLenarSuppliers(id);
            LlenarCategorias(id);
            if (id == 0)
                
                return View(new Product());
            else
            {
                using (DBModel db = new DBModel())
                {

                    db.Configuration.LazyLoadingEnabled = false;
                    return View(db.Products.Where(x => x.ProductID== id).FirstOrDefault<Product>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(Product prod)
        {

            using (DBModel db = new DBModel())
            {
                
                var wea = prod.CategoryID;
                var wea2 = prod.SupplierID;
                
                //db.Configuration.LazyLoadingEnabled = false;
                if (prod.ProductID== 0)
                {
                    
                    prod.FecIns = DateTime.Now;
                    prod.Active = true;
                    db.Products.Add(prod);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Guardado Correctamente" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    prod.FecUpd = DateTime.Now;
                    db.Entry(prod).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Actualizado Correctamente" }, JsonRequestBehavior.AllowGet);
                }
            }


        }

        [HttpPost]
        public ActionResult Delete(int id)
        {

            using (DBModel db = new DBModel())
            {
                db.Configuration.LazyLoadingEnabled = false;//ver despues de terminar si es q lo hago ctm
                Product prod= db.Products.Where(x => x.ProductID== id).FirstOrDefault<Product>();
                prod.Active = false;
                prod.FecUpd = DateTime.Now;
                db.Products.Remove(prod);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LLenarSuppliers(int id=0)
        {
            try
            {
                //llega en 0 a insertar
                if (id==0)
                {
                    using (DBModel db = new DBModel())
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        List<SupplierVM> lstSup = null;
                        lstSup = (from l in db.Suppliers
                                  select new SupplierVM
                                  {
                                      Id = l.SupplierID,
                                      Nombre    = l.CompanyName

                                  }).ToList();
                        List<SelectListItem> items = lstSup.ConvertAll(d =>
                        {
                            return new SelectListItem()
                            {
                                Text = d.Nombre.ToString(),
                                Value = d.Id.ToString(),
                                Selected = false
                            };

                        });

                        ViewBag.items = items;
                    }
                }
                else
                {
                    using (DBModel db = new DBModel())
                    {
                        List<SupplierVM> lstSup = null;
                        
                        lstSup = (from l in db.Suppliers 
                                   join p in db.Products
                                  on l.SupplierID equals p.SupplierID
                                  where p.ProductID==id
                                  select new SupplierVM
                                  {
                                      Id = l.SupplierID,
                                      Nombre= l.CompanyName

                                  }).ToList();

                        List<SelectListItem> items = lstSup.ConvertAll(d => 
                        {
                            return new SelectListItem()
                            {
                                Text = d.Nombre.ToString(),
                                Value = d.Id.ToString(),
                                Selected = false
                            };

                        });

                        ViewBag.noEdit = items;
                    }
                }

                
                return View();
            }
            catch (Exception e)
            {

                throw e;
            }
            

        }

        public ActionResult LlenarCategorias(int id=0)
        {
            try
            {
                if (id==0)
                {
                    using (DBModel db = new DBModel())
                    {
                        db.Configuration.LazyLoadingEnabled = false;
                        List<CategoriasVM> lstSup = null;
                        lstSup = (from l in db.Categories
                                  select new CategoriasVM
                                  {
                                      Id = l.CategoryID,
                                      Nombre    = l.CategoryName

                                  }).ToList();
                        List<SelectListItem> items = lstSup.ConvertAll(d =>
                        {
                            return new SelectListItem()
                            {
                                Text = d.Nombre.ToString(),
                                Value = d.Id.ToString(),
                                Selected = false
                            };

                        });

                        ViewBag.itemsCateg = items;
                    }
                }
                else
                {
                    using (DBModel db = new DBModel())
                    {
                        List<CategoriasVM> lstCateg = null;
                        lstCateg = 
                            (from l in db.Categories
                            join p in db.Products
                            on l.CategoryID equals p.CategoryID
                              where p.ProductID == id
                              select new CategoriasVM
                                  {
                                      Id = l.CategoryID,
                                      Nombre = l.CategoryName

                                  }).ToList();

                        List<SelectListItem> items = lstCateg.ConvertAll(d =>
                        {
                            return new SelectListItem()
                            {
                                Text = d.Nombre.ToString(),
                                Value = d.Id.ToString(),
                                Selected = false
                            };

                        });

                        ViewBag.CategNoEdit = items;
                    }
                }

                
                return View();
            }
            catch (Exception e)
            {

                throw e;
            }
            

        }
    }
}