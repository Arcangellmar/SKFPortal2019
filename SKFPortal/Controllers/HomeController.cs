using BLL;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DAL;
using ET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SKFPortal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"];

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DashBoard()
        {
            return View();
        }

        public ActionResult RegistroVacaciones()
        {
            return View();
        }

        public ActionResult VerBoletas()
        {
            return View();
        }

        [HttpGet]
        public FileResult GenerarBoleta()
        {
            //string PathReportes = @"C:\Users\rolan\OneDrive\Documentos\GitHub\SKFPortal2019\SKFPortal\Reportes\";
            string PathReportes = @"D:\Fuentes\SKFPortal2019\SKFPortal\Reportes\";
            //string PathPDF = @"C:\Users\rolan\OneDrive\Documentos\GitHub\SKFPortal2019\SKFPortal\PDF\";
            string PathPDF = @"D:\Fuentes\SKFPortal2019\SKFPortal\PDF\";
            String nombreArchivo = "Boleta" + DateTime.Now.ToString("dd-MM-yyyy") + ".pdf";
            String rutaArchivo = PathPDF + nombreArchivo;

            string DB = "SCIRERH";

            ReportDocument oRep = new ReportDocument();

            ParameterFields parametros = new ParameterFields();
            ParameterField parametro1 = new ParameterField();
            ParameterField parametro2 = new ParameterField();
            ParameterField parametro3 = new ParameterField();
            ParameterDiscreteValue valor1 = new ParameterDiscreteValue();
            ParameterDiscreteValue valor2 = new ParameterDiscreteValue();
            ParameterDiscreteValue valor3 = new ParameterDiscreteValue();


            parametro1.ParameterValueType = ParameterValueKind.StringParameter;
            parametro1.Name = "@Personal";
            valor1.Value = "000152";
            parametro1.CurrentValues.Add(valor1);
            parametros.Add(parametro1);

            parametro2.ParameterValueType = ParameterValueKind.StringParameter;
            parametro2.Name = "@cPeriodo";
            valor2.Value = "0334";
            parametro2.CurrentValues.Add(valor2);
            parametros.Add(parametro2);

            parametro3.ParameterValueType = ParameterValueKind.StringParameter;
            parametro3.Name = "@cProceso";
            valor3.Value = "01";
            parametro3.CurrentValues.Add(valor3);
            parametros.Add(parametro3);

            oRep.Load(PathReportes + "NEO_Boleta_Pago.rpt");

            oRep.SetDatabaseLogon("sa", "@Admin123", "CLRLAP122", DB);

            oRep.ExportToDisk(ExportFormatType.PortableDocFormat, PathPDF + nombreArchivo);

            return File(rutaArchivo, "application/pdf", nombreArchivo);
        }

        [HttpGet]
        public string ObtenerRegistroVacacional()
        {
            SKFBLL skfBLL = new SKFBLL();
            List<EmpleadoSKFVacacionET> listaEmpleados = new List<EmpleadoSKFVacacionET>();
            var _usuarioSKF = (UsuarioSKFET)Session["UsuarioSKF"];

            if (_usuarioSKF != null)
            {
                listaEmpleados = skfBLL.ListarVacaciones(_usuarioSKF.Personal_id);
            }

            return JsonConvert.SerializeObject(listaEmpleados, Formatting.Indented);
        }

      
    }
}