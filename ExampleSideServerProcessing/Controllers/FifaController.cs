using ExampleSideServerProcessing.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ExampleSideServerProcessing.Controllers
{
    public class FifaController : Controller
    {
        Teams_SoccerEntities db = new Teams_SoccerEntities();



        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtener Jugadores de FIFA
        /// </summary>
        /// <returns>Json</returns>
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

                throw ex;
            }

        }


        /// <summary>
        /// Export to CSV
        /// </summary>
        public ActionResult ExportToCSV()
        {
            var playerSoccer = db.FIFA.ToList();
            var builder = new StringBuilder();
            PropertyInfo[] lst = typeof(FIFA).GetProperties();


            int valueHeader = 1;
            //Header
            foreach (var item in lst)
            {
                if (lst.Length == valueHeader)
                {
                    builder.AppendLine($"{item.Name} ");

                }
                else
                {
                    builder.Append($"{item.Name}, ");

                }

                valueHeader++;
            }

            //Values
            foreach (var item in playerSoccer)
            {

                int valueValues = 1;

                foreach (var propertyInfo in lst)
                {
                    string Valor = propertyInfo.GetValue(item).ToString();
                    if (lst.Length == valueValues)
                    {
                        builder.AppendLine($"{Valor} ");

                    }
                    else
                    {
                        builder.Append($"{Valor}, ");

                    }
                    valueValues++;
                }

            }


            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "DataTable_Example_V0.1.1.csv");
        }

    }
}