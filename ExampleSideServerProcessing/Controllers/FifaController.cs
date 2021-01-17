using ExampleSideServerProcessing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExampleSideServerProcessing.Controllers
{
    public class FifaController : Controller
    {
        SSISEntities db = new SSISEntities();

       

        // GET: Fifa
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetPlayerFifa() {

            List<FIFA> play1 = new List<FIFA>();

            play1 = db.FIFAs.ToList();

            return View();
        
        }
    }
}