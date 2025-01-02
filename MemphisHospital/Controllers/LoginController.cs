using MemphisHospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemphisHospital.Controllers
{
    public class LoginController : Controller
    {
        HospitalEntities1 DB = new HospitalEntities1();

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            var data = DB.Users.Where(x => x.UserName == user.UserName && x.Password == user.Password);
            if(data != null)
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Enter Details");
            }
            return View();
        }

        public ActionResult Index(string searchquery)
        {
            var data = DB.Patients.ToList();
            if(!string.IsNullOrEmpty(searchquery))
            {
                data = data.Where(x => x.FullName.Contains(searchquery)).ToList();
            }
            return View(data);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var data = DB.Patients.Where(x => x.PatientID == id).FirstOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(Patient patient)
        {
            if(ModelState.IsValid)
            {
                var data =DB.Patients.Where(x => x.PatientID ==  patient.PatientID).FirstOrDefault();
                if(data != null)
                {
                    data.FullName = patient.FullName;
                    data.DateofBirth= patient.DateofBirth;
                    data.Address = patient.Address;
                    data.DateofRegistration = patient.DateofRegistration;
                    data.Email = patient.Email;
                    data.Gender = patient.Gender;
                    DB.SaveChangesAsync();
                    TempData["Sucess"] = "Data for Patient " +data.FullName+ "has been edited successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Data not updated");
                    TempData["Error"] = "Data for Patient " + data.FullName + "hasn't been edited successfully";
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            if(ModelState.IsValid)
            {
                DB.Patients.Add(patient);
                TempData["Success"] = "Patient " +patient.FullName+ " was successfully regitered";
                DB.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Enter details");
                TempData["Error"] = "Patient " + patient.FullName + " wasn't regitered";
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            var data = DB.Patients.Where(x => x.PatientID == id).FirstOrDefault();
            return View(data);
        }
        public ActionResult Delete(int id)
        {
            var data = DB.Patients.Where(x => x.PatientID == id).FirstOrDefault();
            DB.Patients.Remove(data);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}