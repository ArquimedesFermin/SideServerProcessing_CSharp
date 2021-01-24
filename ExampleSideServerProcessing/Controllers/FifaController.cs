using ExampleSideServerProcessing.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ExampleSideServerProcessing.Controllers
{
    public class FifaController : Controller
    {
        Teams_SoccerEntities db = new Teams_SoccerEntities();



        // GET: Fifa
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetPlayerFifa()
        {

            JsonResult jr = new JsonResult();
            int start = Convert.ToInt32(Request["start"]); //Indica el primer registro de la paginación
            int length = Convert.ToInt32(Request["length"]); //Numero de registro que la vista actual puede mostrar
            string searchValue = Request["search[value]"]; //valor de busqueda
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"]; //columna a la que se le debe aplicar el orden
            string sortDirection = Request["order[0][dir]"]; //tipo de orden que se aplicara a la columna

            try
            {

                var playersSoccer = db.FIFA.ToList();
                var total = playersSoccer.Count;

                if (!string.IsNullOrEmpty(searchValue))
                {
                    playersSoccer = playersSoccer.Where(x => x.Nombre.ToLower().Contains(searchValue.ToLower())
                    || x.NombreCompleto.ToLower().Contains(searchValue.ToLower())
                    || x.Club.ToLower().Contains(searchValue.ToLower())
                    || x.Liga.ToLower().Contains(searchValue.ToLower())
                    || x.FechaNacimiento.ToString().Contains(searchValue.ToLower())
                    || x.AlturaCM.ToString().Contains(searchValue.ToLower())
                    || x.PesoKG.ToString().Contains(searchValue.ToLower())
                    || x.Nacionalidad.ToLower().Contains(searchValue.ToLower())
                    || x.ValorEuropa.ToString().Contains(searchValue.ToLower())
                    || x.PiePreferido.ToLower().Contains(searchValue.ToLower())
                    || x.PieEsp.ToLower().Contains(searchValue.ToLower())).ToList();

                }
                var totalFilter = playersSoccer.Count;

                //sorting
                playersSoccer = playersSoccer.OrderBy(sortColumnName + " " + sortDirection).ToList();

                //paging
                playersSoccer = playersSoccer.Skip(start).Take(length).ToList();


                jr = (Json(new { data = playersSoccer, draw = Request["draw"], recordsTotal = total, recordsFiltered = totalFilter }, JsonRequestBehavior.AllowGet));
                jr.MaxJsonLength = int.MaxValue;

                return jr;
            }
            catch (Exception ex)
            {

                throw;
            }

        }




        public ActionResult ExportToCSV()
        {
            var playerSoccer = db.FIFA.ToList();

            var builder = new StringBuilder();
            builder.AppendLine("ID,Nombre,Nombre Completo,Club,Liga,Fecha Nacimiento,AlturaCM,PesoKG,Nacionalidad,ValorEuropa,Pie Preferido");

            foreach (var item in playerSoccer)
            {
                builder.AppendLine($"{item.ID},{item.Nombre},{item.NombreCompleto},{item.Club},{item.Liga},{item.FechaNacimiento},{item.AlturaCM},{item.PesoKG},{item.Nacionalidad},{item.ValorEuropa},{item.PiePreferido}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "DataTable_Example_V1.0.csv");
        }

    }
}