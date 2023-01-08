using Hospital.Data;
using Hospital.Models;
using Hospital.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hospital.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DoctorController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Docter> objList = _db.Docter.Include(u => u.Category);
            return View(objList);
        }
        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            DoctorVM docterVM = new DoctorVM()
            {
                Docter = new Docter(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                
            };

            if (id == null)
            {
                //this is for create
                return View(docterVM);
            }
            else
            {
                docterVM.Docter = _db.Docter.Find(id);
                if (docterVM.Docter == null)
                {
                    return NotFound();
                }
                return View(docterVM);
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(DoctorVM docterVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (docterVM.Docter.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePathDoctor;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    docterVM.Docter.Image = fileName + extension;

                    _db.Docter.Add(docterVM.Docter);
                }
                else
                {
                    //updating
                    var objFromDb = _db.Docter.AsNoTracking().FirstOrDefault(u => u.Id == docterVM.Docter.Id);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePathDoctor;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        docterVM.Docter.Image = fileName + extension;
                    }
                    else
                    {
                        docterVM.Docter.Image = objFromDb.Image;
                    }
                    _db.Docter.Update(docterVM.Docter);
                }


                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            docterVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            
            return View(docterVM);

        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Docter product = _db.Docter.Include(u => u.Category).FirstOrDefault(u => u.Id == id);
            //product.Category = _db.Category.Find(product.CategoryId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Docter.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePathDoctor;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _db.Docter.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");


        }


    }
}
