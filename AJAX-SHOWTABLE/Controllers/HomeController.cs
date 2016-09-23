using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AJAX_SHOWTABLE;
using AJAX_SHOWTABLE.Models;
using System.Data.Entity;

namespace AJAX_SHOWTABLE.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 顯示所有資料
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowAllData()
        {
            using (EmployeeData2Entities Data = new EmployeeData2Entities())
            {
                IndexViewModel DB = new IndexViewModel();
                DB.DataEmp =
                    (from p in Data.Person
                     join c in Data.CompanyUnit on p.EmpID equals c.EmpID                     
                     select new DataEmp
                     {
                         EmpID = p.EmpID,
                         EmpName = p.EmpName,
                         EmpBirthday = p.EmpBirthday,
                         Gender = p.Gender,
                         BuildDate = p.BuildDate,
                         LogingDate = p.LogingDate,
                         JobTitle = c.JobTitle,
                         UnitID = c.UnitID,
                         UnitName = c.UnitName,
                         Seniority = c.Seniority,
                     }).ToList();

                return View(DB);
            }
  
        }

        public PartialViewResult PartialData(string empid)
        {
            EmployeeData2Entities Empmodel = new EmployeeData2Entities();
            IndexViewModel objresult = new IndexViewModel();

            if (!string.IsNullOrWhiteSpace(empid))
            {
                int intempid = int.Parse(empid);
                objresult.DataEmp =
                 (from p in Empmodel.Person
                  join c in Empmodel.CompanyUnit on p.EmpID equals c.EmpID
                  where p.EmpID == intempid //搜尋id，不用撈整個table
                  select new DataEmp
                  {
                      EmpID = p.EmpID,
                      EmpName = p.EmpName,
                      EmpBirthday = p.EmpBirthday,
                      Gender = p.Gender,
                      BuildDate = p.BuildDate,
                      LogingDate = p.LogingDate,
                      JobTitle = c.JobTitle,
                      UnitID = c.UnitID,
                      UnitName = c.UnitName,
                      Seniority = c.Seniority,

                  }).ToList();
                //objresult.DataEmp = empdata;

                //objresult.DataEmp = objresult.DataEmp.Where(x => x.EmpID == int.Parse(empid)).ToList();

                if (objresult.DataEmp.Count == 0)
                {
                    objresult.IsSuccess = false;
                    objresult.Msg = "No Data can search.";
                }
                else
                {
                    objresult.IsSuccess = true;
                    objresult.Msg = "";
                }
            }

            else //未做任何動作前
            {
                objresult.IsSuccess = false;
                objresult.Msg = "目前尚未搜尋";
            }


            return PartialView(objresult);
        }

        public ActionResult Edit(int empid)
        {
            EmployeeData2Entities empmodel = new EmployeeData2Entities();
            var empdata =
                 (from p in empmodel.Person
                  from c in empmodel.CompanyUnit
                  where p.EmpID == c.EmpID && c.EmpID == empid
                  select new DataEmp
                  {
                      EmpID = p.EmpID,
                      EmpName = p.EmpName,
                      EmpBirthday = p.EmpBirthday,
                      Gender = p.Gender,
                      BuildDate = p.BuildDate,
                      LogingDate = p.LogingDate,
                      JobTitle = c.JobTitle,
                      UnitID = c.UnitID,
                      UnitName = c.UnitName,
                      Seniority = c.Seniority
                  }).FirstOrDefault();

            return View(empdata);
        }
         
        //00
        [HttpPost]
        public JsonResult Edit(DataEmp getdata)
        {
            //if (!string.IsNullOrWhiteSpace(Convert.ToString(getdata.EmpID))) { 
            using (EmployeeData2Entities db = new EmployeeData2Entities())
            {
                Person pedata = db.Person.Where(x => x.EmpID == getdata.EmpID).FirstOrDefault();
                pedata.EmpName = getdata.EmpName;
                pedata.EmpBirthday = getdata.EmpBirthday;
                pedata.Gender = getdata.Gender;
                pedata.BuildDate = getdata.BuildDate;
                pedata.LogingDate = getdata.LogingDate;

                CompanyUnit comdata = db.CompanyUnit.Where(y => y.EmpID == getdata.EmpID).FirstOrDefault();
                comdata.JobTitle = getdata.JobTitle;
                comdata.UnitID = getdata.UnitID;
                comdata.UnitName = getdata.UnitName;
                comdata.Seniority = getdata.Seniority;
                db.SaveChanges();

            }
            return Json(getdata);
        }
        public ActionResult Create()
        {


            return View();
        }

        [HttpPost]
        public JsonResult Create(Person createData1, CompanyUnit createData2)
        {
            using (EmployeeData2Entities db = new EmployeeData2Entities())
            {

                db.Person.Add(createData1);
                db.SaveChanges();
                createData2.EmpID = createData1.EmpID;
                db.CompanyUnit.Add(createData2);
                db.SaveChanges();


            }

            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(int empid)
        {
            using (EmployeeData2Entities db = new EmployeeData2Entities())
            {
                               
                Person pedata = db.Person.Where(x => x.EmpID == empid).FirstOrDefault();
                db.Person.Remove(pedata);
                CompanyUnit comdata = db.CompanyUnit.Where(y => y.EmpID == empid).FirstOrDefault();
                db.CompanyUnit.Remove(comdata);
                db.SaveChanges();

                return RedirectToAction("ShowAllData");
            }
            
        }
        
    }
}
