using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProject9.Models;
using PagedList.Mvc;
using PagedList;
using System.Net;
using System.Data.Entity;

namespace TestProject9.Controllers
{
    public class VehicleController : Controller
    {
        VehiclesEntities db = new VehiclesEntities();
        // GET: Vehicle
        public ActionResult Index(string searching, int? i)
        {
            List<VehicleMake> makeListing = db.VehicleMakes.ToList();
            List<VehicleModel> modelListing = db.VehicleModels.ToList();

            var joinTables = from vhMake in makeListing
                             join vhModel in modelListing on vhMake.Id equals vhModel.MakeId into table1
                             from vhModel in table1.DefaultIfEmpty()
                             select new BindTablesClass { makeDetails = vhMake, modelDetails = vhModel };

            if (!String.IsNullOrEmpty(searching))
            {
                joinTables = joinTables.Where(vhMake => vhMake.makeDetails.Name.Contains(searching));
            }

            return View(joinTables.ToList().ToPagedList(i ?? 1, 10));
        }
    }
}